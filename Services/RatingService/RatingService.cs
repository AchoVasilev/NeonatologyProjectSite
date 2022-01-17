namespace Services.RatingService
{
    using System.Linq;
    using System.Threading.Tasks;

    using Data;
    using Data.Models;

    using Microsoft.EntityFrameworkCore;

    using Services.AppointmentService;

    using ViewModels.Rating;

    public class RatingService : IRatingService
    {
        private readonly NeonatologyDbContext data;
        private readonly IAppointmentService appointmentService;

        public RatingService(NeonatologyDbContext data, IAppointmentService appointmentService)
        {
            this.data = data;
            this.appointmentService = appointmentService;
        }

        public async Task<bool> AddAsync(CreateRatingFormModel model)
        {
            var appointment = await this.appointmentService.GetAppointmentByIdAsync(model.AppointmentId);
            if (appointment == null)
            {
                return false;
            }

            var rating = new Rating()
            {
                AppointmentId = model.AppointmentId,
                Number = model.Number,
                Comment = model.Comment,
                DoctorId = model.DoctorId,
                PatientId = model.PatientId
            };

            appointment.IsRated = true;
            appointment.Rating = rating;

            await this.data.Ratings.AddAsync(rating);
            await this.data.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ApproveRating(int appointmentId)
        {
            var rating = await this.data.Ratings
                .Where(x => x.AppointmentId == appointmentId && x.IsDeleted == false && x.IsConfirmed == false)
                .FirstOrDefaultAsync();

            if (rating == null)
            {
                return false;
            }

            rating.IsConfirmed = true;

            await this.data.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteRating(int appointmentId)
        {
            var rating = await this.data.Ratings
                .Where(x => x.AppointmentId == appointmentId && x.IsDeleted == false && x.IsConfirmed == false)
                .FirstOrDefaultAsync();

            if (rating == null)
            {
                return false;
            }

            rating.IsDeleted = true;

            await this.data.SaveChangesAsync();

            return true;
        }

        public async Task<int> GetRatingsCount()
            => await this.data.Ratings.CountAsync();
    }
}
