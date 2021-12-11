namespace Services.AppointmentService
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using ViewModels.Appointments;

    public interface IAppointmentService
    {
        ICollection<CreateAppointmentModel> GetAllDoctorAppointmentsById(string doctorId);

        ICollection<AppointmentViewModel> GetUserAppointments(string patientId);

        ICollection<AppointmentViewModel> GetPastUserAppointments(string patientId);

        ICollection<AppointmentViewModel> GetUpcomingUserAppointments(string patientId);

        ICollection<AppointmentViewModel> GetUpcomingDoctorAppointments(string doctorId);

        ICollection<AppointmentViewModel> GetPastDoctorAppointments(string doctorId);

        Task<bool> AddAsync(string doctorId, CreateAppointmentModel model, DateTime date);

        Task<bool> AddAsync(string doctorId, PatientAppointmentCreateModel model, DateTime date);
    }
}
