namespace Neonatology.Controllers.api
{
    using System;
    using System.IO;
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

    [ApiController]
    [Route("[controller]")]
    public class StripeController : ControllerBase
    {
        //https://dashboard.stripe.com/test/webhooks/create?endpoint_location=hosted
        private readonly IPaymentService paymentService;
        private readonly IOptions<StripeSettings> settings;
        private readonly IOfferService offerService;

        public StripeController(
            IPaymentService paymentService,
            IOptions<StripeSettings> settings,
            IOfferService offerService)
        {
            this.paymentService = paymentService;
            this.settings = settings;
            this.offerService = offerService;
        }

        [HttpPost]
        public async Task<IActionResult> Index()
        {
            string secret = this.settings.Value.AccountSecret;
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            try
            {
                var stripeEvent = EventUtility.ConstructEvent(
                  json,
                  Request.Headers["Stripe-Signature"],
                  secret
                );

                // Handle the checkout.session.completed event
                if (stripeEvent.Type == Events.CheckoutSessionCompleted)
                {
                    var session = stripeEvent.Data.Object as Session;

                    // Fulfill the purchase...
                    await this.FulfillOrder(session);
                }

                return Ok();
            }
            catch (StripeException e)
            {
                return BadRequest();
            }
        }

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