namespace Services.AppointmentService;

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

using static Common.GlobalConstants.DoctorConstants;

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
            .AsNoTracking()
            .ProjectTo<AppointmentViewModel>(this.mapper.ConfigurationProvider)
            .ToListAsync();

    public async Task<ICollection<AppointmentViewModel>> GetGabrovoAppointments()
        => await this.data.Appointments
            .Where(x => x.IsDeleted == false && x.Address.City.Name == GabrovoCityName)
            .OrderByDescending(x => x.DateTime)
            .AsNoTracking()
            .ProjectTo<AppointmentViewModel>(this.mapper.ConfigurationProvider)
            .ToListAsync();

    public async Task<ICollection<AppointmentViewModel>> GetPlevenAppointments()
        => await this.data.Appointments
            .Where(x => x.IsDeleted == false && x.Address.City.Name == PlevenCityName)
            .OrderByDescending(x => x.DateTime)
            .AsNoTracking()
            .ProjectTo<AppointmentViewModel>(this.mapper.ConfigurationProvider)
            .ToListAsync();

    public async Task<AllAppointmentsViewModel> GetUpcomingUserAppointments(string patientId, int itemsPerPage, int page)
    {
        var appointments = await this.data.Appointments
            .Where(x => x.PatientId == patientId && x.IsDeleted == false && x.DateTime.Date >= DateTime.UtcNow.Date)
            .OrderBy(x => x.DateTime)
            .Skip((page - 1) * itemsPerPage)
            .Take(itemsPerPage)
            .AsNoTracking()
            .ProjectTo<AppointmentViewModel>(this.mapper.ConfigurationProvider)
            .ToListAsync();

        var model = new AllAppointmentsViewModel
        {
            Appointments = appointments,
            ItemCount = appointments.Count,
            ItemsPerPage = itemsPerPage,
            PageNumber = page
        };

        return model;
    }

    public async Task<AllAppointmentsViewModel> GetPastUserAppointments(string patientId, int itemsPerPage, int page)
    {
        var appointments = await this.data.Appointments
            .Where(x => x.PatientId == patientId && x.IsDeleted == false && x.DateTime.Date < DateTime.UtcNow.Date)
            .OrderBy(x => x.DateTime)
            .Skip((page - 1) * itemsPerPage)
            .Take(itemsPerPage)
            .AsNoTracking()
            .ProjectTo<AppointmentViewModel>(this.mapper.ConfigurationProvider)
            .ToListAsync();

        var model = new AllAppointmentsViewModel
        {
            Appointments = appointments,
            ItemCount = appointments.Count,
            ItemsPerPage = itemsPerPage,
            PageNumber = page
        };

        return model;
    }

    public async Task<AllAppointmentsViewModel> GetUpcomingDoctorAppointments(string doctorId, int itemsPerPage, int page)
    {
        var appointments = await this.data.Appointments
            .Where(x => x.DoctorId == doctorId && x.IsDeleted == false && x.DateTime.Date >= DateTime.UtcNow.Date)
            .OrderBy(x => x.DateTime)
            .Skip((page - 1) * itemsPerPage)
            .Take(itemsPerPage)
            .AsNoTracking()
            .ProjectTo<AppointmentViewModel>(this.mapper.ConfigurationProvider)
            .ToListAsync();

        var model = new AllAppointmentsViewModel
        {
            Appointments = appointments,
            ItemCount = appointments.Count,
            ItemsPerPage = itemsPerPage,
            PageNumber = page
        };

        return model;
    }

    public async Task<AllAppointmentsViewModel> GetPastDoctorAppointments(string doctorId, int itemsPerPage, int page)
    {
        var appointments = await this.data.Appointments
            .Where(x => x.DoctorId == doctorId && x.IsDeleted == false && x.DateTime.Date < DateTime.UtcNow.Date)
            .OrderBy(x => x.DateTime)
            .Skip((page - 1) * itemsPerPage)
            .Take(itemsPerPage)
            .AsNoTracking()
            .ProjectTo<AppointmentViewModel>(this.mapper.ConfigurationProvider)
            .ToListAsync();

        var model = new AllAppointmentsViewModel
        {
            Appointments = appointments,
            ItemCount = appointments.Count,
            ItemsPerPage = itemsPerPage,
            PageNumber = page
        };

        return model;
    }

    public async Task<ICollection<TakenAppointmentsViewModel>> GetTakenAppointmentSlots()
        => await this.data.Appointments
            .Where(x => x.DateTime.Date >= DateTime.UtcNow.Date && x.IsDeleted == false)
            .Select(x => new TakenAppointmentsViewModel()
            {
                Start = x.DateTime,
                End = x.End,
                Status = "Зает"
            })
            .ToListAsync();

    public async Task<bool> AddAsync(string doctorId, CreateAppointmentModel model, DateTime startDate, DateTime endDate)
    {
        var doctorAppointment = await this.data.Doctors
            .FirstOrDefaultAsync(x => x.Id == doctorId && x.Appointments.Any(a => a.DateTime.Year == startDate.Year &&
                a.DateTime.Day == startDate.Day &&
                a.DateTime.Hour == startDate.Hour &&
                a.DateTime.Minute == startDate.Minute));

        if (doctorAppointment != null)
        {
            return false;
        }

        var appointment = new Appointment()
        {
            DateTime = startDate,
            End = endDate,
            ParentFirstName = model.ParentFirstName,
            ParentLastName = model.ParentLastName,
            ChildFirstName = model.ChildFirstName,
            PhoneNumber = model.PhoneNumber,
            DoctorId = doctorId,
            AppointmentCauseId = model.AppointmentCauseId,
            AddressId = model.AddressId
        };

        await this.data.Appointments.AddAsync(appointment);
        await this.data.SaveChangesAsync();

        return true;
    }

    public async Task<bool> AddAsync(string doctorId, PatientAppointmentCreateModel model, DateTime startDate, DateTime endDate)
    {
        var doctorAppointment = await this.data.Doctors
            .FirstOrDefaultAsync(x => x.Id == doctorId && x.Appointments.Any(a => a.DateTime.Year == startDate.Year &&
                a.DateTime.Day == startDate.Day &&
                a.DateTime.Hour == startDate.Hour &&
                a.DateTime.Minute == startDate.Minute));

        if (doctorAppointment != null)
        {
            return false;
        }

        var patient = await this.data.Patients
            .FirstOrDefaultAsync(x => x.Id == model.PatientId);

        if (patient == null)
        {
            return false;
        }

        var appointment = new Appointment()
        {
            DateTime = startDate,
            End = endDate,
            DoctorId = doctorId,
            PatientId = model.PatientId,
            ChildFirstName = model.ChildFirstName,
            AppointmentCauseId = model.AppointmentCauseId,
            AddressId = model.AddressId,
            ParentFirstName = patient.FirstName,
            ParentLastName = patient.LastName,
            PhoneNumber = patient.Phone,
        };

        await this.data.Appointments.AddAsync(appointment);
        await this.data.SaveChangesAsync();

        return true;
    }

    public async Task<AppointmentViewModel> GetUserAppointmentAsync(string userId, int appointmentId)
    {
        var appointment = await this.data.Appointments
            .Where(x => x.Patient.UserId == userId &&
                        x.Id == appointmentId &&
                        x.IsDeleted == false)
            .AsNoTracking()
            .FirstOrDefaultAsync();

        var result = this.mapper.Map<AppointmentViewModel>(appointment);

        return result;
    }

    public async Task<Appointment> GetAppointmentByIdAsync(int id)
        => await this.data.Appointments
            .Where(x => x.Id == id && x.IsDeleted == false)
            .Include(x => x.AppointmentCause)
            .Include(x => x.Patient)
            .Include(x => x.Rating)
            .AsNoTracking()
            .FirstOrDefaultAsync();

    public async Task<ICollection<AppointmentViewModel>> GetTodaysAppointments(string doctorUserId)
    {
        var appointments = await this.data.Appointments
            .Where(x => x.Doctor.UserId == doctorUserId &&
                        x.DateTime.Date == DateTime.Now.Date &&
                        x.IsDeleted == false)
            .AsNoTracking()
            .ProjectTo<AppointmentViewModel>(this.mapper.ConfigurationProvider)
            .ToListAsync();

        return appointments;
    }

    public async Task<int> GetTotalAppointmentsCount()
        => await this.data.Appointments.CountAsync(x => x.IsDeleted == false);

    public async Task<bool> DeleteAppointment(int appointmentId)
    {
        var appointment = await this.data.Appointments
            .FirstOrDefaultAsync(x => x.Id == appointmentId && x.IsDeleted == false);

        if (appointment == null)
        {
            return false;
        }

        appointment.IsDeleted = true;
        appointment.DeletedOn = DateTime.UtcNow;
        await this.data.SaveChangesAsync();

        return true;
    }
}