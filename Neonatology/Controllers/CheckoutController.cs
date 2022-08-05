namespace Neonatology.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Hangfire;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;

    using Services.OfferService;
    using Services.PaymentService;

    using Stripe;
    using Stripe.Checkout;

    using ViewModels.Payment;
    using ViewModels.Stripe;

    [IgnoreAntiforgeryToken]
    public class CheckoutController : BaseController
    {
        private readonly IPaymentService paymentService;
        private readonly IOfferService offerService;
        private readonly IOptions<StripeSettings> options;
        private readonly IStripeClient stripeClient;
        public CheckoutController(IPaymentService paymentService, IOfferService offerService, IOptions<StripeSettings> options)
        {
            this.paymentService = paymentService;
            this.offerService = offerService;
            this.options = options;
            this.stripeClient = new StripeClient(this.options.Value.SecretKey);
        }

        public async Task<IActionResult> Index()
        {
            var model = await this.offerService.GetOnlineConsultationModelAsync();

            return this.View(model);
        }

        [HttpGet("config")]
        public async Task<ConfigResponse> GetConfig()
        {
            // Fetch price from the API
            var service = new PriceService(this.stripeClient);
            var price = await service.GetAsync(this.options.Value.Price);

            // return json: publicKey (env), unitAmount, currency
            return new ConfigResponse
            {
                PublicKey = this.options.Value.PublishableKey,
                UnitAmount = price.UnitAmount,
                Currency = price.Currency,
            };
        }

        [HttpGet("checkout-session")]
        public async Task<Session> GetCheckoutSession(string sessionId)
        {
            var service = new SessionService(this.stripeClient);
            var session = await service.GetAsync(sessionId);

            return session;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCheckoutSession()
        {
            var consultation = await this.offerService.GetOnlineConsultationModelAsync();

            var options = new SessionCreateOptions
            {
                SuccessUrl = "https://pediamedbg.com/checkout/successfulpayment?session_id={{CHECKOUT_SESSION_ID}}",
                CancelUrl = "https://pediamedbg.com/checkout/canceledpayment",
                Mode = "payment",
                CustomerEmail = this.User.FindFirst(ClaimTypes.Email).Value,
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        Quantity = 1,
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = (long)(consultation.Price * 100),
                            Currency = "bgn",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = consultation.Name,
                                Images = new List<string>
                                {
                                    "https://res.cloudinary.com/dpo3vbxnl/image/upload/v1641585449/pediamed/onlineConsultation_vyvebl.jpg"
                                }
                            },
                        },
                    },
                },
                // AutomaticTax = new SessionAutomaticTaxOptions { Enabled = true },
            };

            var service = new SessionService(this.stripeClient);
            var session = await service.CreateAsync(options);
            this.Response.Headers.Add("Location", session.Url);

            await this.paymentService.CreateCheckoutSession(session.Id, session.PaymentIntentId, session.ClientReferenceId);

            return new StatusCodeResult(303);
        }

        [HttpPost("stripe")]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> Webhook()
        {
            var json = await new StreamReader(this.HttpContext.Request.Body).ReadToEndAsync();
            var secret = this.options.Value.SigningSecret;
            Event stripeEvent;
            try
            {
                stripeEvent = EventUtility.ConstructEvent(
                    json, this.Request.Headers["Stripe-Signature"],
                    secret
                );

                //Console.WriteLine($"Webhook notification with type: {stripeEvent.Type} found for {stripeEvent.Id}");
            }
            catch (Exception e)
            {
                return this.BadRequest();
            }

            if (stripeEvent.Type == "checkout.session.completed")
            {
                var session = stripeEvent.Data.Object as Session;

                await this.FulfillOrder(session);
            }

            return this.Ok();
        }

        public IActionResult SuccessfulPayment([FromQuery] string sessionId)
                =>
                    this.View(sessionId);

        public IActionResult CanceledPayment()
            =>
                this.View();

        private async Task FulfillOrder(Session session)
        {
            var model = new CreatePaymentModel()
            {
                CustomerEmail = session.CustomerEmail,
                Charge = session.AmountTotal,
                PaymentStatus = session.PaymentStatus,
                Status = session.Status,
                SessionId = session.Id,
                CustomerId = session.CustomerId,
                OfferedServiceId = await this.offerService.GetOnlineConsultationId()
            };

            var patientId = await this.paymentService.CreatePayment(model);

            BackgroundJob.Schedule(
                () => this.paymentService.ChangePaymentStatus(patientId),
                TimeSpan.FromDays(1));
        }
    }
}
