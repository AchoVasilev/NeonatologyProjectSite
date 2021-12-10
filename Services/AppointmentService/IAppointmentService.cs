namespace Services.AppointmentService
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using ViewModels.Appointments;

    public interface IAppointmentService
    {
        ICollection<AppointmentViewModel> GetAllPatientAppointmentsById(string patientId);

        ICollection<AppointmentViewModel> GetAllDoctorAppointmentsById(string doctorId);

        Task<bool> AddAsync(string doctorId, AppointmentViewModel model, DateTime date);

        Task<bool> AddAsync(string doctorId, string patientId, DateTime date);
    }
}
