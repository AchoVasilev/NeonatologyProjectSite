namespace Test.ServiceUnitTests;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Data.Models;
using Helpers;
using Services.AppointmentService;

using Microsoft.EntityFrameworkCore;

using Mocks;

using ViewModels.Appointments;

using Xunit;

public class AppointmentServiceTests
{
    [Fact]
    public async Task GetAllAppointmentsShouldReturnCorrectCount()
    {
        var dataMock = DatabaseMock.Instance;
        var mapperMock = MapperMock.Instance;

        await AppointmentCauseData.GetCauses(dataMock);
        await AddressData.GetOneAddress(dataMock);
        await AppointmentsData.GetAppointmentsWithAddress(dataMock);

        var service = new AppointmentService(dataMock, mapperMock);
        var result = await service.GetAllAppointments();

        Assert.NotNull(result);
        Assert.Equal(3, result.Count);
    }

    [Fact]
    public async Task GetAllAppointmentsShouldReturnCorrectCountIfAnyIsDeleted()
    {
        var dataMock = DatabaseMock.Instance;
        var mapperMock = MapperMock.Instance;

        await AppointmentCauseData.GetCauses(dataMock);
        await AddressData.GetOneAddress(dataMock);
        await AppointmentsData.GetAppointmentsWithOneDeleted(dataMock);

        var service = new AppointmentService(dataMock, mapperMock);
        var result = await service.GetAllAppointments();

        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task GetAllAppointmentsShouldReturnCorrectModel()
    {
        var dataMock = DatabaseMock.Instance;
        var mapperMock = MapperMock.Instance;

        await AppointmentCauseData.GetCauses(dataMock);
        await AddressData.GetOneAddress(dataMock);
        await AppointmentsData.GetAppointments(dataMock);

        var service = new AppointmentService(dataMock, mapperMock);
        var result = await service.GetAllAppointments();

        Assert.NotNull(result);
        Assert.IsAssignableFrom<ICollection<AppointmentViewModel>>(result);
    }

    [Fact]
    public async Task GetUpcomingUserAppointmentsShouldReturnCorrectCount()
    {
        var dataMock = DatabaseMock.Instance;
        var mapperMock = MapperMock.Instance;

        await PatientData.OnePatient(dataMock);
        await AppointmentCauseData.GetCauses(dataMock);
        await AppointmentsData.AppointmentsWithPatientId(dataMock);

        var service = new AppointmentService(dataMock, mapperMock);
        var result = await service.GetUpcomingUserAppointments("pat", 8, 1);

        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetUpcomingUserAppointmentsShouldReturnCorrectCountIfOneIsDeleted()
    {
        var dataMock = DatabaseMock.Instance;
        var mapperMock = MapperMock.Instance;

        await PatientData.OnePatient(dataMock);
        await AppointmentCauseData.GetCauses(dataMock);
        await AppointmentsData.AppointmentsWithPatientId(dataMock, true);

        var service = new AppointmentService(dataMock, mapperMock);
        var result = await service.GetUpcomingUserAppointments("pat", 8, 1);

        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetUpcomingUserAppointmentsShouldReturnCorrectModel()
    {
        var dataMock = DatabaseMock.Instance;
        var mapperMock = MapperMock.Instance;

        await PatientData.OnePatient(dataMock);
        await AppointmentCauseData.GetCauses(dataMock);
        await AppointmentsData.AppointmentsWithPatientId(dataMock);

        var service = new AppointmentService(dataMock, mapperMock);
        var result = await service.GetUpcomingUserAppointments("pat", 8, 1);

        Assert.NotNull(result);
        Assert.IsAssignableFrom<AllAppointmentsViewModel>(result);
    }

    [Fact]
    public async Task GetPastUserAppointmentsShouldReturnCorrectCount()
    {
        var dataMock = DatabaseMock.Instance;
        var mapperMock = MapperMock.Instance;

        await PatientData.OnePatient(dataMock);
        await AppointmentCauseData.GetCauses(dataMock);
        await AppointmentsData.AppointmentsWithPatientIdWithNegativeDays(dataMock);

        var service = new AppointmentService(dataMock, mapperMock);
        var result = await service.GetPastUserAppointments("pat", 8, 1);

        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetPastUserAppointmentsShouldReturnCorrectCountIfOneIsDeleted()
    {
        var dataMock = DatabaseMock.Instance;
        var mapperMock = MapperMock.Instance;

        await PatientData.OnePatient(dataMock);
        await AppointmentCauseData.GetCauses(dataMock);
        await AppointmentsData.AppointmentsWithPatientIdWithNegativeDays(dataMock, true);

        var service = new AppointmentService(dataMock, mapperMock);
        var result = await service.GetPastUserAppointments("pat", 8, 1);

        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetPastUserAppointmentsShouldReturnCorrectModel()
    {
        var dataMock = DatabaseMock.Instance;
        var mapperMock = MapperMock.Instance;

        await PatientData.OnePatient(dataMock);
        await AppointmentCauseData.GetCauses(dataMock);
        await AppointmentsData.AppointmentsWithPatientIdWithNegativeDays(dataMock);

        var service = new AppointmentService(dataMock, mapperMock);
        var result = await service.GetPastUserAppointments("pat", 8, 1);

        Assert.NotNull(result);
        Assert.IsAssignableFrom<AllAppointmentsViewModel>(result);
    }

    [Fact]
    public async Task GetUpcomingDoctorAppointmentsShouldReturnCorrectCount()
    {
        var dataMock = DatabaseMock.Instance;
        var mapperMock = MapperMock.Instance;

        await DoctorData.OneDoctor(dataMock);
        await AppointmentCauseData.GetCauses(dataMock);
        await AppointmentsData.AppointmentsWithDoctorId(dataMock);

        var service = new AppointmentService(dataMock, mapperMock);
        var result = await service.GetUpcomingDoctorAppointments("doc", 8, 1);

        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetUpcomingDoctorAppointmentsShouldReturnCorrectCountIfOneIsDeleted()
    {
        var dataMock = DatabaseMock.Instance;
        var mapperMock = MapperMock.Instance;

        await DoctorData.OneDoctor(dataMock);
        await AppointmentCauseData.GetCauses(dataMock);
        await AppointmentsData.AppointmentsWithDoctorId(dataMock, true);

        var service = new AppointmentService(dataMock, mapperMock);
        var result = await service.GetUpcomingDoctorAppointments("doc", 8, 1);

        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetUpcomingDoctorAppointmentsShouldReturnCorrectModel()
    {
        var dataMock = DatabaseMock.Instance;
        var mapperMock = MapperMock.Instance;

        await DoctorData.OneDoctor(dataMock);
        await AppointmentCauseData.GetCauses(dataMock);
        await AppointmentsData.AppointmentsWithDoctorId(dataMock);

        var service = new AppointmentService(dataMock, mapperMock);
        var result = await service.GetUpcomingDoctorAppointments("doc", 8, 1);

        Assert.NotNull(result);
        Assert.IsAssignableFrom<AllAppointmentsViewModel>(result);
    }

    [Fact]
    public async Task GetPastDoctorAppointmentsShouldReturnCorrectCount()
    {
        var dataMock = DatabaseMock.Instance;
        var mapperMock = MapperMock.Instance;

        await DoctorData.OneDoctor(dataMock);
        await AppointmentCauseData.GetCauses(dataMock);
        await AppointmentsData.AppointmentsWithDoctorIdWithNegativeDays(dataMock);

        var service = new AppointmentService(dataMock, mapperMock);
        var result = await service.GetPastDoctorAppointments("doc", 8, 1);

        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetPastDoctorAppointmentsShouldReturnCorrectCountIfOneIsDeleted()
    {
        var dataMock = DatabaseMock.Instance;
        var mapperMock = MapperMock.Instance;

        await DoctorData.OneDoctor(dataMock);
        await AppointmentCauseData.GetCauses(dataMock);
        await AppointmentsData.AppointmentsWithDoctorIdWithNegativeDays(dataMock, true);

        var service = new AppointmentService(dataMock, mapperMock);
        var result = await service.GetPastDoctorAppointments("doc", 8, 1);

        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetPastDoctorAppointmentsShouldReturnCorrectModel()
    {
        var dataMock = DatabaseMock.Instance;
        var mapperMock = MapperMock.Instance;

        await DoctorData.OneDoctor(dataMock);
        await AppointmentCauseData.GetCauses(dataMock);
        await AppointmentsData.AppointmentsWithDoctorIdWithNegativeDays(dataMock);

        var service = new AppointmentService(dataMock, mapperMock);
        var result = await service.GetPastDoctorAppointments("doc", 8, 1);

        Assert.NotNull(result);
        Assert.IsAssignableFrom<AllAppointmentsViewModel>(result);
    }

    [Fact]
    public async Task GetTakenAppointmentSlotsShouldReturnCorrectCount()
    {
        var dataMock = DatabaseMock.Instance;
        var mapperMock = MapperMock.Instance;

        await AppointmentCauseData.GetCauses(dataMock);
        await AppointmentsData.GetAppointments(dataMock);

        var service = new AppointmentService(dataMock, mapperMock);
        var result = await service.GetTakenAppointmentSlots();

        Assert.NotNull(result);
        Assert.Equal(3, result.Count);
    }

    [Fact]
    public async Task GetTakenAppointmentSlotsShouldReturnCorrectCountIfOneIsDeleted()
    {
        var dataMock = DatabaseMock.Instance;
        var mapperMock = MapperMock.Instance;
        
        await AppointmentCauseData.GetCauses(dataMock);
        await AppointmentsData.GetTakenAppointments(dataMock, true);

        var service = new AppointmentService(dataMock, mapperMock);
        var result = await service.GetTakenAppointmentSlots();

        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task GetTakenAppointmentSlotsShouldReturnCorrectCountIfDateTimeIsBeforeNow()
    {
        var dataMock = DatabaseMock.Instance;
        var mapperMock = MapperMock.Instance;

        await AppointmentCauseData.GetCauses(dataMock);
        await AppointmentsData.GetAppointmentsWithOneNegativeDay(dataMock);

        var service = new AppointmentService(dataMock, mapperMock);
        var result = await service.GetTakenAppointmentSlots();

        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task GetTakenAppointmentsShouldReturnCorrectModel()
    {
        var dataMock = DatabaseMock.Instance;
        var mapperMock = MapperMock.Instance;
        
        await AppointmentCauseData.GetCauses(dataMock);
        await AppointmentsData.GetAppointmentsWithThreeDifferentDates(dataMock);

        var service = new AppointmentService(dataMock, mapperMock);
        var result = await service.GetTakenAppointmentSlots();

        Assert.NotNull(result);
        Assert.IsAssignableFrom<ICollection<TakenAppointmentsViewModel>>(result);
    }

    [Fact]
    public async Task GetTakenAppointmentSlotsShouldReturnModelWithCorrectStatus()
    {
        var dataMock = DatabaseMock.Instance;
        var mapperMock = MapperMock.Instance;

        await AppointmentCauseData.GetCauses(dataMock);
        await AppointmentsData.GetAppointmentsWithThreeDifferentDates(dataMock);

        var service = new AppointmentService(dataMock, mapperMock);
        var result = await service.GetTakenAppointmentSlots();

        Assert.NotNull(result);
        Assert.Equal("Зает", result.First().Status);
    }

    [Fact]
    public async Task AddShouldReturnTrueIfModelIsValid()
    {
        var dataMock = DatabaseMock.Instance;
        var mapperMock = MapperMock.Instance;

        await DoctorData.OneDoctor(dataMock);
        await AppointmentCauseData.GetCauses(dataMock);

        var model = new CreateAppointmentServiceModel()
        {
            AppointmentCauseId = 1,
            ChildFirstName = "Evlogi",
            ParentFirstName = "Mancho",
            ParentLastName = "Vanev",
            PhoneNumber = "098785623",
            DoctorId = "doc",
            Email = "gosho@abv.bg",
            Start = "23.04.2022 15:33",
            End = "28.04.2022 15:43"
        };

        var service = new AppointmentService(dataMock, mapperMock);
        var result = await service.AddAsync("doc", model, DateTime.UtcNow, DateTime.UtcNow.AddDays(1));

        Assert.True(result.Succeeded);
        Assert.Equal(1, dataMock.Appointments.Count());
    }

    [Fact]
    public async Task AddAsyncShouldReturnFalseIfAppointmentAtTheSameTimeExists()
    {
        var dataMock = DatabaseMock.Instance;
        var mapperMock = MapperMock.Instance;
        
        await DoctorData.OneDoctor(dataMock);
        await AppointmentCauseData.GetCauses(dataMock);

        var appointment = new Appointment()
        {
            AppointmentCauseId = 1,
            ChildFirstName = "Evlogi",
            ParentFirstName = "Mancho",
            ParentLastName = "Vanev",
            PhoneNumber = "098785623",
            DoctorId = "doc",
            DateTime = DateTime.ParseExact("29.01.2022 08:30", "dd.MM.yyyy HH:mm", null),
            End = DateTime.ParseExact("03.02.2022 08:40", "dd.MM.yyyy HH:mm", null)
        };

        await dataMock.Appointments.AddAsync(appointment);
        await dataMock.SaveChangesAsync();

        var model = new CreateAppointmentServiceModel()
        {
            AppointmentCauseId = 1,
            ChildFirstName = "Evlogi",
            ParentFirstName = "Mancho",
            ParentLastName = "Vanev",
            PhoneNumber = "098785623",
            DoctorId = "doc",
            Email = "gosho@abv.bg",
            Start = "29.01.2022 08:30",
            End = "03.02.2022 08:40"
        };

        var service = new AppointmentService(dataMock, mapperMock);
        var result = await service.AddAsync("doc", model, DateTime.ParseExact(model.Start, "dd.MM.yyyy HH:mm", null),
            DateTime.ParseExact(model.End, "dd.MM.yyyy HH:mm", null));

        Assert.True(result.Failed);
    }

    [Fact]
    public async Task AddAsyncForPatientsShouldReturnTrueIfModelIsValid()
    {
        var dataMock = DatabaseMock.Instance;
        var mapperMock = MapperMock.Instance;

        await DoctorData.OneDoctor(dataMock);
        await AppointmentCauseData.GetCauses(dataMock);
        await PatientData.OnePatient(dataMock);
        
        var model = new CreatePatientAppointmentModel()
        {
            PatientId = "pat",
            AppointmentCauseId = 1,
            ChildFirstName = "Evlogi",
            DoctorId = "doc",
            Start = "10.10.2022 15:30",
            End = "15.10.2022 15:35"
        };

        var service = new AppointmentService(dataMock, mapperMock);
        var result = await service.AddAsync("doc", model, DateTime.ParseExact(model.Start, "dd.MM.yyyy HH:mm", null),
            DateTime.ParseExact(model.End, "dd.MM.yyyy HH:mm", null));

        Assert.True(result.Succeeded);
        Assert.Equal(1, dataMock.Appointments.Count());
    }

    [Fact]
    public async Task AddAsyncForPatientsShouldReturnFalseIfAppointmentAtTheSameTimeExists()
    {
        var dataMock = DatabaseMock.Instance;
        var mapperMock = MapperMock.Instance;

        await DoctorData.OneDoctor(dataMock);
        await AppointmentCauseData.GetCauses(dataMock);

        var appointment = new Appointment()
        {
            AppointmentCauseId = 1,
            ChildFirstName = "Evlogi",
            ParentFirstName = "Mancho",
            ParentLastName = "Vanev",
            PhoneNumber = "098785623",
            DoctorId = "doc",
            DateTime = new DateTime(2022, 01, 29, 08, 30, 00),
            End = new DateTime(2022, 01, 29, 08, 40, 00)
        };

        await dataMock.Appointments.AddAsync(appointment);
        await dataMock.SaveChangesAsync();

        var model = new CreatePatientAppointmentModel()
        {
            AppointmentCauseId = 1,
            ChildFirstName = "Evlogi",
            DoctorId = "doc",
            Start = "29.01.2022 08:30",
            End = "29.01.2022 08:40"
        };

        var service = new AppointmentService(dataMock, mapperMock);
        var result = await service.AddAsync("doc", model, DateTime.ParseExact(model.Start, "dd.MM.yyyy HH:mm", null),
            DateTime.ParseExact(model.End, "dd.MM.yyyy HH:mm", null));

        Assert.True(result.Failed);
    }

    [Fact]
    public async Task GetUserAppointmentShouldReturnCorrectAppointment()
    {
        var dataMock = DatabaseMock.Instance;
        var mapperMock = MapperMock.Instance;

        await PatientData.OnePatient(dataMock);
        await AppointmentCauseData.GetCauses(dataMock);
        await AppointmentsData.AppointmentsWithPatientId(dataMock);

        var service = new AppointmentService(dataMock, mapperMock);
        var result = await service.GetUserAppointmentAsync("patpat", 3);

        Assert.NotNull(result);
        Assert.Equal(3, result.Id);
    }

    [Fact]
    public async Task GetUserAppointmentShouldReturnCorrectAppointmentModel()
    {
        var dataMock = DatabaseMock.Instance;
        var mapperMock = MapperMock.Instance;

        await PatientData.OnePatient(dataMock);
        await AppointmentCauseData.GetCauses(dataMock);
        await AppointmentsData.AppointmentsWithPatientId(dataMock);
        
        var service = new AppointmentService(dataMock, mapperMock);
        var result = await service.GetUserAppointmentAsync("patpat", 3);

        Assert.NotNull(result);
        Assert.IsType<AppointmentViewModel>(result);
    }

    [Fact]
    public async Task GetUserAppointmentShouldReturnNullIfUserIdIsWrong()
    {
        var dataMock = DatabaseMock.Instance;
        var mapperMock = MapperMock.Instance;

        await PatientData.OnePatient(dataMock);
        await AppointmentCauseData.GetCauses(dataMock);
        await AppointmentsData.AppointmentsWithPatientId(dataMock);

        var service = new AppointmentService(dataMock, mapperMock);
        var result = await service.GetUserAppointmentAsync("pat", 3);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetUserAppointmentShouldReturnNullIfAppointmentIdIsWrong()
    {
        var dataMock = DatabaseMock.Instance;
        var mapperMock = MapperMock.Instance;

        await PatientData.OnePatient(dataMock);
        await AppointmentCauseData.GetCauses(dataMock);
        await AppointmentsData.AppointmentsWithPatientId(dataMock);

        var service = new AppointmentService(dataMock, mapperMock);
        var result = await service.GetUserAppointmentAsync("patpat", 5);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetUserAppointmentShouldReturnNullIfAppointmentIsDeleted()
    {
        var dataMock = DatabaseMock.Instance;
        var mapperMock = MapperMock.Instance;

        await PatientData.OnePatient(dataMock);
        await AppointmentCauseData.GetCauses(dataMock);
        await AppointmentsData.AppointmentsWithPatientId(dataMock, true);

        var service = new AppointmentService(dataMock, mapperMock);
        var result = await service.GetUserAppointmentAsync("patpat", 1);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetAppointmentByIdShouldReturnCorrectResult()
    {
        var dataMock = DatabaseMock.Instance;
        var mapperMock = MapperMock.Instance;

        await PatientData.OnePatient(dataMock);
        await AppointmentCauseData.GetCauses(dataMock);
        await AppointmentsData.AppointmentsWithPatientId(dataMock);

        var service = new AppointmentService(dataMock, mapperMock);
        var result = await service.GetAppointmentByIdAsync(3);

        Assert.NotNull(result);
        Assert.Equal(3, result.Id);
        Assert.Equal("pat", result.PatientId);
    }

    [Fact]
    public async Task GetAppointmentByIdShouldReturnCorrectModel()
    {
        var dataMock = DatabaseMock.Instance;
        var mapperMock = MapperMock.Instance;

        await PatientData.OnePatient(dataMock);
        await AppointmentCauseData.GetCauses(dataMock);
        await AppointmentsData.AppointmentsWithPatientId(dataMock);

        var service = new AppointmentService(dataMock, mapperMock);
        var result = await service.GetAppointmentByIdAsync(3);

        Assert.NotNull(result);
        Assert.IsType<Appointment>(result);
    }

    [Fact]
    public async Task GetAppointmentByIdShouldReturnNullIfIdIsIncorrect()
    {
        var dataMock = DatabaseMock.Instance;
        var mapperMock = MapperMock.Instance;
        
        await PatientData.OnePatient(dataMock);
        await AppointmentCauseData.GetCauses(dataMock);
        await AppointmentsData.AppointmentsWithPatientId(dataMock);
        
        var service = new AppointmentService(dataMock, mapperMock);
        var result = await service.GetAppointmentByIdAsync(5);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetAppointmentByIdShouldReturnNullIfIsDeleted()
    {
        var dataMock = DatabaseMock.Instance;
        var mapperMock = MapperMock.Instance;
        
        await PatientData.OnePatient(dataMock);
        await AppointmentCauseData.GetCauses(dataMock);
        await AppointmentsData.AppointmentsWithPatientId(dataMock, true);

        var service = new AppointmentService(dataMock, mapperMock);
        var result = await service.GetAppointmentByIdAsync(1);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetTodaysAppointmentsShouldReturnCorrectCount()
    {
        var dataMock = DatabaseMock.Instance;
        var mapperMock = MapperMock.Instance;

        await DoctorData.OneDoctor(dataMock);
        await AppointmentCauseData.GetCauses(dataMock);
        await AppointmentsData.AppointmentsWithDoctorIdForToday(dataMock);

        var service = new AppointmentService(dataMock, mapperMock);
        var result = await service.GetTodaysAppointments("docdoc");

        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetTodaysAppointmentsShouldReturnCorrectCountWithDifferentDateTime()
    {
        var dataMock = DatabaseMock.Instance;
        var mapperMock = MapperMock.Instance;

        await DoctorData.OneDoctor(dataMock);
        await AppointmentCauseData.GetCauses(dataMock);

        var appointments = new List<Appointment>()
        {
            new Appointment
            {
                AppointmentCauseId = 1,
                DoctorId = "doc",
                DateTime = DateTime.UtcNow
            },

            new Appointment
            {
                AppointmentCauseId = 2,
                DoctorId = "doc",
                DateTime = DateTime.UtcNow
            },

            new Appointment
            {
                AppointmentCauseId = 3,
                DoctorId = "doc",
                DateTime = DateTime.UtcNow.AddDays(10)
            },
        };

        await dataMock.Appointments.AddRangeAsync(appointments);
        await dataMock.SaveChangesAsync();

        var service = new AppointmentService(dataMock, mapperMock);
        var result = await service.GetTodaysAppointments("docdoc");

        Assert.Equal(2, result.Count);
        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetTodaysAppointmentsShouldReturnCorrectCountIfOneIsDeleted()
    {
        var dataMock = DatabaseMock.Instance;
        var mapperMock = MapperMock.Instance;

        await DoctorData.OneDoctor(dataMock);
        await AppointmentCauseData.GetCauses(dataMock);

        var appointments = new List<Appointment>()
        {
            new Appointment
            {
                AppointmentCauseId = 1,
                DoctorId = "doc",
                DateTime = DateTime.UtcNow
            },

            new Appointment
            {
                AppointmentCauseId = 2,
                DoctorId = "doc",
                DateTime = DateTime.UtcNow,
                IsDeleted = true
            },

            new Appointment
            {
                AppointmentCauseId = 3,
                DoctorId = "doc",
                DateTime = DateTime.UtcNow.AddDays(10)
            },
        };

        await dataMock.Appointments.AddRangeAsync(appointments);
        await dataMock.SaveChangesAsync();

        var service = new AppointmentService(dataMock, mapperMock);
        var result = await service.GetTodaysAppointments("docdoc");

        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetTodaysAppointmentsShouldReturnCorrectModel()
    {
        var dataMock = DatabaseMock.Instance;
        var mapperMock = MapperMock.Instance;

        await DoctorData.OneDoctor(dataMock);
        await AppointmentCauseData.GetCauses(dataMock);
        await AppointmentsData.AppointmentsWithDoctorIdForToday(dataMock);

        var service = new AppointmentService(dataMock, mapperMock);
        var result = await service.GetTodaysAppointments("docdoc");

        Assert.IsAssignableFrom<ICollection<AppointmentViewModel>>(result);
    }

    [Fact]
    public async Task GetAppointmentsCountShouldReturnCorrectCount()
    {
        var dataMock = DatabaseMock.Instance;
        var mapperMock = MapperMock.Instance;

        await DoctorData.OneDoctor(dataMock);
        await AppointmentCauseData.GetCauses(dataMock);
        await AppointmentsData.AppointmentsWithDoctorId(dataMock);

        var service = new AppointmentService(dataMock, mapperMock);
        var result = await service.GetTotalAppointmentsCount();

        Assert.Equal(3, result);
    }

    [Fact]
    public async Task DeleteAppointmentShouldReturnTrueIfEverythingIsCorrect()
    {
        var dataMock = DatabaseMock.Instance;
        var mapperMock = MapperMock.Instance;

        await DoctorData.OneDoctor(dataMock);
        await AppointmentCauseData.GetCauses(dataMock);
        await AppointmentsData.AppointmentsWithDoctorId(dataMock);

        var service = new AppointmentService(dataMock, mapperMock);
        var result = await service.DeleteAppointment(3);

        Assert.True(result.Succeeded);
    }

    [Fact]
    public async Task DeleteAppointmentShouldSetIsDeletedToTrueWhenSuccessful()
    {
        var dataMock = DatabaseMock.Instance;
        var mapperMock = MapperMock.Instance;

        await DoctorData.OneDoctor(dataMock);
        await AppointmentCauseData.GetCauses(dataMock);
        await AppointmentsData.AppointmentsWithDoctorId(dataMock);

        var service = new AppointmentService(dataMock, mapperMock);
        var result = await service.DeleteAppointment(3);

        var model = await dataMock.Appointments
            .FirstOrDefaultAsync(x => x.Id == 3);

        Assert.True(model.IsDeleted);
    }

    [Fact]
    public async Task DeleteAppointmentShouldReturnFalseIfAppointmentDoesNotExist()
    {
        var dataMock = DatabaseMock.Instance;
        var mapperMock = MapperMock.Instance;

        await DoctorData.OneDoctor(dataMock);
        await AppointmentCauseData.GetCauses(dataMock);
        await AppointmentsData.AppointmentsWithDoctorId(dataMock);

        var service = new AppointmentService(dataMock, mapperMock);
        var result = await service.DeleteAppointment(5);

        Assert.True(result.Failed);
    }

    [Fact]
    public async Task DeleteAppointmentShouldReturnFalseIfAppointmentIsAlreadyDeleted()
    {
        var dataMock = DatabaseMock.Instance;
        var mapperMock = MapperMock.Instance;

        await DoctorData.OneDoctor(dataMock);
        await AppointmentCauseData.GetCauses(dataMock);
        await AppointmentsData.AppointmentsWithDoctorId(dataMock, true);

        var service = new AppointmentService(dataMock, mapperMock);
        var result = await service.DeleteAppointment(1);

        Assert.True(result.Failed);
    }
}