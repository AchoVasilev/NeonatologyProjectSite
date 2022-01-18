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

        public async Task<ICollection<AppointmentViewModel>> GetAllAppointments()
            => await this.data.Appointments
                        .Where(x => x.IsDeleted == false)
                        .OrderByDescending(x => x.DateTime)
                        .ProjectTo<AppointmentViewModel>(this.mapper.ConfigurationProvider)
                        .ToListAsync();

        //public async Task<ICollection<AppointmentViewModel>> GetUserAppointments(string patientId)
        //    => await this.data.Appointments
        //            .Where(x => x.PatientId == patientId && x.IsDeleted == false)
        //            .ProjectTo<AppointmentViewModel>(this.mapper.ConfigurationProvider)
        //            .ToListAsync();

        public async Task<ICollection<AppointmentViewModel>> GetUpcomingUserAppointments(string patientId)
            => await this.data.Appointments
                        .Where(x => x.PatientId == patientId && x.IsDeleted == false && x.DateTime.Date > DateTime.UtcNow.Date)
                        .ProjectTo<AppointmentViewModel>(this.mapper.ConfigurationProvider)
                        .ToListAsync();

        public async Task<ICollection<AppointmentViewModel>> GetPastUserAppointments(string patientId)
            => await this.data.Appointments
                        .Where(x => x.PatientId == patientId && x.IsDeleted == false && x.DateTime.Date <= DateTime.UtcNow.Date)
                        .ProjectTo<AppointmentViewModel>(this.mapper.ConfigurationProvider)
                        .ToListAsync();

        public async Task<ICollection<AppointmentViewModel>> GetUpcomingDoctorAppointments(string doctorId)
            => await this.data.Appointments
                        .Where(x => x.DoctorId == doctorId && x.IsDeleted == false && x.DateTime.Date > DateTime.UtcNow.Date)
                        .ProjectTo<AppointmentViewModel>(this.mapper.ConfigurationProvider)
                        .ToListAsync();

        public async Task<ICollection<AppointmentViewModel>> GetPastDoctorAppointments(string doctorId)
            => await this.data.Appointments
                        .Where(x => x.DoctorId == doctorId && x.IsDeleted == false && x.DateTime.Date <= DateTime.UtcNow.Date)
                        .ProjectTo<AppointmentViewModel>(this.mapper.ConfigurationProvider)
                        .ToListAsync();

        //public async Task<ICollection<CreateAppointmentModel>> GetAllDoctorAppointmentsById(string doctorId)
        //    => await this.data.Appointments
        //                .Where(x => x.DoctorId == doctorId && x.IsDeleted == false)
        //                .ProjectTo<CreateAppointmentModel>(this.mapper.ConfigurationProvider)
        //                .ToListAsync();

        public async Task<ICollection<TakenAppointmentsViewModel>> GetTakenAppointmentSlots()
            => await this.data.Appointments
                    .Where(x => x.DateTime.Date >= DateTime.UtcNow.Date)
                    .Select(x => new TakenAppointmentsViewModel()
                    {
                        Start = x.DateTime,
                        End = x.End,
                        Status = "Зает"
                    })
                    .ToListAsync();

        public async Task<bool> AddAsync(string doctorId, CreateAppointmentModel model)
        {
            var doctorAppointment = await this.data.Doctors
                .FirstOrDefaultAsync(x => x.Id == doctorId && x.Appointments.Any(a => a.DateTime == model.Start));

            if (doctorAppointment != null)
            {
                return false;
            }

            var appointment = new Appointment()
            {
                DateTime = model.Start.ToLocalTime(),
                End = model.End.ToLocalTime(),
                ParentFirstName = model.ParentFirstName,
                ParentLastName = model.ParentLastName,
                ChildFirstName = model.ChildFirstName,
                PhoneNumber = model.PhoneNumber,
                DoctorId = doctorId,
                AppointmentCauseId = model.AppointmentCauseId
            };

            await this.data.Appointments.AddAsync(appointment);
            await this.data.SaveChangesAsync();

            return true;
        }

        public async Task<bool> AddAsync(string doctorId, PatientAppointmentCreateModel model)
        {
            var doctorAppointment = await this.data.Doctors
                .FirstOrDefaultAsync(x => x.Id == doctorId && x.Appointments.Any(a => a.DateTime == model.Start));

            if (doctorAppointment != null)
            {
                return false;
            }

            var appointment = new Appointment()
            {
                DateTime = model.Start.ToLocalTime(),
                End = model.End.ToLocalTime(),
                DoctorId = doctorId,
                PatientId = model.PatientId,
                ChildFirstName = model.ChildFirstName,
                AppointmentCauseId = model.AppointmentCauseId
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
                        .Include(x => x.AppointmentCause)
                        .Include(x => x.Patient)
                        .Include(x => x.Rating)
                        .FirstOrDefaultAsync();

        public async Task<ICollection<AppointmentViewModel>> GetTodaysAppointments(string id)
            => await this.data.Appointments
                .Where(x => x.Doctor.UserId == id &&
                        x.DateTime.Date == DateTime.Now.Date &&
                        x.IsDeleted == false)
                .ProjectTo<AppointmentViewModel>(this.mapper.ConfigurationProvider)
                .ToListAsync();

        public async Task<int> GetTotalAppointmentsCount()
            => await this.data.Appointments.CountAsync();

        public async Task<bool> DeleteAppointment(int appointmentId)
        {
            var appointment = await this.data.Appointments
                .FirstOrDefaultAsync(x => x.Id == appointmentId && x.IsDeleted == false);

            if (appointment == null)
            {
                return false;
            }

            appointment.IsDeleted = true;

            await this.data.SaveChangesAsync();

            return true;
        }
    }
}