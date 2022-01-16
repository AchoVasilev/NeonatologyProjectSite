namespace Neonatology.Controllers
{
    using System.Threading.Tasks;

    using Infrastructure;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using Services.FeedbackService;

    using ViewModels.Feedback;

    using static Common.GlobalConstants.Messages;

    public class FeedbackController : BaseController
    {
        private readonly IFeedbackService feedbackService;

        public FeedbackController(IFeedbackService feedbackService) 
            => this.feedbackService = feedbackService;

        public IActionResult Send() 
            => View(new FeedbackInputModel());

        [HttpPost]
        public async Task<IActionResult> Send(FeedbackInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await this.feedbackService.CreateFeedback(model);

            this.TempData["Message"] = SuccessfulFeedbackSent;

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> MyFeedbacks(string email)
        {
            var feedbacks = await this.feedbackService.GetUserFeedbacks(email);

            return View(feedbacks);
        }
    }
}
