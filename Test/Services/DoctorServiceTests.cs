namespace Test.Services
{
    using System.Threading.Tasks;

    using Data.Models;

    using global::Services.DoctorService;

    using NuGet.ContentModel;

    using Test.Mocks;

    using Xunit;

    public class DoctorServiceTests
    {
        [Fact]
        public async Task GetDoctorIdByUserIdShouldReturnId()
        {
            var dataMock = DatabaseMock.Instance;
            var mapperMock = MapperMock.Instance;

            var user = new ApplicationUser()
            {
                Id = "user",
                Doctor = new Doctor()
                {
                    Id = "doc",
                    FirstName = "Gosho",
                    LastName = "Goshev",
                    Age = 27,
                    Address = "Kaspichan Street",
                },
                Image = new Image()
                {
                    Url = "asd.bg"
                }
            };

            await dataMock.Users.AddAsync(user);
            await dataMock.SaveChangesAsync();

            var service = new DoctorService(dataMock, mapperMock, null, null);
            var result = await service.GetDoctorIdByUserId("user");

            Assert.NotNull(result);
            Asset.Equals("user", result);
            Assert.IsType<string>(result);
        }

        [Fact]
        public async Task GetDoctorIdByAppointmentIdShouldReturnCorrectId()
        {
            var dataMock = DatabaseMock.Instance;
            var mapperMock = MapperMock.Instance;

            var user = new ApplicationUser()
            {
                Id = "user",
                Doctor = new Doctor()
                {
                    Id = "doc",
                    FirstName = "Gosho",
                    LastName = "Goshev",
                    Age = 27,
                    Address = "Kaspichan Street",
                },
                Image = new Image()
                {
                    Url = "asd.bg"
                }
            };

            var appointmentCause = new AppointmentCause
            {
                Id = 1,
                Name = "Check"
            };

            var appointment = new Appointment
            {
                Id = 1,
                AppointmentCauseId = 1,
                DoctorId = "doc"
            };

            await dataMock.Users.AddAsync(user);
            await dataMock.AppointmentCauses.AddAsync(appointmentCause);
            await dataMock.Appointments.AddAsync(appointment);
            await dataMock.SaveChangesAsync();

            var service = new DoctorService(dataMock, mapperMock, null, null);
            var result = await service.GetDoctorIdByAppointmentId(1);

            Assert.Equal("doc", result);
        }

        [Fact]
        public async Task GetDoctorIdShouldReturnCorrectId()
        {
            var dataMock = DatabaseMock.Instance;
            var mapperMock = MapperMock.Instance;

            var user = new ApplicationUser()
            {
                Id = "user",
                Doctor = new Doctor()
                {
                    Id = "doc",
                    FirstName = "Gosho",
                    LastName = "Goshev",
                    Age = 27,
                    Address = "Kaspichan Street",
                },
                Image = new Image()
                {
                    Url = "asd.bg"
                }
            };

            await dataMock.Users.AddAsync(user);
            await dataMock.SaveChangesAsync();

            var service = new DoctorService(dataMock, mapperMock, null, null);
            var result = await service.GetDoctorId();

            Assert.Equal("doc", result);
        }

        [Fact]
        public async Task GetDoctorEmailShouldReturnCorrectEmail()
        {
            var dataMock = DatabaseMock.Instance;
            var mapperMock = MapperMock.Instance;

            var user = new ApplicationUser()
            {
                Id = "user",
                Doctor = new Doctor()
                {
                    Id = "doc",
                    FirstName = "Gosho",
                    LastName = "Goshev",
                    Age = 27,
                    Address = "Kaspichan Street",
                    Email = "gosho@gosho.bg"
                },
                Image = new Image()
                {
                    Url = "asd.bg"
                }
            };

            await dataMock.Users.AddAsync(user);
            await dataMock.SaveChangesAsync();

            var service = new DoctorService(dataMock, mapperMock, null, null);
            var result = await service.GetDoctorEmail("doc");

            Assert.Equal("gosho@gosho.bg", result);
        }
    }
}
