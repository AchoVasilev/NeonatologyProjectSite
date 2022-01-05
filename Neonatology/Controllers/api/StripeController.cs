namespace Neonatology.Controllers.api
{
    using System.IO;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;

    using Services.PaymentService;

    using Stripe;
    using Stripe.Checkout;

    using ViewModels.Stripe;

    [ApiController]
    [Route("[controller]")]
    public class StripeController : ControllerBase
    {
        //https://dashboard.stripe.com/test/webhooks/create?endpoint_location=hosted
        private readonly IPaymentService paymentService;
        private readonly IOptions<StripeSettings> settings;

        public StripeController(IPaymentService paymentService, IOptions<StripeSettings> settings)
        {
            this.paymentService = paymentService;
            this.settings = settings;
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
                    this.FulfillOrder(session);
                }

                return Ok();
            }
            catch (StripeException e)
            {
                return BadRequest(e.Message);
            }
        }

        private void FulfillOrder(Session session)
        {
            // TODO: fill me in
        }
    }
}