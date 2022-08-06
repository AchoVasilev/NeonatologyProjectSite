namespace Services.AppointmentService;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common;
using Common.Models;
using Data.Models;

using ViewModels.Appointments;

public interface IAppointmentService
{
    Task<ICollection<AppointmentViewModel>> GetAllAppointments();

    Task<ICollection<AppointmentViewModel>> GetGabrovoAppointments();

    Task<ICollection<AppointmentViewModel>> GetPlevenAppointments();

    Task<AllAppointmentsViewModel> GetUpcomingUserAppointments(string patientId, int itemsPerPage, int page);

    Task<AllAppointmentsViewModel> GetPastUserAppointments(string patientId, int itemsPerPage, int page);

    Task<AllAppointmentsViewModel> GetUpcomingDoctorAppointments(string doctorId, int itemsPerPage, int page);

    Task<AllAppointmentsViewModel> GetPastDoctorAppointments(string doctorId, int itemsPerPage, int page);

    Task<ICollection<TakenAppointmentsViewModel>> GetTakenAppointmentSlots();

    Task<OperationResult> AddAsync(string doctorId, CreateAppointmentServiceModel model, DateTime startDate, DateTime endDate);

    Task<OperationResult> AddAsync(string doctorId, CreatePatientAppointmentModel model, DateTime startDate, DateTime endDate);

    Task<AppointmentViewModel> GetUserAppointmentAsync(string userId, int appointmentId);

    Task<Appointment> GetAppointmentByIdAsync(int id);

    Task<ICollection<AppointmentViewModel>> GetTodaysAppointments(string doctorUserId);

    Task<int> GetTotalAppointmentsCount();

    Task<OperationResult> DeleteAppointment(int appointmentId);
}