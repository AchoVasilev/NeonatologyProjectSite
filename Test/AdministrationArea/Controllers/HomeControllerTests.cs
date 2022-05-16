namespace Test.AdministrationArea.Controllers;

using System.Collections.Generic;
using System.Threading.Tasks;
using Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using Neonatology.Areas.Administration.Controllers;
using Services.AppointmentService;
using Services.PatientService;
using Services.RatingService;
using ViewModels.Administration.Home;
using ViewModels.Appointments;
using Xunit;

public class HomeControllerTests
{
    [Fact]
    public async Task HomeShouldReturnViewWithModel()
    {
        var appointmentServiceMock = new Mock<IAppointmentService>();
        var patientServiceMock = new Mock<IPatientService>();
        var ratingServiceMock = new Mock<IRatingService>();

        var appointments = new List<AppointmentViewModel>();
        appointmentServiceMock.Setup(x => x.GetTotalAppointmentsCount())
            .ReturnsAsync(5);
        appointmentServiceMock.Setup(x => x.GetAllAppointments())
            .ReturnsAsync(appointments);

        patientServiceMock.Setup(x => x.GetLastThisMonthsRegisteredCount())
            .ReturnsAsync(5);

        ratingServiceMock.Setup(x => x.GetRatingsCount())
            .ReturnsAsync(5);
        
        var httpContext = new DefaultHttpContext();
        var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>())
        {
            ["SessionVariable"] = "admin"
        };

        var controller = new HomeController(appointmentServiceMock.Object, patientServiceMock.Object, ratingServiceMock.Object)
        {
            TempData = tempData
        };

        ControllerExtensions.WithIdentity(controller, "1", "gosho@abv.bg", "Administrator");

        var result = await controller.Index();

        var route = Assert.IsType<ViewResult>(result);
        Assert.IsType<IndexViewModel>(route.Model);
    }
}