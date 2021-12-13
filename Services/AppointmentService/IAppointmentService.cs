﻿namespace Services.AppointmentService
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Data.Models;

    using ViewModels.Appointments;

    public interface IAppointmentService
    {
        ICollection<AppointmentViewModel> GetAllAppointments();

        ICollection<CreateAppointmentModel> GetAllDoctorAppointmentsById(string doctorId);

        ICollection<AppointmentViewModel> GetUserAppointments(string patientId);

        ICollection<AppointmentViewModel> GetPastUserAppointments(string patientId);

        ICollection<AppointmentViewModel> GetUpcomingUserAppointments(string patientId);

        ICollection<AppointmentViewModel> GetUpcomingDoctorAppointments(string doctorId);

        ICollection<AppointmentViewModel> GetPastDoctorAppointments(string doctorId);

        Task<bool> AddAsync(string doctorId, CreateAppointmentModel model, DateTime date);

        Task<bool> AddAsync(string doctorId, PatientAppointmentCreateModel model, DateTime date);

        Task<AppointmentViewModel> GetUserAppointmentAsync(string userId, int appointmentId);

        Task<Appointment> GetAppointmentByIdAsync(int id);
    }
}
