namespace Neonatology.Web.Controllers;

using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Services.FeedbackService;

using ViewModels.Feedback;

using static Common.Constants.GlobalConstants.MessageConstants;

public class FeedbackController : BaseController
{
    private readonly IFeedbackService feedbackService;
    private readonly IMapper mapper;

    public FeedbackController(IFeedbackService feedbackService, IMapper mapper)
    {
        this.feedbackService = feedbackService;
        this.mapper = mapper;
    }

    public IActionResult Send() 
        => this.View(new FeedbackInputModel());

    [HttpPost]
    public async Task<IActionResult> Send(FeedbackInputModel model)
    {
        if (!this.ModelState.IsValid)
        {
            return this.View(model);
        }

        var feedbackModel = this.mapper.Map<CreateFeedbackModel>(model);
        await this.feedbackService.CreateFeedback(feedbackModel);

        this.TempData["Message"] = SuccessfulFeedbackSent;

        return this.RedirectToAction("Index", "Home");
    }

    [Authorize]
    public async Task<IActionResult> MyFeedbacks(string email)
    {
        if (this.User.Identity.Name != email)
        {
            return this.NotFound();
        }

        var feedbacks = await this.feedbackService.GetUserFeedbacks(email);

        return this.View(feedbacks);
    }
}