namespace Neonatology.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using Services.FeedbackService;

    using ViewModels.Feedback;

    using static Common.GlobalConstants.MessageConstants;

    public class FeedbackController : BaseController
    {
        private readonly IFeedbackService feedbackService;

        public FeedbackController(IFeedbackService feedbackService) 
            => this.feedbackService = feedbackService;

        public IActionResult Send() 
            =>
                this.View(new FeedbackInputModel());

        [HttpPost]
        public async Task<IActionResult> Send(FeedbackInputModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            await this.feedbackService.CreateFeedback(model);

            this.TempData["Message"] = SuccessfulFeedbackSent;

            return this.RedirectToAction("Index", "Home");
        }

        [Authorize]
        public async Task<IActionResult> MyFeedbacks(string email)
        {
            if (this.User.Identity.Name != email)
            {
                return this.StatusCode(404);
            }

            var feedbacks = await this.feedbackService.GetUserFeedbacks(email);

            return this.View(feedbacks);
        }
    }
}
