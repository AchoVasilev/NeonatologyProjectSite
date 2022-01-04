namespace Neonatology.Controllers
{
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;

    using Stripe;
    using Stripe.Checkout;

    using ViewModels.Stripe;

    public class CheckoutController : BaseController
    {
        private readonly IOptions<StripeSettings> settings;
        public CheckoutController(IOptions<StripeSettings> settings)
        {
            this.settings = settings;
            StripeConfiguration.ApiKey = this.settings.Value.SecretKey;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateCheckoutSession()
        {
            var options = new SessionCreateOptions
            {
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                              UnitAmount = 6000,
                              Currency = "bgn",
                              ProductData = new SessionLineItemPriceDataProductDataOptions
                              {
                                Name = "Онлайн консултация",
                              },
                        },
                        
                        Quantity = 1,
                    },
                },

                Mode = "payment",
                SuccessUrl = "https://localhost:5001/Checkout/SuccessfulPayment",
                CancelUrl = "https://localhost:5001/Checkout/CanceledPayment",
            };

            var service = new SessionService();
            Session session = service.Create(options);

            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);
        }

        public IActionResult SuccessfulPayment()
            => View();

        public IActionResult CanceledPayment()
            => View();
    }
}
