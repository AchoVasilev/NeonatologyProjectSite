﻿namespace Services.FeedbackService
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using ViewModels.Feedback;

    public interface IFeedbackService
    {
        Task CreateFeedback(FeedbackInputModel model);

        Task<ICollection<FeedbackViewModel>> GetUserFeedbacks(string email);
    }
}
