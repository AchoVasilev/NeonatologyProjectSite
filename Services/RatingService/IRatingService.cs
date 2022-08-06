namespace Services.RatingService;

using System.Collections.Generic;
using System.Threading.Tasks;

using ViewModels.Administration.Rating;
using ViewModels.Rating;

public interface IRatingService
{
    Task<bool> AddAsync(CreateRatingFormModel model);

    Task<bool> ApproveRating(int appointmentId);

    Task<bool> DeleteRating(int appointmentId);

    Task<int> GetRatingsCount();

    Task<ICollection<RatingViewModel>> GetRatings();
}