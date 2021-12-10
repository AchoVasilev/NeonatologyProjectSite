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

        public ICollection<AppointmentViewModel> GetAllPatientAppointmentsById(string patientId)
            => this.data.Appointments
                .Where(x => x.PatientId == patientId && x.IsDeleted == false)
                .ProjectTo<AppointmentViewModel>(this.mapper.ConfigurationProvider)
                .ToList();

        public ICollection<AppointmentViewModel> GetAllDoctorAppointmentsById(string doctorId)
            => this.data.Appointments
                .Where(x => x.DoctorId == doctorId && x.IsDeleted == false)
                .ProjectTo<AppointmentViewModel>(this.mapper.ConfigurationProvider)
                .ToList();

        public async Task<bool> AddAsync(string doctorId, AppointmentViewModel model, DateTime date)
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
                IsRated = model.IsRated,
                DoctorId = doctorId
            };

            await this.data.Appointments.AddAsync(appointment);
            await this.data.SaveChangesAsync();

            return true;
        }

        public async Task<bool> AddAsync(string doctorId, string patientId, DateTime date)
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
                PatientId = patientId
            };

            await this.data.Appointments.AddAsync(appointment);
            await this.data.SaveChangesAsync();

            return true;
        }
    }
}