namespace Services.FeedbackService;

using System.Collections.Generic;
using System.Threading.Tasks;
using Common;
using Common.Models;
using ViewModels.Feedback;

public interface IFeedbackService
{
    Task CreateFeedback(CreateFeedbackModel model);

    Task<ICollection<FeedbackViewModel>> GetUserFeedbacks(string email);

    Task<ICollection<FeedbackViewModel>> GetAll();

    Task<OperationResult> Delete(int id);

    Task<FeedbackViewModel> GetById(int id);

    Task SolveFeedback(int id);
}