namespace Neonatology.Controllers;

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
using static Common.Constants.WebConstants.RouteTemplates;
using static Common.Constants.WebConstants.CheckoutConstants;

[IgnoreAntiforgeryToken]
public class CheckoutController : BaseController
{
    private readonly IPaymentService paymentService;
    private readonly IOfferService offerService;
    private readonly IOptions<StripeSettings> options;
    private readonly IStripeClient stripeClient;

    public CheckoutController(IPaymentService paymentService, IOfferService offerService,
        IOptions<StripeSettings> options)
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

    [HttpGet]
    [Route(CheckoutConfig)]
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

    [HttpGet]
    [Route(CheckoutGetCheckoutSession)]
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

        var sessionCreateOptions = new SessionCreateOptions
        {
            SuccessUrl = SuccessUrl,
            CancelUrl = CancelUrl,
            Mode = SessionOptionsMode,
            CustomerEmail = this.User.FindFirst(ClaimTypes.Email).Value,
            LineItems = new List<SessionLineItemOptions>
            {
                new SessionLineItemOptions
                {
                    Quantity = Quantity,
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(consultation.Price * Multiplier),
                        Currency = Currency,
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = consultation.Name,
                            Images = new List<string>
                            {
                                OnlineConsultationUrl,
                            }
                        },
                    },
                },
            },
        };

        var service = new SessionService(this.stripeClient);
        var session = await service.CreateAsync(sessionCreateOptions);
        this.Response.Headers.Add(LocationHeader, session.Url);

        await this.paymentService.CreateCheckoutSession(session.Id, session.PaymentIntentId,
            session.ClientReferenceId);

        return new StatusCodeResult(303);
    }

    [HttpPost]
    [Route(CheckoutWebhook)]
    [IgnoreAntiforgeryToken]
    public async Task<IActionResult> Webhook()
    {
        var json = await new StreamReader(this.HttpContext.Request.Body).ReadToEndAsync();
        var secret = this.options.Value.SigningSecret;
        Event stripeEvent;
        try
        {
            stripeEvent = EventUtility.ConstructEvent(
                json, this.Request.Headers[StripeSignatureHeader],
                secret
            );
        }
        catch (Exception e)
        {
            return this.BadRequest();
        }

        if (stripeEvent.Type == CheckoutSessionCompleted)
        {
            var session = stripeEvent.Data.Object as Session;

            await this.FulfillOrder(session);
        }

        return this.Ok();
    }

    public IActionResult SuccessfulPayment([FromQuery] string sessionId)
        => this.View(sessionId);

    public IActionResult CanceledPayment()
        => this.View();

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
            TimeSpan.FromDays(HangFireBackgroundJobDays));
    }
}