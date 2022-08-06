namespace Test.ServiceUnitTests;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using Mocks;
using Services.AppointmentService;
using Services.RatingService;
using ViewModels.Administration.Rating;
using ViewModels.Appointments;
using ViewModels.Rating;
using Xunit;

public class RatingServiceTests
{
    [Fact]
    public async Task AddShouldWorkCorrectlyAndReturnTrue()
    {
        var dataMock = DatabaseMock.Instance;
        var mapperMock = MapperMock.Instance;

        var doctor = new Doctor()
        {
            Id = "doc",
            FirstName = "Evlogi",
            LastName = "Manev",
            PhoneNumber = "098787862"
        };

        var patient = new Patient()
        {
            Id = "pat",
            FirstName = "Gosho",
            LastName = "Peshev",
            Phone = "0988878264",
        };

        var causes = new List<AppointmentCause>()
        {
            new AppointmentCause { Id = 1, Name = "Start" },
            new AppointmentCause { Id = 2, Name = "End" },
            new AppointmentCause { Id = 3, Name = "Middle" },
        };

        await dataMock.AppointmentCauses.AddRangeAsync(causes);
        await dataMock.Doctors.AddAsync(doctor);
        await dataMock.Patients.AddAsync(patient);
        await dataMock.SaveChangesAsync();

        var model = new PatientAppointmentCreateModel()
        {
            PatientId = "pat",
            AppointmentCauseId = 1,
            ChildFirstName = "Evlogi",
            DoctorId = "doc",
            Start = "10.10.2022 15:30",
            End = "15.10.2022 15:35"
        };

        var appointmentService = new AppointmentService(dataMock, mapperMock);
        await appointmentService.AddAsync(doctor.Id, model, DateTime.ParseExact(model.Start, "dd.MM.yyyy HH:mm", null),
            DateTime.ParseExact(model.End, "dd.MM.yyyy HH:mm", null));

        var appointment = dataMock.Appointments.First();

        var rating = new CreateRatingFormModel()
        {
            AppointmentId = appointment.Id,
            PatientId = "pat",
            Comment = "comment",
            DoctorId = "doc",
            Number = 5
        };

        var ratingService = new RatingService(dataMock, appointmentService, mapperMock);

        var result = await ratingService.AddAsync(rating);
        
        Assert.True(result.Succeeded);
    }
    
    [Fact]
    public async Task AddShouldReturnFalseIfAppointmentDoesntExist()
    {
        var dataMock = DatabaseMock.Instance;
        var mapperMock = MapperMock.Instance;

        var doctor = new Doctor()
        {
            Id = "doc",
            FirstName = "Evlogi",
            LastName = "Manev",
            PhoneNumber = "098787862"
        };

        var patient = new Patient()
        {
            Id = "pat",
            FirstName = "Gosho",
            LastName = "Peshev",
            Phone = "0988878264",
        };

        var causes = new List<AppointmentCause>()
        {
            new AppointmentCause { Id = 1, Name = "Start" },
            new AppointmentCause { Id = 2, Name = "End" },
            new AppointmentCause { Id = 3, Name = "Middle" },
        };

        await dataMock.AppointmentCauses.AddRangeAsync(causes);
        await dataMock.Doctors.AddAsync(doctor);
        await dataMock.Patients.AddAsync(patient);
        await dataMock.SaveChangesAsync();

        var model = new PatientAppointmentCreateModel()
        {
            PatientId = "pat",
            AppointmentCauseId = 1,
            ChildFirstName = "Evlogi",
            DoctorId = "doc",
            Start = "10.10.2022 15:30",
            End = "15.10.2022 15:35"
        };

        var appointmentService = new AppointmentService(dataMock, mapperMock);
        await appointmentService.AddAsync(doctor.Id, model, DateTime.ParseExact(model.Start, "dd.MM.yyyy HH:mm", null),
            DateTime.ParseExact(model.End, "dd.MM.yyyy HH:mm", null));

        var rating = new CreateRatingFormModel()
        {
            AppointmentId = 15,
            PatientId = "pat",
            Comment = "comment",
            DoctorId = "doc",
            Number = 5
        };

        var ratingService = new RatingService(dataMock, appointmentService, mapperMock);

        var result = await ratingService.AddAsync(rating);
        
        Assert.True(result.Failed);
    }

    [Fact]
    public async Task ApproveRatingShouldWorkCorrectlyAndReturnTrue()
    {
        var dataMock = DatabaseMock.Instance;
        var mapperMock = MapperMock.Instance;

        var doctor = new Doctor()
        {
            Id = "doc",
            FirstName = "Evlogi",
            LastName = "Manev",
            PhoneNumber = "098787862"
        };

        var patient = new Patient()
        {
            Id = "pat",
            FirstName = "Gosho",
            LastName = "Peshev",
            Phone = "0988878264",
        };

        var causes = new List<AppointmentCause>()
        {
            new AppointmentCause { Id = 1, Name = "Start" },
            new AppointmentCause { Id = 2, Name = "End" },
            new AppointmentCause { Id = 3, Name = "Middle" },
        };

        await dataMock.AppointmentCauses.AddRangeAsync(causes);
        await dataMock.Doctors.AddAsync(doctor);
        await dataMock.Patients.AddAsync(patient);
        await dataMock.SaveChangesAsync();

        var model = new PatientAppointmentCreateModel()
        {
            PatientId = "pat",
            AppointmentCauseId = 1,
            ChildFirstName = "Evlogi",
            DoctorId = "doc",
            Start = "10.10.2022 15:30",
            End = "15.10.2022 15:35"
        };

        var appointmentService = new AppointmentService(dataMock, mapperMock);
        await appointmentService.AddAsync(doctor.Id, model, DateTime.ParseExact(model.Start, "dd.MM.yyyy HH:mm", null),
            DateTime.ParseExact(model.End, "dd.MM.yyyy HH:mm", null));

        var appointment = dataMock.Appointments.First();

        var rating = new CreateRatingFormModel()
        {
            AppointmentId = appointment.Id,
            PatientId = "pat",
            Comment = "comment",
            DoctorId = "doc",
            Number = 5
        };

        var ratingService = new RatingService(dataMock, appointmentService, mapperMock);

        await ratingService.AddAsync(rating);
        var ratingData = await dataMock.Ratings.FirstAsync();

        var result = await ratingService.ApproveRating(ratingData.Id);
        
        Assert.True(result.Succeeded);
    }
    
    [Fact]
    public async Task ApproveRatingShouldReturnFalseIfRatingDoesNotExist()
    {
        var dataMock = DatabaseMock.Instance;
        var mapperMock = MapperMock.Instance;

        var doctor = new Doctor()
        {
            Id = "doc",
            FirstName = "Evlogi",
            LastName = "Manev",
            PhoneNumber = "098787862"
        };

        var patient = new Patient()
        {
            Id = "pat",
            FirstName = "Gosho",
            LastName = "Peshev",
            Phone = "0988878264",
        };

        var causes = new List<AppointmentCause>()
        {
            new AppointmentCause { Id = 1, Name = "Start" },
            new AppointmentCause { Id = 2, Name = "End" },
            new AppointmentCause { Id = 3, Name = "Middle" },
        };

        await dataMock.AppointmentCauses.AddRangeAsync(causes);
        await dataMock.Doctors.AddAsync(doctor);
        await dataMock.Patients.AddAsync(patient);
        await dataMock.SaveChangesAsync();

        var model = new PatientAppointmentCreateModel()
        {
            PatientId = "pat",
            AppointmentCauseId = 1,
            ChildFirstName = "Evlogi",
            DoctorId = "doc",
            Start = "10.10.2022 15:30",
            End = "15.10.2022 15:35"
        };

        var appointmentService = new AppointmentService(dataMock, mapperMock);
        await appointmentService.AddAsync(doctor.Id, model, DateTime.ParseExact(model.Start, "dd.MM.yyyy HH:mm", null),
            DateTime.ParseExact(model.End, "dd.MM.yyyy HH:mm", null));

        var appointment = dataMock.Appointments.First();

        var rating = new CreateRatingFormModel()
        {
            AppointmentId = appointment.Id,
            PatientId = "pat",
            Comment = "comment",
            DoctorId = "doc",
            Number = 5
        };

        var ratingService = new RatingService(dataMock, appointmentService, mapperMock);

        await ratingService.AddAsync(rating);

        var result = await ratingService.ApproveRating(15);
        
        Assert.True(result.Failed);
    }

    [Fact]
    public async Task DeleteRatingShouldWorkCorrectly()
    {
        var dataMock = DatabaseMock.Instance;
        var mapperMock = MapperMock.Instance;

        var doctor = new Doctor()
        {
            Id = "doc",
            FirstName = "Evlogi",
            LastName = "Manev",
            PhoneNumber = "098787862"
        };

        var patient = new Patient()
        {
            Id = "pat",
            FirstName = "Gosho",
            LastName = "Peshev",
            Phone = "0988878264",
        };

        var causes = new List<AppointmentCause>()
        {
            new AppointmentCause { Id = 1, Name = "Start" },
            new AppointmentCause { Id = 2, Name = "End" },
            new AppointmentCause { Id = 3, Name = "Middle" },
        };

        await dataMock.AppointmentCauses.AddRangeAsync(causes);
        await dataMock.Doctors.AddAsync(doctor);
        await dataMock.Patients.AddAsync(patient);
        await dataMock.SaveChangesAsync();

        var model = new PatientAppointmentCreateModel()
        {
            PatientId = "pat",
            AppointmentCauseId = 1,
            ChildFirstName = "Evlogi",
            DoctorId = "doc",
            Start = "10.10.2022 15:30",
            End = "15.10.2022 15:35"
        };

        var appointmentService = new AppointmentService(dataMock, mapperMock);
        await appointmentService.AddAsync(doctor.Id, model, DateTime.ParseExact(model.Start, "dd.MM.yyyy HH:mm", null),
            DateTime.ParseExact(model.End, "dd.MM.yyyy HH:mm", null));

        var appointment = dataMock.Appointments.First();

        var rating = new CreateRatingFormModel()
        {
            AppointmentId = appointment.Id,
            PatientId = "pat",
            Comment = "comment",
            DoctorId = "doc",
            Number = 5
        };

        var ratingService = new RatingService(dataMock, appointmentService, mapperMock);

        await ratingService.AddAsync(rating);
        var ratingData = await dataMock.Ratings.FirstAsync();

        var result = await ratingService.DeleteRating(ratingData.Id);
        
        Assert.True(result.Succeeded);
    }

    [Fact]
    public async Task DeleteRatingShouldReturnFalseIfRatingDoesNotExist()
    {
        var dataMock = DatabaseMock.Instance;
        var mapperMock = MapperMock.Instance;

        var doctor = new Doctor()
        {
            Id = "doc",
            FirstName = "Evlogi",
            LastName = "Manev",
            PhoneNumber = "098787862"
        };

        var patient = new Patient()
        {
            Id = "pat",
            FirstName = "Gosho",
            LastName = "Peshev",
            Phone = "0988878264",
        };

        var causes = new List<AppointmentCause>()
        {
            new AppointmentCause { Id = 1, Name = "Start" },
            new AppointmentCause { Id = 2, Name = "End" },
            new AppointmentCause { Id = 3, Name = "Middle" },
        };

        await dataMock.AppointmentCauses.AddRangeAsync(causes);
        await dataMock.Doctors.AddAsync(doctor);
        await dataMock.Patients.AddAsync(patient);
        await dataMock.SaveChangesAsync();

        var model = new PatientAppointmentCreateModel()
        {
            PatientId = "pat",
            AppointmentCauseId = 1,
            ChildFirstName = "Evlogi",
            DoctorId = "doc",
            Start = "10.10.2022 15:30",
            End = "15.10.2022 15:35"
        };

        var appointmentService = new AppointmentService(dataMock, mapperMock);
        await appointmentService.AddAsync(doctor.Id, model, DateTime.ParseExact(model.Start, "dd.MM.yyyy HH:mm", null),
            DateTime.ParseExact(model.End, "dd.MM.yyyy HH:mm", null));

        var appointment = dataMock.Appointments.First();

        var rating = new CreateRatingFormModel()
        {
            AppointmentId = appointment.Id,
            PatientId = "pat",
            Comment = "comment",
            DoctorId = "doc",
            Number = 5
        };

        var ratingService = new RatingService(dataMock, appointmentService, mapperMock);

        await ratingService.AddAsync(rating);

        var result = await ratingService.DeleteRating(15);
        
        Assert.True(result.Failed);
    }

    [Fact]
    public async Task GetRatingsCountShouldWorkCorrectly()
    {
        var dataMock = DatabaseMock.Instance;
        var mapperMock = MapperMock.Instance;

        var doctor = new Doctor()
        {
            Id = "doc",
            FirstName = "Evlogi",
            LastName = "Manev",
            PhoneNumber = "098787862"
        };

        var patient = new Patient()
        {
            Id = "pat",
            FirstName = "Gosho",
            LastName = "Peshev",
            Phone = "0988878264",
        };

        var causes = new List<AppointmentCause>()
        {
            new AppointmentCause { Id = 1, Name = "Start" },
            new AppointmentCause { Id = 2, Name = "End" },
            new AppointmentCause { Id = 3, Name = "Middle" },
        };

        await dataMock.AppointmentCauses.AddRangeAsync(causes);
        await dataMock.Doctors.AddAsync(doctor);
        await dataMock.Patients.AddAsync(patient);
        await dataMock.SaveChangesAsync();

        var model = new PatientAppointmentCreateModel()
        {
            PatientId = "pat",
            AppointmentCauseId = 1,
            ChildFirstName = "Evlogi",
            DoctorId = "doc",
            Start = "10.10.2022 15:30",
            End = "15.10.2022 15:35"
        };

        var appointmentService = new AppointmentService(dataMock, mapperMock);
        await appointmentService.AddAsync(doctor.Id, model, DateTime.ParseExact(model.Start, "dd.MM.yyyy HH:mm", null),
            DateTime.ParseExact(model.End, "dd.MM.yyyy HH:mm", null));

        var appointment = dataMock.Appointments.First();

        var rating = new CreateRatingFormModel()
        {
            AppointmentId = appointment.Id,
            PatientId = "pat",
            Comment = "comment",
            DoctorId = "doc",
            Number = 5
        };

        var ratingService = new RatingService(dataMock, appointmentService, mapperMock);

        await ratingService.AddAsync(rating);

        var result = await ratingService.GetRatingsCount();
        
        Assert.Equal(1, result);
    }
    
    [Fact]
    public async Task GetRatingsShouldWorkCorrectly()
    {
        var dataMock = DatabaseMock.Instance;
        var mapperMock = MapperMock.Instance;

        var doctor = new Doctor()
        {
            Id = "doc",
            FirstName = "Evlogi",
            LastName = "Manev",
            PhoneNumber = "098787862"
        };

        var patient = new Patient()
        {
            Id = "pat",
            FirstName = "Gosho",
            LastName = "Peshev",
            Phone = "0988878264",
        };

        var causes = new List<AppointmentCause>()
        {
            new AppointmentCause { Id = 1, Name = "Start" },
            new AppointmentCause { Id = 2, Name = "End" },
            new AppointmentCause { Id = 3, Name = "Middle" },
        };

        await dataMock.AppointmentCauses.AddRangeAsync(causes);
        await dataMock.Doctors.AddAsync(doctor);
        await dataMock.Patients.AddAsync(patient);
        await dataMock.SaveChangesAsync();

        var model = new PatientAppointmentCreateModel()
        {
            PatientId = "pat",
            AppointmentCauseId = 1,
            ChildFirstName = "Evlogi",
            DoctorId = "doc",
            Start = "10.10.2022 15:30",
            End = "15.10.2022 15:35"
        };

        var appointmentService = new AppointmentService(dataMock, mapperMock);
        await appointmentService.AddAsync(doctor.Id, model, DateTime.ParseExact(model.Start, "dd.MM.yyyy HH:mm", null),
            DateTime.ParseExact(model.End, "dd.MM.yyyy HH:mm", null));

        var appointment = dataMock.Appointments.First();

        var rating = new CreateRatingFormModel()
        {
            AppointmentId = appointment.Id,
            PatientId = "pat",
            Comment = "comment",
            DoctorId = "doc",
            Number = 5
        };

        var ratingService = new RatingService(dataMock, appointmentService, mapperMock);

        await ratingService.AddAsync(rating);

        var result = await ratingService.GetRatings();
        
        Assert.NotNull(result);
        Assert.Equal(1, result.Count);
        Assert.IsAssignableFrom<List<RatingViewModel>>(result);
    }
}