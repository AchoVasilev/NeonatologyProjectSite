namespace Test.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Data.Models;

    using global::Services.AppointmentService;

    using Test.Mocks;

    using ViewModels.Appointments;

    using Xunit;

    public class AppointmentServiceTests
    {
        [Fact]
        public async Task GetAllAppointmentsShouldReturnCorrectCount()
        {
            var dataMock = DatabaseMock.Instance;
            var mapperMock = MapperMock.Instance;

            var causes = new List<AppointmentCause>()
            {
                    new AppointmentCause { Id = 1, Name = "Start" },
                    new AppointmentCause { Id = 2, Name = "End" },
                    new AppointmentCause { Id = 3, Name = "Middle" },
            };

            await dataMock.AppointmentCauses.AddRangeAsync(causes);
            await dataMock.SaveChangesAsync();

            var appointments = new List<Appointment>()
            {
                new Appointment
                {
                    AppointmentCauseId = 1
                },

                new Appointment
                {
                    AppointmentCauseId = 2
                },

                new Appointment
                {
                    AppointmentCauseId = 3
                },
            };

            await dataMock.Appointments.AddRangeAsync(appointments);
            await dataMock.SaveChangesAsync();

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

            var causes = new List<AppointmentCause>()
            {
                    new AppointmentCause { Id = 1, Name = "Start" },
                    new AppointmentCause { Id = 2, Name = "End" },
                    new AppointmentCause { Id = 3, Name = "Middle" },
            };

            await dataMock.AppointmentCauses.AddRangeAsync(causes);
            await dataMock.SaveChangesAsync();

            var appointments = new List<Appointment>()
            {
                new Appointment
                {
                    AppointmentCauseId = 1
                },

                new Appointment
                {
                    AppointmentCauseId = 2,
                    IsDeleted = true
                },

                new Appointment
                {
                    AppointmentCauseId = 3,
                    IsDeleted = true
                },
            };

            await dataMock.Appointments.AddRangeAsync(appointments);
            await dataMock.SaveChangesAsync();

            var service = new AppointmentService(dataMock, mapperMock);
            var result = await service.GetAllAppointments();

            Assert.NotNull(result);
            Assert.Equal(1, result.Count);
        }

        [Fact]
        public async Task GetAllAppointmentsShouldReturnCorrectModel()
        {
            var dataMock = DatabaseMock.Instance;
            var mapperMock = MapperMock.Instance;

            var causes = new List<AppointmentCause>()
            {
                    new AppointmentCause { Id = 1, Name = "Start" },
                    new AppointmentCause { Id = 2, Name = "End" },
                    new AppointmentCause { Id = 3, Name = "Middle" },
            };

            await dataMock.AppointmentCauses.AddRangeAsync(causes);
            await dataMock.SaveChangesAsync();

            var appointments = new List<Appointment>()
            {
                new Appointment
                {
                    AppointmentCauseId = 1
                },

                new Appointment
                {
                    AppointmentCauseId = 2
                },

                new Appointment
                {
                    AppointmentCauseId = 3
                },
            };

            await dataMock.Appointments.AddRangeAsync(appointments);
            await dataMock.SaveChangesAsync();

            var service = new AppointmentService(dataMock, mapperMock);
            var result = await service.GetAllAppointments();

            Assert.NotNull(result);
            Assert.IsAssignableFrom<ICollection<AppointmentViewModel>>(result);
        }

        [Fact]
        public async Task GetUpcommingUserAppointmentsShouldReturnCorrectCount()
        {
            var dataMock = DatabaseMock.Instance;
            var mapperMock = MapperMock.Instance;

            var patient = new Patient() 
            { 
                Id = "pat",
                FirstName = "Evlogi",
                LastName = "Manev",
                Phone = "098787862"
            };

            var causes = new List<AppointmentCause>()
            {
                    new AppointmentCause { Id = 1, Name = "Start" },
                    new AppointmentCause { Id = 2, Name = "End" },
                    new AppointmentCause { Id = 3, Name = "Middle" },
            };

            await dataMock.Patients.AddAsync(patient);
            await dataMock.AppointmentCauses.AddRangeAsync(causes);
            await dataMock.SaveChangesAsync();

            var appointments = new List<Appointment>()
            {
                new Appointment
                {
                    AppointmentCauseId = 1,
                    PatientId = "pat",
                    DateTime = DateTime.UtcNow.AddDays(10)
                },

                new Appointment
                {
                    AppointmentCauseId = 2,
                    PatientId = "pat",
                    DateTime = DateTime.UtcNow.AddDays(10)
                },

                new Appointment
                {
                    AppointmentCauseId = 3,
                    PatientId = "pat",
                    DateTime = DateTime.UtcNow.AddDays(10)
                },
            };

            await dataMock.Appointments.AddRangeAsync(appointments);
            await dataMock.SaveChangesAsync();

            var service = new AppointmentService(dataMock, mapperMock);
            var result = await service.GetUpcomingUserAppointments("pat");

            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
        }

        [Fact]
        public async Task GetUpcommingUserAppointmentsShouldReturnCorrectCountIfOneIsDeleted()
        {
            var dataMock = DatabaseMock.Instance;
            var mapperMock = MapperMock.Instance;

            var patient = new Patient()
            {
                Id = "pat",
                FirstName = "Evlogi",
                LastName = "Manev",
                Phone = "098787862"
            };

            var causes = new List<AppointmentCause>()
            {
                    new AppointmentCause { Id = 1, Name = "Start" },
                    new AppointmentCause { Id = 2, Name = "End" },
                    new AppointmentCause { Id = 3, Name = "Middle" },
            };

            await dataMock.Patients.AddAsync(patient);
            await dataMock.AppointmentCauses.AddRangeAsync(causes);
            await dataMock.SaveChangesAsync();

            var appointments = new List<Appointment>()
            {
                new Appointment
                {
                    AppointmentCauseId = 1,
                    PatientId = "pat",
                    DateTime = DateTime.UtcNow.AddDays(10),
                    IsDeleted = true
                },

                new Appointment
                {
                    AppointmentCauseId = 2,
                    PatientId = "pat",
                    DateTime = DateTime.UtcNow.AddDays(10)
                },

                new Appointment
                {
                    AppointmentCauseId = 3,
                    PatientId = "pat",
                    DateTime = DateTime.UtcNow.AddDays(10)
                },
            };

            await dataMock.Appointments.AddRangeAsync(appointments);
            await dataMock.SaveChangesAsync();

            var service = new AppointmentService(dataMock, mapperMock);
            var result = await service.GetUpcomingUserAppointments("pat");

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task GetUpcommingUserAppointmentsShouldReturnCorrectModel()
        {
            var dataMock = DatabaseMock.Instance;
            var mapperMock = MapperMock.Instance;

            var patient = new Patient()
            {
                Id = "pat",
                FirstName = "Evlogi",
                LastName = "Manev",
                Phone = "098787862"
            };

            var causes = new List<AppointmentCause>()
            {
                    new AppointmentCause { Id = 1, Name = "Start" },
                    new AppointmentCause { Id = 2, Name = "End" },
                    new AppointmentCause { Id = 3, Name = "Middle" },
            };

            await dataMock.Patients.AddAsync(patient);
            await dataMock.AppointmentCauses.AddRangeAsync(causes);
            await dataMock.SaveChangesAsync();

            var appointments = new List<Appointment>()
            {
                new Appointment
                {
                    AppointmentCauseId = 1,
                    PatientId = "pat",
                    DateTime = DateTime.UtcNow.AddDays(10)
                },

                new Appointment
                {
                    AppointmentCauseId = 2,
                    PatientId = "pat",
                    DateTime = DateTime.UtcNow.AddDays(10)
                },

                new Appointment
                {
                    AppointmentCauseId = 3,
                    PatientId = "pat",
                    DateTime = DateTime.UtcNow.AddDays(10)
                },
            };

            await dataMock.Appointments.AddRangeAsync(appointments);
            await dataMock.SaveChangesAsync();

            var service = new AppointmentService(dataMock, mapperMock);
            var result = await service.GetUpcomingUserAppointments("pat");

            Assert.NotNull(result);
            Assert.IsAssignableFrom<ICollection<AppointmentViewModel>>(result);
        }

        [Fact]
        public async Task GetPastUserAppointmentsShouldReturnCorrectCount()
        {
            var dataMock = DatabaseMock.Instance;
            var mapperMock = MapperMock.Instance;

            var patient = new Patient()
            {
                Id = "pat",
                FirstName = "Evlogi",
                LastName = "Manev",
                Phone = "098787862"
            };

            var causes = new List<AppointmentCause>()
            {
                    new AppointmentCause { Id = 1, Name = "Start" },
                    new AppointmentCause { Id = 2, Name = "End" },
                    new AppointmentCause { Id = 3, Name = "Middle" },
            };

            await dataMock.Patients.AddAsync(patient);
            await dataMock.AppointmentCauses.AddRangeAsync(causes);
            await dataMock.SaveChangesAsync();

            var appointments = new List<Appointment>()
            {
                new Appointment
                {
                    AppointmentCauseId = 1,
                    PatientId = "pat",
                    DateTime = DateTime.UtcNow.AddDays(-10)
                },

                new Appointment
                {
                    AppointmentCauseId = 2,
                    PatientId = "pat",
                    DateTime = DateTime.UtcNow.AddDays(-10)
                },

                new Appointment
                {
                    AppointmentCauseId = 3,
                    PatientId = "pat",
                    DateTime = DateTime.UtcNow.AddDays(-10)
                },
            };

            await dataMock.Appointments.AddRangeAsync(appointments);
            await dataMock.SaveChangesAsync();

            var service = new AppointmentService(dataMock, mapperMock);
            var result = await service.GetPastUserAppointments("pat");

            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
        }

        [Fact]
        public async Task GetPastUserAppointmentsShouldReturnCorrectCountIfOneIsDeleted()
        {
            var dataMock = DatabaseMock.Instance;
            var mapperMock = MapperMock.Instance;

            var patient = new Patient()
            {
                Id = "pat",
                FirstName = "Evlogi",
                LastName = "Manev",
                Phone = "098787862"
            };

            var causes = new List<AppointmentCause>()
            {
                    new AppointmentCause { Id = 1, Name = "Start" },
                    new AppointmentCause { Id = 2, Name = "End" },
                    new AppointmentCause { Id = 3, Name = "Middle" },
            };

            await dataMock.Patients.AddAsync(patient);
            await dataMock.AppointmentCauses.AddRangeAsync(causes);
            await dataMock.SaveChangesAsync();

            var appointments = new List<Appointment>()
            {
                new Appointment
                {
                    AppointmentCauseId = 1,
                    PatientId = "pat",
                    DateTime = DateTime.UtcNow.AddDays(-10),
                    IsDeleted = true
                },

                new Appointment
                {
                    AppointmentCauseId = 2,
                    PatientId = "pat",
                    DateTime = DateTime.UtcNow.AddDays(-10)
                },

                new Appointment
                {
                    AppointmentCauseId = 3,
                    PatientId = "pat",
                    DateTime = DateTime.UtcNow.AddDays(-10)
                },
            };

            await dataMock.Appointments.AddRangeAsync(appointments);
            await dataMock.SaveChangesAsync();

            var service = new AppointmentService(dataMock, mapperMock);
            var result = await service.GetPastUserAppointments("pat");

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task GetPastUserAppointmentsShouldReturnCorrectModel()
        {
            var dataMock = DatabaseMock.Instance;
            var mapperMock = MapperMock.Instance;

            var patient = new Patient()
            {
                Id = "pat",
                FirstName = "Evlogi",
                LastName = "Manev",
                Phone = "098787862"
            };

            var causes = new List<AppointmentCause>()
            {
                    new AppointmentCause { Id = 1, Name = "Start" },
                    new AppointmentCause { Id = 2, Name = "End" },
                    new AppointmentCause { Id = 3, Name = "Middle" },
            };

            await dataMock.Patients.AddAsync(patient);
            await dataMock.AppointmentCauses.AddRangeAsync(causes);
            await dataMock.SaveChangesAsync();

            var appointments = new List<Appointment>()
            {
                new Appointment
                {
                    AppointmentCauseId = 1,
                    PatientId = "pat",
                    DateTime = DateTime.UtcNow.AddDays(-10)
                },

                new Appointment
                {
                    AppointmentCauseId = 2,
                    PatientId = "pat",
                    DateTime = DateTime.UtcNow.AddDays(-10)
                },

                new Appointment
                {
                    AppointmentCauseId = 3,
                    PatientId = "pat",
                    DateTime = DateTime.UtcNow.AddDays(-10)
                },
            };

            await dataMock.Appointments.AddRangeAsync(appointments);
            await dataMock.SaveChangesAsync();

            var service = new AppointmentService(dataMock, mapperMock);
            var result = await service.GetPastUserAppointments("pat");

            Assert.NotNull(result);
            Assert.IsAssignableFrom<ICollection<AppointmentViewModel>>(result);
        }
    }
}
