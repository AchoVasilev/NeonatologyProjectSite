namespace Neonatology.Controllers.api
{
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Mvc;

    using Stripe.Checkout;

    [ApiController]
    [Route("[controller]")]
    public class StripeController : ControllerBase
    {
        [HttpPost]

        public ActionResult Create()
        {
            var domain = "http://localhost:4242";

            var options = new SessionCreateOptions
            {
                LineItems = new List<SessionLineItemOptions>
                {
                  new SessionLineItemOptions
                  {
                    // Provide the exact Price ID (for example, pr_1234) of the product you want to sell
                    Price = "{{PRICE_ID}}",
                    Quantity = 1,
                  },
                },

                Mode = "payment",
                SuccessUrl = domain + "/success.html",
                CancelUrl = domain + "/cancel.html",
            };

            var service = new SessionService();

            Session session = service.Create(options);

            Response.Headers.Add("Location", session.Url);

            return new StatusCodeResult(303);
        }
    }
}