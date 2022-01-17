namespace Services.RatingService
{
    using System.Threading.Tasks;

    using ViewModels.Rating;

    public interface IRatingService
    {
        Task<bool> AddAsync(CreateRatingFormModel model);

        Task<bool> ApproveRating(int appointmentId);

        Task<bool> DeleteRating(int appointmentId);

        Task<int> GetRatingsCount();
    }
}
