namespace Test.AdministrationArea.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Data.Models;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;

    using Moq;

    using Neonatology.Areas.Administration.Controllers;

    using Services.AppointmentService;

    using Test.Mocks;
    using ViewModels.Administration.Appointment;
    using ViewModels.Appointments;

    using Xunit;

    public class AppointmentControllerTests
    {
        [Fact]
        public async Task AllShouldReturnCorrectViewAndModel()
        {
            // Arrange
            var appointments = new List<AppointmentViewModel>();
            var service = new Mock<IAppointmentService>();
            service.Setup(x => x.GetAllAppointments())
                .ReturnsAsync(appointments);

            // Act
            var controller = new AppointmentController(service.Object, null);
            var result = await controller.All();

            // Assert 
            var route = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<ICollection<AppointmentViewModel>>(route.Model);
        }

        [Fact]
        public async Task InformationShouldReturnCorrectViewAndModel()
        {
            // Arrange
            var service = new Mock<IAppointmentService>();
            service.Setup(x => x.GetAppointmentByIdAsync(1))
                .ReturnsAsync(new Appointment());

            var mapper = MapperMock.Instance;
            // Act
            var controller = new AppointmentController(service.Object, mapper);
            var result = await controller.Information(1);

            // Assert 
            var route = Assert.IsType<ViewResult>(result);
            Assert.IsType<AdminAppointmentViewModel>(route.Model);
        }

        [Fact]
        public async Task DeleteShouldReturnRedirectToAllWithTempDataMessage()
        {
            // Arrange
            var service = new Mock<IAppointmentService>();
            service.Setup(x => x.DeleteAppointment(1))
                .ReturnsAsync(true);

            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>())
            {
                ["SessionVariable"] = "admin"
            };

            var controller = new AppointmentController(service.Object, null)
            {
                TempData = tempData
            };

            // Act
            var result = await controller.Delete(1);

            // Assert 
            var route = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("All", route.ActionName);
        }
    }
}
