namespace Services.AppointmentService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    using Data;
    using Data.Models;

    using Microsoft.EntityFrameworkCore;

    using ViewModels.Appointments;

    public class AppointmentService : IAppointmentService
    {
        private readonly NeonatologyDbContext data;
        private readonly IMapper mapper;

        public AppointmentService(NeonatologyDbContext data, IMapper mapper)
        {
            this.data = data;
            this.mapper = mapper;
        }

        public ICollection<AppointmentViewModel> GetAllAppointments()
            => this.data.Appointments
                .Where(x => x.IsDeleted == false)
                .ProjectTo<AppointmentViewModel>(this.mapper.ConfigurationProvider)
                .ToList();

        public ICollection<AppointmentViewModel> GetUserAppointments(string patientId)
            => this.data.Appointments
                .Where(x => x.PatientId == patientId && x.IsDeleted == false)
                .ProjectTo<AppointmentViewModel>(this.mapper.ConfigurationProvider)
                .ToList();

        public ICollection<AppointmentViewModel> GetUpcomingUserAppointments(string patientId)
            => this.data.Appointments
                .Where(x => x.PatientId == patientId && x.IsDeleted == false && x.DateTime.Date > DateTime.UtcNow.Date)
                .ProjectTo<AppointmentViewModel>(this.mapper.ConfigurationProvider)
                .ToList();

        public ICollection<AppointmentViewModel> GetPastUserAppointments(string patientId)
            => this.data.Appointments
                .Where(x => x.PatientId == patientId && x.IsDeleted == false && x.DateTime.Date <= DateTime.UtcNow.Date)
                .ProjectTo<AppointmentViewModel>(this.mapper.ConfigurationProvider)
                .ToList();

        public ICollection<AppointmentViewModel> GetUpcomingDoctorAppointments(string doctorId)
            => this.data.Appointments
                .Where(x => x.DoctorId == doctorId && x.IsDeleted == false && x.DateTime.Date > DateTime.UtcNow.Date)
                .ProjectTo<AppointmentViewModel>(this.mapper.ConfigurationProvider)
                .ToList();

        public ICollection<AppointmentViewModel> GetPastDoctorAppointments(string doctorId)
            => this.data.Appointments
                .Where(x => x.DoctorId == doctorId && x.IsDeleted == false && x.DateTime.Date <= DateTime.UtcNow.Date)
                .ProjectTo<AppointmentViewModel>(this.mapper.ConfigurationProvider)
                .ToList();

        public ICollection<CreateAppointmentModel> GetAllDoctorAppointmentsById(string doctorId)
            => this.data.Appointments
                .Where(x => x.DoctorId == doctorId && x.IsDeleted == false)
                .ProjectTo<CreateAppointmentModel>(this.mapper.ConfigurationProvider)
                .ToList();

        public async Task<bool> AddAsync(string doctorId, CreateAppointmentModel model, DateTime date)
        {
            var doctorAppointment = await this.data.Doctors
                .FirstOrDefaultAsync(x => x.Id == doctorId && x.Appointments.Any(a => a.DateTime == date));

            if (doctorAppointment != null)
            {
                return false;
            }

            var appointment = new Appointment()
            {
                DateTime = date,
                ParentFirstName = model.ParentFirstName,
                ParentLastName = model.ParentLastName,
                ChildFirstName = model.ChildFirstName,
                PhoneNumber = model.PhoneNumber,
                DoctorId = doctorId,
                AppointmentCause = model.AppointmentCause
            };

            await this.data.Appointments.AddAsync(appointment);
            await this.data.SaveChangesAsync();

            return true;
        }

        public async Task<bool> AddAsync(string doctorId, PatientAppointmentCreateModel model, DateTime date)
        {
            var doctorAppointment = await this.data.Doctors
                .FirstOrDefaultAsync(x => x.Id == doctorId && x.Appointments.Any(a => a.DateTime == date));

            if (doctorAppointment != null)
            {
                return false;
            }

            var appointment = new Appointment()
            {
                DateTime = date,
                DoctorId = doctorId,
                PatientId = model.PatientId,
                ChildFirstName = model.ChildFirstName,
                AppointmentCause = model.AppointmentCause
            };

            await this.data.Appointments.AddAsync(appointment);
            await this.data.SaveChangesAsync();

            return true;
        }

        public async Task<AppointmentViewModel> GetUserAppointmentAsync(string userId, int appointmentId)
            => await this.data.Appointments
                        .Where(x => x.Patient.UserId == userId && x.Id == appointmentId)
                        .ProjectTo<AppointmentViewModel>(this.mapper.ConfigurationProvider)
                        .FirstOrDefaultAsync();

        public async Task<Appointment> GetAppointmentByIdAsync(int id)
            => await this.data.Appointments
                        .Where(x => x.Id == id && x.IsDeleted == false)
                        .FirstOrDefaultAsync();
    }
}