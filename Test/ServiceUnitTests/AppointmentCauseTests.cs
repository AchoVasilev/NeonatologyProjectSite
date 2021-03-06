namespace Test.ServiceUnitTests
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Data.Models;

    using Services.AppointmentCauseService;

    using Mocks;

    using ViewModels.Appointments;

    using Xunit;

    public class AppointmentCauseTests
    {
        [Fact]
        public async Task GetAllCausesShouldReturnCorrectCount()
        {
            var databaseMock = DatabaseMock.Instance;
            var mapperMock = MapperMock.Instance;

            var causes = new List<AppointmentCause>()
            {
                    new AppointmentCause { Name = "Start" },
                    new AppointmentCause { Name = "End" },
                    new AppointmentCause { Name = "Middle" },
            };

            await databaseMock.AppointmentCauses.AddRangeAsync(causes);
            await databaseMock.SaveChangesAsync();

            var appointmentCauseService = new AppointmentCauseService(databaseMock, mapperMock);
            var result = await appointmentCauseService.GetAllCauses();

            Assert.Equal(3, result.Count);
        }

        [Fact]
        public async Task GetAllCausesShouldReturnCorrectCountIfSomeEntriesAreDeleted()
        {
            var databaseMock = DatabaseMock.Instance;
            var mapperMock = MapperMock.Instance;

            var causes = new List<AppointmentCause>()
            {
                    new AppointmentCause { Name = "Start" },
                    new AppointmentCause { Name = "End", IsDeleted = true },
                    new AppointmentCause { Name = "Middle" },
            };

            await databaseMock.AppointmentCauses.AddRangeAsync(causes);
            await databaseMock.SaveChangesAsync();

            var appointmentCauseService = new AppointmentCauseService(databaseMock, mapperMock);
            var result = await appointmentCauseService.GetAllCauses();

            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task GetAllCausesShouldReturnCorrectModel()
        {
            var databaseMock = DatabaseMock.Instance;
            var mapperMock = MapperMock.Instance;

            var causes = new List<AppointmentCause>()
            {
                    new AppointmentCause { Name = "Start" },
                    new AppointmentCause { Name = "End", IsDeleted = true },
                    new AppointmentCause { Name = "Middle" },
            };

            await databaseMock.AppointmentCauses.AddRangeAsync(causes);
            await databaseMock.SaveChangesAsync();

            var appointmentCauseService = new AppointmentCauseService(databaseMock, mapperMock);
            var result = await appointmentCauseService.GetAllCauses();

            Assert.NotNull(result);
            Assert.IsAssignableFrom<ICollection<AppointmentCauseViewModel>>(result);
        }

        [Fact]
        public async Task GetAppointmentCauseByIdShouldReturnCorrectAppointmentCause()
        {
            var databaseMock = DatabaseMock.Instance;
            var mapperMock = MapperMock.Instance;

            var causes = new List<AppointmentCause>()
            {
                    new AppointmentCause { Id = 1, Name = "Start" },
                    new AppointmentCause { Id = 2, Name = "End" },
                    new AppointmentCause { Id = 3, Name = "Middle" },
            };

            await databaseMock.AppointmentCauses.AddRangeAsync(causes);
            await databaseMock.SaveChangesAsync();

            var appointmentCauseService = new AppointmentCauseService(databaseMock, mapperMock);
            var result = await appointmentCauseService.GetAppointmentCauseByIdAsync(1);

            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Start", result.Name);
        }

        [Fact]
        public async Task GetAppointmentCauseByIdShouldReturnNullIfCauseIsDeleted()
        {
            var databaseMock = DatabaseMock.Instance;
            var mapperMock = MapperMock.Instance;

            var causes = new List<AppointmentCause>()
            {
                    new AppointmentCause { Id = 1, Name = "Start", IsDeleted = true },
                    new AppointmentCause { Id = 2, Name = "End" },
                    new AppointmentCause { Id = 3, Name = "Middle" },
            };

            await databaseMock.AppointmentCauses.AddRangeAsync(causes);
            await databaseMock.SaveChangesAsync();

            var appointmentCauseService = new AppointmentCauseService(databaseMock, mapperMock);
            var result = await appointmentCauseService.GetAppointmentCauseByIdAsync(1);

            Assert.Null(result);
        }

        [Fact]
        public async Task GetAppointmentCauseByIdShouldReturnCorrectModel()
        {
            var databaseMock = DatabaseMock.Instance;
            var mapperMock = MapperMock.Instance;

            var causes = new List<AppointmentCause>()
            {
                    new AppointmentCause { Id = 1, Name = "Start" },
                    new AppointmentCause { Id = 2, Name = "End" },
                    new AppointmentCause { Id = 3, Name = "Middle" },
            };

            await databaseMock.AppointmentCauses.AddRangeAsync(causes);
            await databaseMock.SaveChangesAsync();

            var appointmentCauseService = new AppointmentCauseService(databaseMock, mapperMock);
            var result = await appointmentCauseService.GetAppointmentCauseByIdAsync(1);

            Assert.NotNull(result);
            Assert.IsType<AppointmentCauseViewModel>(result);
        }
    }
}
