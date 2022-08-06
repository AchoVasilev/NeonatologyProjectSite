namespace Neonatology.Areas.Administration.Controllers;

using System.Threading.Tasks;

using global::Services.FeedbackService;

using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;

using ViewModels.Feedback;

using static Common.GlobalConstants.MessageConstants;

public class FeedbackController : BaseController
{
    private readonly IFeedbackService feedbackService;
    private readonly IEmailSender emailSender;

    public FeedbackController(IFeedbackService feedbackService, IEmailSender emailSender)
    {
        this.feedbackService = feedbackService;
        this.emailSender = emailSender;
    }

    public async Task<IActionResult> All()
    {
        var model = await this.feedbackService.GetAll();

        return this.View(model);
    }

    public async Task<IActionResult> Delete(int id)
    {
        var result = await this.feedbackService.Delete(id);
        if (result == false)
        {
            this.TempData["Message"] = ErrorDeletingMsg;
            return this.RedirectToAction(nameof(this.All));
        }

        this.TempData["Message"] = SuccessfulDeleteMsg;

        return this.RedirectToAction(nameof(this.All));
    }

    public async Task<IActionResult> Information(int id)
    {
        var model = await this.feedbackService.GetById(id);

        return this.View(model);
    }

    public async Task<IActionResult> Reply(int id)
    {
        var model = await this.feedbackService.GetById(id);
        return this.View(new FeedbackReplyModel
        {
            ReceiverFirstName = model.FirstName,
            ReceiverLastName = model.LastName,
            ReceiverEmail = model.Email,
            Subject = model.Type,
            FeedbackId = id
        });
    }

    [HttpPost]
    public async Task<IActionResult> Reply(FeedbackReplyModel model)
    {
        await this.feedbackService.SolveFeedback(model.FeedbackId);

        await this.emailSender.SendEmailAsync(model.ReceiverEmail, model.Subject, model.Text);
        this.TempData["Message"] = SuccessfulSendEmailMsg;

        return this.RedirectToAction(nameof(this.All));
    }
}