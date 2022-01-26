namespace Neonatology.Controllers
{
    using System.Threading.Tasks;

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

        [Authorize]
        public async Task<IActionResult> MyFeedbacks(string email)
        {
            if (this.User.Identity.Name != email)
            {
                return StatusCode(404);
            }

            var feedbacks = await this.feedbackService.GetUserFeedbacks(email);

            return View(feedbacks);
        }
    }
}
