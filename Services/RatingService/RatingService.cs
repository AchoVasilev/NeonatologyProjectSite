namespace Services.RatingService
{
    using System.Threading.Tasks;

    using Data;
    using Data.Models;

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
    }
}
