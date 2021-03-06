namespace Test.ServiceUnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Data.Models;

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

            var causes = new List<AppointmentCause>()
            {
                    new AppointmentCause { Id = 1, Name = "Start" },
                    new AppointmentCause { Id = 2, Name = "End" },
                    new AppointmentCause { Id = 3, Name = "Middle" },
            };

            var address = new Address
            {
                Id = 1,
                StreetName = "Kaspichan 49",
                City = new City
                {
                    Name = "Kaspichan",
                    ZipCode = 1234
                }
            };

            await dataMock.Addresses.AddAsync(address);

            await dataMock.AppointmentCauses.AddRangeAsync(causes);
            await dataMock.SaveChangesAsync();

            var appointments = new List<Appointment>()
            {
                new Appointment
                {
                    AppointmentCauseId = 1,
                    AddressId = 1
                },

                new Appointment
                {
                    AppointmentCauseId = 2,
                    AddressId = 1
                },

                new Appointment
                {
                    AppointmentCauseId = 3,
                    AddressId = 1
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

            var address = new Address
            {
                Id = 1,
                StreetName = "Kaspichan 49",
                City = new City
                {
                    Name = "Kaspichan",
                    ZipCode = 1234
                }
            };

            await dataMock.Addresses.AddAsync(address);
            await dataMock.AppointmentCauses.AddRangeAsync(causes);
            await dataMock.SaveChangesAsync();

            var appointments = new List<Appointment>()
            {
                new Appointment
                {
                    AppointmentCauseId = 1,
                    AddressId = 1
                },

                new Appointment
                {
                    AppointmentCauseId = 2,
                    IsDeleted = true,
                    AddressId = 1
                },

                new Appointment
                {
                    AppointmentCauseId = 3,
                    IsDeleted = true,
                    AddressId = 1
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
            var result = await service.GetUpcomingUserAppointments("pat", 8, 1);

            Assert.NotNull(result);
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
            var result = await service.GetUpcomingUserAppointments("pat", 8, 1);

            Assert.NotNull(result);
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
            var result = await service.GetUpcomingUserAppointments("pat", 8, 1);

            Assert.NotNull(result);
            Assert.IsAssignableFrom<AllAppointmentsViewModel>(result);
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
            var result = await service.GetPastUserAppointments("pat", 8, 1);

            Assert.NotNull(result);
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
            var result = await service.GetPastUserAppointments("pat", 8, 1);

            Assert.NotNull(result);
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
            var result = await service.GetPastUserAppointments("pat", 8, 1);

            Assert.NotNull(result);
            Assert.IsAssignableFrom<AllAppointmentsViewModel>(result);
        }

        [Fact]
        public async Task GetUpcommingDoctorAppointmentsShouldReturnCorrectCount()
        {
            var dataMock = DatabaseMock.Instance;
            var mapperMock = MapperMock.Instance;

            var patient = new Doctor()
            {
                Id = "pat",
                FirstName = "Evlogi",
                LastName = "Manev",
                PhoneNumber = "098787862"
            };

            var causes = new List<AppointmentCause>()
            {
                    new AppointmentCause { Id = 1, Name = "Start" },
                    new AppointmentCause { Id = 2, Name = "End" },
                    new AppointmentCause { Id = 3, Name = "Middle" },
            };

            await dataMock.Doctors.AddAsync(patient);
            await dataMock.AppointmentCauses.AddRangeAsync(causes);
            await dataMock.SaveChangesAsync();

            var appointments = new List<Appointment>()
            {
                new Appointment
                {
                    AppointmentCauseId = 1,
                    DoctorId = "pat",
                    DateTime = DateTime.UtcNow.AddDays(10)
                },

                new Appointment
                {
                    AppointmentCauseId = 2,
                    DoctorId = "pat",
                    DateTime = DateTime.UtcNow.AddDays(10)
                },

                new Appointment
                {
                    AppointmentCauseId = 3,
                    DoctorId = "pat",
                    DateTime = DateTime.UtcNow.AddDays(10)
                },
            };

            await dataMock.Appointments.AddRangeAsync(appointments);
            await dataMock.SaveChangesAsync();

            var service = new AppointmentService(dataMock, mapperMock);
            var result = await service.GetUpcomingDoctorAppointments("pat", 8, 1);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetUpcommingDoctorAppointmentsShouldReturnCorrectCountIfOneIsDeleted()
        {
            var dataMock = DatabaseMock.Instance;
            var mapperMock = MapperMock.Instance;

            var patient = new Doctor()
            {
                Id = "pat",
                FirstName = "Evlogi",
                LastName = "Manev",
                PhoneNumber = "098787862"
            };

            var causes = new List<AppointmentCause>()
            {
                    new AppointmentCause { Id = 1, Name = "Start" },
                    new AppointmentCause { Id = 2, Name = "End" },
                    new AppointmentCause { Id = 3, Name = "Middle" },
            };

            await dataMock.Doctors.AddAsync(patient);
            await dataMock.AppointmentCauses.AddRangeAsync(causes);
            await dataMock.SaveChangesAsync();

            var appointments = new List<Appointment>()
            {
                new Appointment
                {
                    AppointmentCauseId = 1,
                    DoctorId = "pat",
                    DateTime = DateTime.UtcNow.AddDays(10),
                    IsDeleted = true
                },

                new Appointment
                {
                    AppointmentCauseId = 2,
                    DoctorId = "pat",
                    DateTime = DateTime.UtcNow.AddDays(10)
                },

                new Appointment
                {
                    AppointmentCauseId = 3,
                    DoctorId = "pat",
                    DateTime = DateTime.UtcNow.AddDays(10)
                },
            };

            await dataMock.Appointments.AddRangeAsync(appointments);
            await dataMock.SaveChangesAsync();

            var service = new AppointmentService(dataMock, mapperMock);
            var result = await service.GetUpcomingDoctorAppointments("pat", 8, 1);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetUpcommingDoctorAppointmentsShouldReturnCorrectModel()
        {
            var dataMock = DatabaseMock.Instance;
            var mapperMock = MapperMock.Instance;

            var patient = new Doctor()
            {
                Id = "pat",
                FirstName = "Evlogi",
                LastName = "Manev",
                PhoneNumber = "098787862"
            };

            var causes = new List<AppointmentCause>()
            {
                    new AppointmentCause { Id = 1, Name = "Start" },
                    new AppointmentCause { Id = 2, Name = "End" },
                    new AppointmentCause { Id = 3, Name = "Middle" },
            };

            await dataMock.Doctors.AddAsync(patient);
            await dataMock.AppointmentCauses.AddRangeAsync(causes);
            await dataMock.SaveChangesAsync();

            var appointments = new List<Appointment>()
            {
                new Appointment
                {
                    AppointmentCauseId = 1,
                    DoctorId = "pat",
                    DateTime = DateTime.UtcNow.AddDays(10)
                },

                new Appointment
                {
                    AppointmentCauseId = 2,
                    DoctorId = "pat",
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
            var result = await service.GetUpcomingDoctorAppointments("pat", 8, 1);

            Assert.NotNull(result);
            Assert.IsAssignableFrom<AllAppointmentsViewModel>(result);
        }

        [Fact]
        public async Task GetPastDoctorAppointmentsShouldReturnCorrectCount()
        {
            var dataMock = DatabaseMock.Instance;
            var mapperMock = MapperMock.Instance;

            var patient = new Doctor()
            {
                Id = "pat",
                FirstName = "Evlogi",
                LastName = "Manev",
                PhoneNumber = "098787862"
            };

            var causes = new List<AppointmentCause>()
            {
                    new AppointmentCause { Id = 1, Name = "Start" },
                    new AppointmentCause { Id = 2, Name = "End" },
                    new AppointmentCause { Id = 3, Name = "Middle" },
            };

            await dataMock.Doctors.AddAsync(patient);
            await dataMock.AppointmentCauses.AddRangeAsync(causes);
            await dataMock.SaveChangesAsync();

            var appointments = new List<Appointment>()
            {
                new Appointment
                {
                    AppointmentCauseId = 1,
                    DoctorId = "pat",
                    DateTime = DateTime.UtcNow.AddDays(-10)
                },

                new Appointment
                {
                    AppointmentCauseId = 2,
                    DoctorId = "pat",
                    DateTime = DateTime.UtcNow.AddDays(-10)
                },

                new Appointment
                {
                    AppointmentCauseId = 3,
                    DoctorId = "pat",
                    DateTime = DateTime.UtcNow.AddDays(-10)
                },
            };

            await dataMock.Appointments.AddRangeAsync(appointments);
            await dataMock.SaveChangesAsync();

            var service = new AppointmentService(dataMock, mapperMock);
            var result = await service.GetPastDoctorAppointments("pat", 8, 1);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetPastDoctorAppointmentsShouldReturnCorrectCountIfOneIsDeleted()
        {
            var dataMock = DatabaseMock.Instance;
            var mapperMock = MapperMock.Instance;

            var patient = new Doctor()
            {
                Id = "pat",
                FirstName = "Evlogi",
                LastName = "Manev",
                PhoneNumber = "098787862"
            };

            var causes = new List<AppointmentCause>()
            {
                    new AppointmentCause { Id = 1, Name = "Start" },
                    new AppointmentCause { Id = 2, Name = "End" },
                    new AppointmentCause { Id = 3, Name = "Middle" },
            };

            await dataMock.Doctors.AddAsync(patient);
            await dataMock.AppointmentCauses.AddRangeAsync(causes);
            await dataMock.SaveChangesAsync();

            var appointments = new List<Appointment>()
            {
                new Appointment
                {
                    AppointmentCauseId = 1,
                    DoctorId = "pat",
                    DateTime = DateTime.UtcNow.AddDays(-10),
                    IsDeleted = true
                },

                new Appointment
                {
                    AppointmentCauseId = 2,
                    DoctorId = "pat",
                    DateTime = DateTime.UtcNow.AddDays(-10)
                },

                new Appointment
                {
                    AppointmentCauseId = 3,
                    DoctorId = "pat",
                    DateTime = DateTime.UtcNow.AddDays(-10)
                },
            };

            await dataMock.Appointments.AddRangeAsync(appointments);
            await dataMock.SaveChangesAsync();

            var service = new AppointmentService(dataMock, mapperMock);
            var result = await service.GetPastDoctorAppointments("pat", 8, 1);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetPastDoctorAppointmentsShouldReturnCorrectModel()
        {
            var dataMock = DatabaseMock.Instance;
            var mapperMock = MapperMock.Instance;

            var patient = new Doctor()
            {
                Id = "pat",
                FirstName = "Evlogi",
                LastName = "Manev",
                PhoneNumber = "098787862"
            };

            var causes = new List<AppointmentCause>()
            {
                    new AppointmentCause { Id = 1, Name = "Start" },
                    new AppointmentCause { Id = 2, Name = "End" },
                    new AppointmentCause { Id = 3, Name = "Middle" },
            };

            await dataMock.Doctors.AddAsync(patient);
            await dataMock.AppointmentCauses.AddRangeAsync(causes);
            await dataMock.SaveChangesAsync();

            var appointments = new List<Appointment>()
            {
                new Appointment
                {
                    AppointmentCauseId = 1,
                    DoctorId = "pat",
                    DateTime = DateTime.UtcNow.AddDays(-10)
                },

                new Appointment
                {
                    AppointmentCauseId = 2,
                    DoctorId = "pat",
                    DateTime = DateTime.UtcNow.AddDays(-10)
                },

                new Appointment
                {
                    AppointmentCauseId = 3,
                    DoctorId = "pat",
                    DateTime = DateTime.UtcNow.AddDays(-10)
                },
            };

            await dataMock.Appointments.AddRangeAsync(appointments);
            await dataMock.SaveChangesAsync();

            var service = new AppointmentService(dataMock, mapperMock);
            var result = await service.GetPastDoctorAppointments("pat", 8, 1);

            Assert.NotNull(result);
            Assert.IsAssignableFrom<AllAppointmentsViewModel>(result);
        }

        [Fact]
        public async Task GetTakenAppointmentSlotsShouldReturnCorrectCount()
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
                    AppointmentCauseId = 1,
                    DateTime = DateTime.UtcNow
                },

                new Appointment
                {
                    AppointmentCauseId = 2,
                    DateTime = DateTime.UtcNow.AddDays(2)
                },

                new Appointment
                {
                    AppointmentCauseId = 3,
                    DateTime = DateTime.UtcNow.AddDays(3)
                },
            };

            await dataMock.Appointments.AddRangeAsync(appointments);
            await dataMock.SaveChangesAsync();

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
                    AppointmentCauseId = 1,
                    DateTime = DateTime.UtcNow,
                    IsDeleted = true
                },

                new Appointment
                {
                    AppointmentCauseId = 2,
                    DateTime = DateTime.UtcNow.AddDays(2)
                },

                new Appointment
                {
                    AppointmentCauseId = 3,
                    DateTime = DateTime.UtcNow.AddDays(3)
                },
            };

            await dataMock.Appointments.AddRangeAsync(appointments);
            await dataMock.SaveChangesAsync();

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
                    AppointmentCauseId = 1,
                    DateTime = DateTime.UtcNow
                },

                new Appointment
                {
                    AppointmentCauseId = 2,
                    DateTime = DateTime.UtcNow.AddDays(2)
                },

                new Appointment
                {
                    AppointmentCauseId = 3,
                    DateTime = DateTime.UtcNow.AddDays(-3)
                },
            };

            await dataMock.Appointments.AddRangeAsync(appointments);
            await dataMock.SaveChangesAsync();

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
                    AppointmentCauseId = 1,
                    DateTime = DateTime.UtcNow
                },

                new Appointment
                {
                    AppointmentCauseId = 2,
                    DateTime = DateTime.UtcNow.AddDays(2)
                },

                new Appointment
                {
                    AppointmentCauseId = 3,
                    DateTime = DateTime.UtcNow.AddDays(3)
                },
            };

            await dataMock.Appointments.AddRangeAsync(appointments);
            await dataMock.SaveChangesAsync();

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
                    AppointmentCauseId = 1,
                    DateTime = DateTime.UtcNow
                },

                new Appointment
                {
                    AppointmentCauseId = 2,
                    DateTime = DateTime.UtcNow.AddDays(2)
                },

                new Appointment
                {
                    AppointmentCauseId = 3,
                    DateTime = DateTime.UtcNow.AddDays(3)
                },
            };

            await dataMock.Appointments.AddRangeAsync(appointments);
            await dataMock.SaveChangesAsync();

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

            var doctor = new Doctor()
            {
                Id = "pat",
                FirstName = "Evlogi",
                LastName = "Manev",
                PhoneNumber = "098787862"
            };

            var causes = new List<AppointmentCause>()
            {
                    new AppointmentCause { Id = 1, Name = "Start" },
                    new AppointmentCause { Id = 2, Name = "End" },
                    new AppointmentCause { Id = 3, Name = "Middle" },
            };

            await dataMock.AppointmentCauses.AddRangeAsync(causes);
            await dataMock.Doctors.AddAsync(doctor);
            await dataMock.SaveChangesAsync();

            var model = new CreateAppointmentModel()
            {
                AppointmentCauseId = 1,
                ChildFirstName = "Evlogi",
                ParentFirstName = "Mancho",
                ParentLastName = "Vanev",
                PhoneNumber = "098785623",
                DoctorId = "pat",
                Email = "gosho@abv.bg",
                Start = "23.04.2022 15:33",
                End = "28.04.2022 15:43"
            };

            var service = new AppointmentService(dataMock, mapperMock);
            var result = await service.AddAsync(doctor.Id, model, DateTime.UtcNow, DateTime.UtcNow.AddDays(1));

            Assert.True(result);
            Assert.Equal(1, dataMock.Appointments.Count());
        }

        [Fact]
        public async Task AddAsyncShouldReturnFalseIfAppointmentAtTheSameTimeExists()
        {
            var dataMock = DatabaseMock.Instance;
            var mapperMock = MapperMock.Instance;

            var doctor = new Doctor()
            {
                Id = "pat",
                FirstName = "Evlogi",
                LastName = "Manev",
                PhoneNumber = "098787862"
            };

            var causes = new List<AppointmentCause>()
            {
                    new AppointmentCause { Id = 1, Name = "Start" },
                    new AppointmentCause { Id = 2, Name = "End" },
                    new AppointmentCause { Id = 3, Name = "Middle" },
            };

            var appointment = new Appointment()
            {
                AppointmentCauseId = 1,
                ChildFirstName = "Evlogi",
                ParentFirstName = "Mancho",
                ParentLastName = "Vanev",
                PhoneNumber = "098785623",
                DoctorId = "pat",
                DateTime = DateTime.ParseExact("29.01.2022 08:30", "dd.MM.yyyy HH:mm", null),
                End = DateTime.ParseExact("03.02.2022 08:40", "dd.MM.yyyy HH:mm", null)
            };

            await dataMock.AppointmentCauses.AddRangeAsync(causes);
            await dataMock.Doctors.AddAsync(doctor);
            await dataMock.Appointments.AddAsync(appointment);
            await dataMock.SaveChangesAsync();

            var model = new CreateAppointmentModel()
            {
                AppointmentCauseId = 1,
                ChildFirstName = "Evlogi",
                ParentFirstName = "Mancho",
                ParentLastName = "Vanev",
                PhoneNumber = "098785623",
                DoctorId = "pat",
                Email = "gosho@abv.bg",
                Start = "29.01.2022 08:30",
                End = "03.02.2022 08:40"
            };

            var service = new AppointmentService(dataMock, mapperMock);
            var result = await service.AddAsync(doctor.Id, model, DateTime.ParseExact(model.Start, "dd.MM.yyyy HH:mm", null),
            DateTime.ParseExact(model.End, "dd.MM.yyyy HH:mm", null));

            Assert.False(result);
        }

        [Fact]
        public async Task AddAsyncForPatientsShouldReturnTrueIfModelIsValid()
        {
            var dataMock = DatabaseMock.Instance;
            var mapperMock = MapperMock.Instance;

            var doctor = new Doctor()
            {
                Id = "pat",
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
                DoctorId = "pat",
                Start = "10.10.2022 15:30",
                End = "15.10.2022 15:35"
            };

            var service = new AppointmentService(dataMock, mapperMock);
            var result = await service.AddAsync(doctor.Id, model, DateTime.ParseExact(model.Start, "dd.MM.yyyy HH:mm", null),
            DateTime.ParseExact(model.End, "dd.MM.yyyy HH:mm", null));

            Assert.True(result);
            Assert.Equal(1, dataMock.Appointments.Count());
        }

        [Fact]
        public async Task AddAsyncForPatientsShouldReturnFalseIfAppointmentAtTheSameTimeExists()
        {
            var dataMock = DatabaseMock.Instance;
            var mapperMock = MapperMock.Instance;

            var doctor = new Doctor()
            {
                Id = "pat",
                FirstName = "Evlogi",
                LastName = "Manev",
                PhoneNumber = "098787862",
            };

            var causes = new List<AppointmentCause>()
            {
                    new AppointmentCause { Id = 1, Name = "Start" },
                    new AppointmentCause { Id = 2, Name = "End" },
                    new AppointmentCause { Id = 3, Name = "Middle" },
            };

            var appointment = new Appointment()
            {
                AppointmentCauseId = 1,
                ChildFirstName = "Evlogi",
                ParentFirstName = "Mancho",
                ParentLastName = "Vanev",
                PhoneNumber = "098785623",
                DoctorId = "pat",
                DateTime = new DateTime(2022, 01, 29, 08, 30, 00),
                End = new DateTime(2022, 01, 29, 08, 40, 00)
            };

            await dataMock.AppointmentCauses.AddRangeAsync(causes);
            await dataMock.Doctors.AddAsync(doctor);
            await dataMock.Appointments.AddAsync(appointment);
            await dataMock.SaveChangesAsync();

            var model = new PatientAppointmentCreateModel()
            {
                AppointmentCauseId = 1,
                ChildFirstName = "Evlogi",
                DoctorId = "pat",
                Start = "29.01.2022 08:30",
                End = "29.01.2022 08:40"
            };

            var service = new AppointmentService(dataMock, mapperMock);
            var result = await service.AddAsync(doctor.Id, model, DateTime.ParseExact(model.Start, "dd.MM.yyyy HH:mm", null),
             DateTime.ParseExact(model.End, "dd.MM.yyyy HH:mm", null));

            Assert.False(result);
        }

        [Fact]
        public async Task GetUserAppointmentShouldReturnCorrectAppointment()
        {
            var dataMock = DatabaseMock.Instance;
            var mapperMock = MapperMock.Instance;

            var patient = new Patient()
            {
                Id = "pat",
                FirstName = "Evlogi",
                LastName = "Manev",
                Phone = "098787862",
                UserId = "patpat"
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
                    Id = 1,
                    AppointmentCauseId = 1,
                    DoctorId = "pat",
                    DateTime = DateTime.UtcNow.AddDays(10)
                },

                new Appointment
                {
                    Id = 2,
                    AppointmentCauseId = 2,
                    DoctorId = "pat",
                    DateTime = DateTime.UtcNow.AddDays(10)
                },

                new Appointment
                {
                    Id = 3,
                    AppointmentCauseId = 3,
                    PatientId = "pat",
                    DateTime = DateTime.UtcNow.AddDays(10)
                },
            };

            await dataMock.Appointments.AddRangeAsync(appointments);
            await dataMock.SaveChangesAsync();

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

            var patient = new Patient()
            {
                Id = "pat",
                FirstName = "Evlogi",
                LastName = "Manev",
                Phone = "098787862",
                UserId = "patpat"
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
                    Id = 1,
                    AppointmentCauseId = 1,
                    DoctorId = "pat",
                    DateTime = DateTime.UtcNow.AddDays(10)
                },

                new Appointment
                {
                    Id = 2,
                    AppointmentCauseId = 2,
                    DoctorId = "pat",
                    DateTime = DateTime.UtcNow.AddDays(10)
                },

                new Appointment
                {
                    Id = 3,
                    AppointmentCauseId = 3,
                    PatientId = "pat",
                    DateTime = DateTime.UtcNow.AddDays(10)
                },
            };

            await dataMock.Appointments.AddRangeAsync(appointments);
            await dataMock.SaveChangesAsync();

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

            var patient = new Patient()
            {
                Id = "pat",
                FirstName = "Evlogi",
                LastName = "Manev",
                Phone = "098787862",
                UserId = "patpat"
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
                    Id = 1,
                    AppointmentCauseId = 1,
                    DoctorId = "pat",
                    DateTime = DateTime.UtcNow.AddDays(10)
                },

                new Appointment
                {
                    Id = 2,
                    AppointmentCauseId = 2,
                    DoctorId = "pat",
                    DateTime = DateTime.UtcNow.AddDays(10)
                },

                new Appointment
                {
                    Id = 3,
                    AppointmentCauseId = 3,
                    PatientId = "pat",
                    DateTime = DateTime.UtcNow.AddDays(10)
                },
            };

            await dataMock.Appointments.AddRangeAsync(appointments);
            await dataMock.SaveChangesAsync();

            var service = new AppointmentService(dataMock, mapperMock);
            var result = await service.GetUserAppointmentAsync("pat", 3);

            Assert.Null(result);
        }

        [Fact]
        public async Task GetUserAppointmentShouldReturnNullIfAppointmentIdIsWrong()
        {
            var dataMock = DatabaseMock.Instance;
            var mapperMock = MapperMock.Instance;

            var patient = new Patient()
            {
                Id = "pat",
                FirstName = "Evlogi",
                LastName = "Manev",
                Phone = "098787862",
                UserId = "patpat"
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
                    Id = 1,
                    AppointmentCauseId = 1,
                    DoctorId = "pat",
                    DateTime = DateTime.UtcNow.AddDays(10)
                },

                new Appointment
                {
                    Id = 2,
                    AppointmentCauseId = 2,
                    DoctorId = "pat",
                    DateTime = DateTime.UtcNow.AddDays(10)
                },

                new Appointment
                {
                    Id = 3,
                    AppointmentCauseId = 3,
                    PatientId = "pat",
                    DateTime = DateTime.UtcNow.AddDays(10)
                },
            };

            await dataMock.Appointments.AddRangeAsync(appointments);
            await dataMock.SaveChangesAsync();

            var service = new AppointmentService(dataMock, mapperMock);
            var result = await service.GetUserAppointmentAsync("patpat", 5);

            Assert.Null(result);
        }

        [Fact]
        public async Task GetUserAppointmentShouldReturnNullIfAppointmentIsDeleted()
        {
            var dataMock = DatabaseMock.Instance;
            var mapperMock = MapperMock.Instance;

            var patient = new Patient()
            {
                Id = "pat",
                FirstName = "Evlogi",
                LastName = "Manev",
                Phone = "098787862",
                UserId = "patpat"
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
                    Id = 1,
                    AppointmentCauseId = 1,
                    DoctorId = "pat",
                    DateTime = DateTime.UtcNow.AddDays(10)
                },

                new Appointment
                {
                    Id = 2,
                    AppointmentCauseId = 2,
                    DoctorId = "pat",
                    DateTime = DateTime.UtcNow.AddDays(10)
                },

                new Appointment
                {
                    Id = 3,
                    AppointmentCauseId = 3,
                    PatientId = "pat",
                    DateTime = DateTime.UtcNow.AddDays(10),
                    IsDeleted = true
                },
            };

            await dataMock.Appointments.AddRangeAsync(appointments);
            await dataMock.SaveChangesAsync();

            var service = new AppointmentService(dataMock, mapperMock);
            var result = await service.GetUserAppointmentAsync("patpat", 3);

            Assert.Null(result);
        }

        [Fact]
        public async Task GetAppointmentByIdShouldReturnCorrectResult()
        {
            var dataMock = DatabaseMock.Instance;
            var mapperMock = MapperMock.Instance;

            var patient = new Patient()
            {
                Id = "pat",
                FirstName = "Evlogi",
                LastName = "Manev",
                Phone = "098787862",
                UserId = "patpat"
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
                    Id = 1,
                    AppointmentCauseId = 1,
                    DoctorId = "pat",
                    DateTime = DateTime.UtcNow.AddDays(10)
                },

                new Appointment
                {
                    Id = 2,
                    AppointmentCauseId = 2,
                    DoctorId = "pat",
                    DateTime = DateTime.UtcNow.AddDays(10)
                },

                new Appointment
                {
                    Id = 3,
                    AppointmentCauseId = 3,
                    PatientId = "pat",
                    DateTime = DateTime.UtcNow.AddDays(10)
                },
            };

            await dataMock.Appointments.AddRangeAsync(appointments);
            await dataMock.SaveChangesAsync();

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

            var patient = new Patient()
            {
                Id = "pat",
                FirstName = "Evlogi",
                LastName = "Manev",
                Phone = "098787862",
                UserId = "patpat"
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
                    Id = 1,
                    AppointmentCauseId = 1,
                    DoctorId = "pat",
                    DateTime = DateTime.UtcNow.AddDays(10)
                },

                new Appointment
                {
                    Id = 2,
                    AppointmentCauseId = 2,
                    DoctorId = "pat",
                    DateTime = DateTime.UtcNow.AddDays(10)
                },

                new Appointment
                {
                    Id = 3,
                    AppointmentCauseId = 3,
                    PatientId = "pat",
                    DateTime = DateTime.UtcNow.AddDays(10)
                },
            };

            await dataMock.Appointments.AddRangeAsync(appointments);
            await dataMock.SaveChangesAsync();

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

            var patient = new Patient()
            {
                Id = "pat",
                FirstName = "Evlogi",
                LastName = "Manev",
                Phone = "098787862",
                UserId = "patpat"
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
                    Id = 1,
                    AppointmentCauseId = 1,
                    DoctorId = "pat",
                    DateTime = DateTime.UtcNow.AddDays(10)
                },

                new Appointment
                {
                    Id = 2,
                    AppointmentCauseId = 2,
                    DoctorId = "pat",
                    DateTime = DateTime.UtcNow.AddDays(10)
                },

                new Appointment
                {
                    Id = 3,
                    AppointmentCauseId = 3,
                    PatientId = "pat",
                    DateTime = DateTime.UtcNow.AddDays(10)
                },
            };

            await dataMock.Appointments.AddRangeAsync(appointments);
            await dataMock.SaveChangesAsync();

            var service = new AppointmentService(dataMock, mapperMock);
            var result = await service.GetAppointmentByIdAsync(5);

            Assert.Null(result);
        }

        [Fact]
        public async Task GetAppointmentByIdShouldReturnNullIfIsDeleted()
        {
            var dataMock = DatabaseMock.Instance;
            var mapperMock = MapperMock.Instance;

            var patient = new Patient()
            {
                Id = "pat",
                FirstName = "Evlogi",
                LastName = "Manev",
                Phone = "098787862",
                UserId = "patpat"
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
                    Id = 1,
                    AppointmentCauseId = 1,
                    DoctorId = "pat",
                    DateTime = DateTime.UtcNow.AddDays(10)
                },

                new Appointment
                {
                    Id = 2,
                    AppointmentCauseId = 2,
                    DoctorId = "pat",
                    DateTime = DateTime.UtcNow.AddDays(10)
                },

                new Appointment
                {
                    Id = 3,
                    AppointmentCauseId = 3,
                    PatientId = "pat",
                    DateTime = DateTime.UtcNow.AddDays(10),
                    IsDeleted = true
                },
            };

            await dataMock.Appointments.AddRangeAsync(appointments);
            await dataMock.SaveChangesAsync();

            var service = new AppointmentService(dataMock, mapperMock);
            var result = await service.GetAppointmentByIdAsync(3);

            Assert.Null(result);
        }

        [Fact]
        public async Task GetTodaysAppointmentsShouldReturnCorrectCount()
        {
            var dataMock = DatabaseMock.Instance;
            var mapperMock = MapperMock.Instance;

            var doctor = new Doctor()
            {
                Id = "pat",
                FirstName = "Evlogi",
                LastName = "Manev",
                PhoneNumber = "098787862",
                UserId = "patpat"
            };

            var causes = new List<AppointmentCause>()
            {
                    new AppointmentCause { Id = 1, Name = "Start" },
                    new AppointmentCause { Id = 2, Name = "End" },
                    new AppointmentCause { Id = 3, Name = "Middle" },
            };

            await dataMock.Doctors.AddAsync(doctor);
            await dataMock.AppointmentCauses.AddRangeAsync(causes);
            await dataMock.SaveChangesAsync();

            var appointments = new List<Appointment>()
            {
                new Appointment
                {
                    AppointmentCauseId = 1,
                    DoctorId = "pat",
                    DateTime = DateTime.UtcNow
                },

                new Appointment
                {
                    AppointmentCauseId = 2,
                    DoctorId = "pat",
                    DateTime = DateTime.UtcNow
                },

                new Appointment
                {
                    AppointmentCauseId = 3,
                    DoctorId = "pat",
                    DateTime = DateTime.UtcNow
                },
            };

            await dataMock.Appointments.AddRangeAsync(appointments);
            await dataMock.SaveChangesAsync();

            var service = new AppointmentService(dataMock, mapperMock);
            var result = await service.GetTodaysAppointments("patpat");

            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetTodaysAppointmentsShouldReturnCorrectCountWithDifferentDateTime()
        {
            var dataMock = DatabaseMock.Instance;
            var mapperMock = MapperMock.Instance;

            var doctor = new Doctor()
            {
                Id = "pat",
                FirstName = "Evlogi",
                LastName = "Manev",
                PhoneNumber = "098787862",
                UserId = "patpat"
            };

            var causes = new List<AppointmentCause>()
            {
                    new AppointmentCause { Id = 1, Name = "Start" },
                    new AppointmentCause { Id = 2, Name = "End" },
                    new AppointmentCause { Id = 3, Name = "Middle" },
            };

            await dataMock.Doctors.AddAsync(doctor);
            await dataMock.AppointmentCauses.AddRangeAsync(causes);
            await dataMock.SaveChangesAsync();

            var appointments = new List<Appointment>()
            {
                new Appointment
                {
                    AppointmentCauseId = 1,
                    DoctorId = "pat",
                    DateTime = DateTime.UtcNow
                },

                new Appointment
                {
                    AppointmentCauseId = 2,
                    DoctorId = "pat",
                    DateTime = DateTime.UtcNow
                },

                new Appointment
                {
                    AppointmentCauseId = 3,
                    DoctorId = "pat",
                    DateTime = DateTime.UtcNow.AddDays(10)
                },
            };

            await dataMock.Appointments.AddRangeAsync(appointments);
            await dataMock.SaveChangesAsync();

            var service = new AppointmentService(dataMock, mapperMock);
            var result = await service.GetTodaysAppointments("patpat");

            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetTodaysAppointmentsShouldReturnCorrectCountIfOneIsDeleted()
        {
            var dataMock = DatabaseMock.Instance;
            var mapperMock = MapperMock.Instance;

            var doctor = new Doctor()
            {
                Id = "pat",
                FirstName = "Evlogi",
                LastName = "Manev",
                PhoneNumber = "098787862",
                UserId = "patpat"
            };

            var causes = new List<AppointmentCause>()
            {
                    new AppointmentCause { Id = 1, Name = "Start" },
                    new AppointmentCause { Id = 2, Name = "End" },
                    new AppointmentCause { Id = 3, Name = "Middle" },
            };

            await dataMock.Doctors.AddAsync(doctor);
            await dataMock.AppointmentCauses.AddRangeAsync(causes);
            await dataMock.SaveChangesAsync();

            var appointments = new List<Appointment>()
            {
                new Appointment
                {
                    AppointmentCauseId = 1,
                    DoctorId = "pat",
                    DateTime = DateTime.UtcNow
                },

                new Appointment
                {
                    AppointmentCauseId = 2,
                    DoctorId = "pat",
                    DateTime = DateTime.UtcNow,
                    IsDeleted = true
                },

                new Appointment
                {
                    AppointmentCauseId = 3,
                    DoctorId = "pat",
                    DateTime = DateTime.UtcNow.AddDays(10)
                },
            };

            await dataMock.Appointments.AddRangeAsync(appointments);
            await dataMock.SaveChangesAsync();

            var service = new AppointmentService(dataMock, mapperMock);
            var result = await service.GetTodaysAppointments("patpat");

            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetTodaysAppointmentsShouldReturnCorrectModel()
        {
            var dataMock = DatabaseMock.Instance;
            var mapperMock = MapperMock.Instance;

            var doctor = new Doctor()
            {
                Id = "pat",
                FirstName = "Evlogi",
                LastName = "Manev",
                PhoneNumber = "098787862",
                UserId = "patpat"
            };

            var causes = new List<AppointmentCause>()
            {
                    new AppointmentCause { Id = 1, Name = "Start" },
                    new AppointmentCause { Id = 2, Name = "End" },
                    new AppointmentCause { Id = 3, Name = "Middle" },
            };

            await dataMock.Doctors.AddAsync(doctor);
            await dataMock.AppointmentCauses.AddRangeAsync(causes);
            await dataMock.SaveChangesAsync();

            var appointments = new List<Appointment>()
            {
                new Appointment
                {
                    AppointmentCauseId = 1,
                    DoctorId = "pat",
                    DateTime = DateTime.UtcNow
                },

                new Appointment
                {
                    AppointmentCauseId = 2,
                    DoctorId = "pat",
                    DateTime = DateTime.UtcNow
                },

                new Appointment
                {
                    AppointmentCauseId = 3,
                    DoctorId = "pat",
                    DateTime = DateTime.UtcNow.AddDays(10)
                },
            };

            await dataMock.Appointments.AddRangeAsync(appointments);
            await dataMock.SaveChangesAsync();

            var service = new AppointmentService(dataMock, mapperMock);
            var result = await service.GetTodaysAppointments("patpat");

            Assert.IsAssignableFrom<ICollection<AppointmentViewModel>>(result);
        }

        [Fact]
        public async Task GetAppointmentsCountShouldReturnCorrectCount()
        {
            var dataMock = DatabaseMock.Instance;
            var mapperMock = MapperMock.Instance;

            var doctor = new Doctor()
            {
                Id = "pat",
                FirstName = "Evlogi",
                LastName = "Manev",
                PhoneNumber = "098787862",
                UserId = "patpat"
            };

            var causes = new List<AppointmentCause>()
            {
                    new AppointmentCause { Id = 1, Name = "Start" },
                    new AppointmentCause { Id = 2, Name = "End" },
                    new AppointmentCause { Id = 3, Name = "Middle" },
            };

            await dataMock.Doctors.AddAsync(doctor);
            await dataMock.AppointmentCauses.AddRangeAsync(causes);
            await dataMock.SaveChangesAsync();

            var appointments = new List<Appointment>()
            {
                new Appointment
                {
                    AppointmentCauseId = 1,
                    DoctorId = "pat",
                    DateTime = DateTime.UtcNow
                },

                new Appointment
                {
                    AppointmentCauseId = 2,
                    DoctorId = "pat",
                    DateTime = DateTime.UtcNow
                },

                new Appointment
                {
                    AppointmentCauseId = 3,
                    DoctorId = "pat",
                    DateTime = DateTime.UtcNow.AddDays(10)
                },
            };

            await dataMock.Appointments.AddRangeAsync(appointments);
            await dataMock.SaveChangesAsync();

            var service = new AppointmentService(dataMock, mapperMock);
            var result = await service.GetTotalAppointmentsCount();

            Assert.Equal(3, result);
        }

        [Fact]
        public async Task DeleteAppointmentShouldReturnTrueIfEverythingIsCorrect()
        {
            var dataMock = DatabaseMock.Instance;
            var mapperMock = MapperMock.Instance;

            var doctor = new Doctor()
            {
                Id = "pat",
                FirstName = "Evlogi",
                LastName = "Manev",
                PhoneNumber = "098787862",
                UserId = "patpat"
            };

            var causes = new List<AppointmentCause>()
            {
                    new AppointmentCause { Id = 1, Name = "Start" },
                    new AppointmentCause { Id = 2, Name = "End" },
                    new AppointmentCause { Id = 3, Name = "Middle" },
            };

            await dataMock.Doctors.AddAsync(doctor);
            await dataMock.AppointmentCauses.AddRangeAsync(causes);
            await dataMock.SaveChangesAsync();

            var appointments = new List<Appointment>()
            {
                new Appointment
                {
                    Id = 1,
                    AppointmentCauseId = 1,
                    DoctorId = "pat",
                    DateTime = DateTime.UtcNow
                },

                new Appointment
                {
                    Id = 2,
                    AppointmentCauseId = 2,
                    DoctorId = "pat",
                    DateTime = DateTime.UtcNow
                },

                new Appointment
                {
                    Id = 3,
                    AppointmentCauseId = 3,
                    DoctorId = "pat",
                    DateTime = DateTime.UtcNow.AddDays(10)
                },
            };

            await dataMock.Appointments.AddRangeAsync(appointments);
            await dataMock.SaveChangesAsync();

            var service = new AppointmentService(dataMock, mapperMock);
            var result = await service.DeleteAppointment(3);

            Assert.True(result);
        }

        [Fact]
        public async Task DeleteAppointmentShouldSetIsDeletedToTrueWhenSuccessful()
        {
            var dataMock = DatabaseMock.Instance;
            var mapperMock = MapperMock.Instance;

            var doctor = new Doctor()
            {
                Id = "pat",
                FirstName = "Evlogi",
                LastName = "Manev",
                PhoneNumber = "098787862",
                UserId = "patpat"
            };

            var causes = new List<AppointmentCause>()
            {
                    new AppointmentCause { Id = 1, Name = "Start" },
                    new AppointmentCause { Id = 2, Name = "End" },
                    new AppointmentCause { Id = 3, Name = "Middle" },
            };

            await dataMock.Doctors.AddAsync(doctor);
            await dataMock.AppointmentCauses.AddRangeAsync(causes);
            await dataMock.SaveChangesAsync();

            var appointments = new List<Appointment>()
            {
                new Appointment
                {
                    Id = 1,
                    AppointmentCauseId = 1,
                    DoctorId = "pat",
                    DateTime = DateTime.UtcNow
                },

                new Appointment
                {
                    Id = 2,
                    AppointmentCauseId = 2,
                    DoctorId = "pat",
                    DateTime = DateTime.UtcNow
                },

                new Appointment
                {
                    Id = 3,
                    AppointmentCauseId = 3,
                    DoctorId = "pat",
                    DateTime = DateTime.UtcNow.AddDays(10)
                },
            };

            await dataMock.Appointments.AddRangeAsync(appointments);
            await dataMock.SaveChangesAsync();

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

            var doctor = new Doctor()
            {
                Id = "pat",
                FirstName = "Evlogi",
                LastName = "Manev",
                PhoneNumber = "098787862",
                UserId = "patpat"
            };

            var causes = new List<AppointmentCause>()
            {
                    new AppointmentCause { Id = 1, Name = "Start" },
                    new AppointmentCause { Id = 2, Name = "End" },
                    new AppointmentCause { Id = 3, Name = "Middle" },
            };

            await dataMock.Doctors.AddAsync(doctor);
            await dataMock.AppointmentCauses.AddRangeAsync(causes);
            await dataMock.SaveChangesAsync();

            var appointments = new List<Appointment>()
            {
                new Appointment
                {
                    Id = 1,
                    AppointmentCauseId = 1,
                    DoctorId = "pat",
                    DateTime = DateTime.UtcNow
                },

                new Appointment
                {
                    Id = 2,
                    AppointmentCauseId = 2,
                    DoctorId = "pat",
                    DateTime = DateTime.UtcNow
                },

                new Appointment
                {
                    Id = 3,
                    AppointmentCauseId = 3,
                    DoctorId = "pat",
                    DateTime = DateTime.UtcNow.AddDays(10)
                },
            };

            await dataMock.Appointments.AddRangeAsync(appointments);
            await dataMock.SaveChangesAsync();

            var service = new AppointmentService(dataMock, mapperMock);
            var result = await service.DeleteAppointment(5);

            Assert.False(result);
        }

        [Fact]
        public async Task DeleteAppointmentShouldReturnFalseIfAppointmentIsAlreadyDeleted()
        {
            var dataMock = DatabaseMock.Instance;
            var mapperMock = MapperMock.Instance;

            var doctor = new Doctor()
            {
                Id = "pat",
                FirstName = "Evlogi",
                LastName = "Manev",
                PhoneNumber = "098787862",
                UserId = "patpat"
            };

            var causes = new List<AppointmentCause>()
            {
                    new AppointmentCause { Id = 1, Name = "Start" },
                    new AppointmentCause { Id = 2, Name = "End" },
                    new AppointmentCause { Id = 3, Name = "Middle" },
            };

            await dataMock.Doctors.AddAsync(doctor);
            await dataMock.AppointmentCauses.AddRangeAsync(causes);
            await dataMock.SaveChangesAsync();

            var appointments = new List<Appointment>()
            {
                new Appointment
                {
                    Id = 1,
                    AppointmentCauseId = 1,
                    DoctorId = "pat",
                    DateTime = DateTime.UtcNow
                },

                new Appointment
                {
                    Id = 2,
                    AppointmentCauseId = 2,
                    DoctorId = "pat",
                    DateTime = DateTime.UtcNow
                },

                new Appointment
                {
                    Id = 3,
                    AppointmentCauseId = 3,
                    DoctorId = "pat",
                    DateTime = DateTime.UtcNow.AddDays(10),
                    IsDeleted = true
                },
            };

            await dataMock.Appointments.AddRangeAsync(appointments);
            await dataMock.SaveChangesAsync();

            var service = new AppointmentService(dataMock, mapperMock);
            var result = await service.DeleteAppointment(3);

            Assert.False(result);
        }
    }
}