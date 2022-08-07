namespace Services.RatingService;

using System.Collections.Generic;
using System.Threading.Tasks;
using Common;
using global::Common.Models;
using ViewModels.Administration.Rating;
using ViewModels.Rating;

public interface IRatingService : ITransientService
{
    Task<OperationResult> AddAsync(CreateRatingModel model);

    Task<OperationResult> ApproveRating(int appointmentId);

    Task<OperationResult> DeleteRating(int appointmentId);

    Task<int> GetRatingsCount();

    Task<ICollection<RatingViewModel>> GetRatings();
}