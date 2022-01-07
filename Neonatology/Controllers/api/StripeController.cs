namespace Neonatology.Controllers.api
{
    using System.IO;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;

    using Services.DoctorService;
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
        private readonly IDoctorService doctorService;
        private readonly IOfferService offerService;

        public StripeController(
            IPaymentService paymentService,
            IOptions<StripeSettings> settings,
            IDoctorService doctorService,
            IOfferService offerService)
        {
            this.paymentService = paymentService;
            this.settings = settings;
            this.doctorService = doctorService;
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
                return BadRequest(e.Message);
            }
        }

        private async Task FulfillOrder(Session session)
        {
            var model = new CreatePaymentModel();

            model.SenderId = session.CustomerId;
            model.RecepientId = await this.doctorService.GetDoctorId();
            model.OfferedServiceId = await this.offerService.GetOnlineConsultationId();

            await this.paymentService.CreatePayment(model);
        }
    }
}