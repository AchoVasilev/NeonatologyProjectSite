namespace Neonatology.Controllers.api
{
    using System.IO;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using Services.PaymentService;

    using Stripe;
    using Stripe.Checkout;

    [ApiController]
    [Route("[controller]")]
    public class StripeController : ControllerBase
    {
        //https://dashboard.stripe.com/test/webhooks/create?endpoint_location=hosted
        private const string secret = "whsec_eL22wrsBMNRkdm3OehalSdaFWThFT83m";
        private readonly IPaymentService paymentService;

        public StripeController(IPaymentService paymentService)
        {
            this.paymentService = paymentService;
        }

        [HttpPost]
        public async Task<IActionResult> Index()
        {
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
                return BadRequest();
            }
        }

        private void FulfillOrder(Session session)
        {
            // TODO: fill me in
        }
    }
}