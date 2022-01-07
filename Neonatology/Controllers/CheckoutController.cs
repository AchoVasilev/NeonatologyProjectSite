namespace Neonatology.Controllers
{
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using Services.PaymentService;

    using Stripe.Checkout;

    [Authorize]
    public class CheckoutController : BaseController
    {
        private readonly IPaymentService paymentService;

        public CheckoutController(IPaymentService paymentService)
        {
            this.paymentService = paymentService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> CreateCheckoutSession()
        {
            var options = new SessionCreateOptions
            {
                CustomerEmail = this.User.FindFirst(ClaimTypes.Email).Value,
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = 3000,
                            Currency = "bgn",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = "Онлайн консултация",
                                Images = new List<string>
                                {
                                    "https://res.cloudinary.com/dpo3vbxnl/image/upload/v1641585449/pediamed/onlineConsultation_vyvebl.jpg"
                                }
                            },
                        },
                        
                        Quantity = 1,
                    },
                },
                PaymentIntentData = new SessionPaymentIntentDataOptions
                {
                    CaptureMethod = "automatic",
                },
                PaymentMethodTypes = new List<string>
                {
                    "card"
                },
                Mode = "payment",
                SuccessUrl = "https://localhost:5001/Checkout/SuccessfulPayment",
                CancelUrl = "https://localhost:5001/Checkout/CanceledPayment",
            };

            var service = new SessionService();
            var session = service.Create(options);

            await this.paymentService.CreateChekoutSession(session.Id, session.PaymentIntentId, session.ClientReferenceId);
            Response.Headers.Add("Location", session.Url);

            return new StatusCodeResult(303);
        }

        public IActionResult SuccessfulPayment()
        {
            return View();
        }

        public IActionResult CanceledPayment()
            => View();
    }
}
