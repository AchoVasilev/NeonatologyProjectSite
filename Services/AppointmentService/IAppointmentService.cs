namespace Services.AppointmentService
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Data.Models;

    using ViewModels.Appointments;

    public interface IAppointmentService
    {
        Task<ICollection<AppointmentViewModel>> GetAllAppointments();

        Task<ICollection<AppointmentViewModel>> GetPastUserAppointments(string patientId);

        Task<ICollection<AppointmentViewModel>> GetUpcomingUserAppointments(string patientId);

        Task<ICollection<AppointmentViewModel>> GetUpcomingDoctorAppointments(string doctorId);

        Task<ICollection<AppointmentViewModel>> GetPastDoctorAppointments(string doctorId);

        Task<ICollection<TakenAppointmentsViewModel>> GetTakenAppointmentSlots();

        Task<bool> AddAsync(string doctorId, CreateAppointmentModel model);

        Task<bool> AddAsync(string doctorId, PatientAppointmentCreateModel model);

        Task<AppointmentViewModel> GetUserAppointmentAsync(string userId, int appointmentId);

        Task<Appointment> GetAppointmentByIdAsync(int id);

        Task<ICollection<AppointmentViewModel>> GetTodaysAppointments(string id);

        Task<int> GetTotalAppointmentsCount();
    }
}
