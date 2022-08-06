namespace Test.ControllerUnitTests;

using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

using Moq;

using Neonatology.Controllers;

using Services.AppointmentService;
using Services.DoctorService;
using Services.PatientService;
using Services.RatingService;

using Helpers;

using ViewModels.Appointments;
using ViewModels.Rating;

using Xunit;

public class RatingControllerTests
{
    [Fact]
    public void ControllerShouldHaveAuthorizeAttribute()
    {
        var controller = new RatingController(null, null, null, null);
        var actualAttribute = controller.GetType()
            .GetCustomAttributes(typeof(AuthorizeAttribute), true);

        Assert.Equal(typeof(AuthorizeAttribute), actualAttribute[0].GetType());
    }

    [Fact]
    public void RateAppointmentShouldReturnModelWithView()
    {
        var controller = new RatingController(null, null, null, null);
        ControllerExtensions.WithIdentity(controller, "1", "Gosho", "Patient");

        var result = controller.RateAppointment(1);

        var route = Assert.IsType<ViewResult>(result);
        Assert.IsType<CreateRatingFormModel>(route.Model);
    }

    [Fact]
    public async Task RateAppointmentWithModelShouldReturnRedirectToActionToMyAppointmentsWhenSuccessful()
    {
        var model = new CreateRatingFormModel
        {
            AppointmentId = 1,
            Comment = "goshogoshogosho",
            DoctorId = "doc",
            Number = 5,
            PatientId = "patient"
        };

        var appointmentService = new Mock<IAppointmentService>();
        appointmentService.Setup(x => x.GetUserAppointmentAsync("1", model.AppointmentId))
            .ReturnsAsync(new AppointmentViewModel() { IsRated = false });

        var doctorService = new Mock<IDoctorService>();
        doctorService.Setup(x => x.GetDoctorIdByAppointmentId(model.AppointmentId))
            .ReturnsAsync("doc");

        var patientService = new Mock<IPatientService>();
        patientService.Setup(x => x.GetPatientIdByUserId("1"))
            .ReturnsAsync("patient");

        var ratingService = new Mock<IRatingService>();
        ratingService.Setup(x => x.AddAsync(model))
            .ReturnsAsync(true);

        var httpContext = new DefaultHttpContext();
        var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>())
        {
            ["SessionVariable"] = "admin"
        };

        var controller = new RatingController(appointmentService.Object, doctorService.Object, patientService.Object, ratingService.Object)
        {
            TempData = tempData
        };

        ControllerExtensions.WithIdentity(controller, "1", "gosho@abv.bg", "Patient");

        var result = await controller.RateAppointment(model);

        var route = Assert.IsType<RedirectToActionResult>(result);

        Assert.Equal("MyPastAppointments", route.ActionName);
        Assert.Equal("Appointment", route.ControllerName);
    }

    [Fact]
    public async Task RateAppointmentShouldReturnStatusCode404IfAppointmentIsNull()
    {
        var model = new CreateRatingFormModel
        {
            AppointmentId = 1,
            Comment = "goshogoshogosho",
            DoctorId = "doc",
            Number = 5,
            PatientId = "patient"
        };

        var appointmentService = new Mock<IAppointmentService>();
        appointmentService.Setup(x => x.GetUserAppointmentAsync("1", model.AppointmentId))
            .ReturnsAsync(value: null);

        var controller = new RatingController(appointmentService.Object, null, null, null);
        ControllerExtensions.WithIdentity(controller, "1", "gosho@abv.bg", "Patient");

        var result = await controller.RateAppointment(model);

        var route = Assert.IsType<StatusCodeResult>(result);

        Assert.Equal(404, route.StatusCode);
    }

    [Fact]
    public async Task RateAppointmentShouldRedirectToMyAppointmentsIfAppointmentIsRated()
    {
        var model = new CreateRatingFormModel
        {
            AppointmentId = 1,
            Comment = "goshogoshogosho",
            DoctorId = "doc",
            Number = 5,
            PatientId = "patient"
        };

        var appointmentService = new Mock<IAppointmentService>();
        appointmentService.Setup(x => x.GetUserAppointmentAsync("1", model.AppointmentId))
            .ReturnsAsync(new AppointmentViewModel() { IsRated = true });

        var httpContext = new DefaultHttpContext();
        var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>())
        {
            ["SessionVariable"] = "admin"
        };

        var controller = new RatingController(appointmentService.Object, null, null, null)
        {
            TempData = tempData
        };

        ControllerExtensions.WithIdentity(controller, "1", "gosho@abv.bg", "Patient");

        var result = await controller.RateAppointment(model);

        var route = Assert.IsType<RedirectToActionResult>(result);

        Assert.Equal("MyPastAppointments", route.ActionName);
        Assert.Equal("Appointment", route.ControllerName);
    }

    [Fact]
    public async Task RateAppointmentShouldReturnStatusCode404IfDoctorIdIsNull()
    {
        var model = new CreateRatingFormModel
        {
            AppointmentId = 1,
            Comment = "goshogoshogosho",
            DoctorId = "doc",
            Number = 5,
            PatientId = "patient"
        };

        var appointmentService = new Mock<IAppointmentService>();
        appointmentService.Setup(x => x.GetUserAppointmentAsync("1", model.AppointmentId))
            .ReturnsAsync(new AppointmentViewModel() { IsRated = false });

        var doctorService = new Mock<IDoctorService>();
        doctorService.Setup(x => x.GetDoctorIdByAppointmentId(model.AppointmentId))
            .ReturnsAsync(value: null);

        var controller = new RatingController(appointmentService.Object, doctorService.Object, null, null);

        ControllerExtensions.WithIdentity(controller, "1", "gosho@abv.bg", "Patient");

        var result = await controller.RateAppointment(model);

        var route = Assert.IsType<StatusCodeResult>(result);

        Assert.Equal(404, route.StatusCode);
    }

    [Fact]
    public async Task RateAppointmentShouldReturnStatusCodeResult404IfPatientIdIsNull()
    {
        var model = new CreateRatingFormModel
        {
            AppointmentId = 1,
            Comment = "goshogoshogosho",
            DoctorId = "doc",
            Number = 5,
            PatientId = "patient"
        };

        var appointmentService = new Mock<IAppointmentService>();
        appointmentService.Setup(x => x.GetUserAppointmentAsync("1", model.AppointmentId))
            .ReturnsAsync(new AppointmentViewModel() { IsRated = false });

        var doctorService = new Mock<IDoctorService>();
        doctorService.Setup(x => x.GetDoctorIdByAppointmentId(model.AppointmentId))
            .ReturnsAsync("doc");

        var patientService = new Mock<IPatientService>();
        patientService.Setup(x => x.GetPatientIdByUserId("1"))
            .ReturnsAsync(value: null);

        var controller = new RatingController(appointmentService.Object, doctorService.Object, patientService.Object, null);

        ControllerExtensions.WithIdentity(controller, "1", "gosho@abv.bg", "Patient");

        var result = await controller.RateAppointment(model);

        var route = Assert.IsType<StatusCodeResult>(result);

        Assert.Equal(404, route.StatusCode);
    }

    [Fact]
    public async Task RateAppointmentShouldRedirectToAllAppointmentsIfAppointmentCanNotBeRated()
    {
        var model = new CreateRatingFormModel
        {
            AppointmentId = 1,
            Comment = "goshogoshogosho",
            DoctorId = "doc",
            Number = 5,
            PatientId = "patient"
        };

        var appointmentService = new Mock<IAppointmentService>();
        appointmentService.Setup(x => x.GetUserAppointmentAsync("1", model.AppointmentId))
            .ReturnsAsync(new AppointmentViewModel() { IsRated = false });

        var doctorService = new Mock<IDoctorService>();
        doctorService.Setup(x => x.GetDoctorIdByAppointmentId(model.AppointmentId))
            .ReturnsAsync("doc");

        var patientService = new Mock<IPatientService>();
        patientService.Setup(x => x.GetPatientIdByUserId("1"))
            .ReturnsAsync("patient");

        var ratingService = new Mock<IRatingService>();
        ratingService.Setup(x => x.AddAsync(model))
            .ReturnsAsync(false);

        var httpContext = new DefaultHttpContext();
        var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>())
        {
            ["SessionVariable"] = "admin"
        };

        var controller = new RatingController(appointmentService.Object, doctorService.Object, patientService.Object, ratingService.Object)
        {
            TempData = tempData
        };

        ControllerExtensions.WithIdentity(controller, "1", "gosho@abv.bg", "Patient");

        var result = await controller.RateAppointment(model);

        var route = Assert.IsType<RedirectToActionResult>(result);

        Assert.Equal("MyPastAppointments", route.ActionName);
        Assert.Equal("Appointment", route.ControllerName);
    }

    [Fact]
    public async Task ApproveShouldRedirectToDoctorAppoitmentsIfSuccesful()
    {
        var ratingService = new Mock<IRatingService>();
        ratingService.Setup(x => x.ApproveRating(1))
            .ReturnsAsync(true);

        var httpContext = new DefaultHttpContext();
        var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>())
        {
            ["SessionVariable"] = "admin"
        };

        var controller = new RatingController(null, null, null, ratingService.Object)
        {
            TempData = tempData
        };

        ControllerExtensions.WithIdentity(controller, "1", "gosho@abv.bg", "Doctor");

        var result = await controller.Approve(1);

        var route = Assert.IsType<RedirectToActionResult>(result);

        Assert.Equal("DoctorPastAppointments", route.ActionName);
        Assert.Equal("Appointment", route.ControllerName);
    }

    [Fact]
    public async Task ApproveShouldRedirectToAdminAreaIfUserIsAdmin()
    {
        var ratingService = new Mock<IRatingService>();
        ratingService.Setup(x => x.ApproveRating(1))
            .ReturnsAsync(true);

        var httpContext = new DefaultHttpContext();
        var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>())
        {
            ["SessionVariable"] = "admin"
        };

        var controller = new RatingController(null, null, null, ratingService.Object)
        {
            TempData = tempData
        };

        ControllerExtensions.WithIdentity(controller, "1", "gosho@abv.bg", "Administrator");

        var result = await controller.Approve(1);

        var route = Assert.IsType<RedirectToActionResult>(result);

        Assert.Equal("All", route.ActionName);
        Assert.Equal("Appointment", route.ControllerName);
        Assert.Equal("Administration", route.RouteValues.Values.First());
    }

    [Fact]
    public async Task ApproveShouldRedirectToDoctorAppoitmentsIfSuccesfulIfRatingIsNotSuccessful()
    {
        var ratingService = new Mock<IRatingService>();
        ratingService.Setup(x => x.ApproveRating(1))
            .ReturnsAsync(false);

        var httpContext = new DefaultHttpContext();
        var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>())
        {
            ["SessionVariable"] = "admin"
        };

        var controller = new RatingController(null, null, null, ratingService.Object)
        {
            TempData = tempData
        };

        ControllerExtensions.WithIdentity(controller, "1", "gosho@abv.bg", "Doctor");

        var result = await controller.Approve(1);

        var route = Assert.IsType<RedirectToActionResult>(result);

        Assert.Equal("DoctorPastAppointments", route.ActionName);
        Assert.Equal("Appointment", route.ControllerName);
    }

    [Fact]
    public async Task ApproveShouldRedirectToAdminAreaIfUserIsAdminIfRatingIsNotSuccessful()
    {
        var ratingService = new Mock<IRatingService>();
        ratingService.Setup(x => x.ApproveRating(1))
            .ReturnsAsync(false);

        var httpContext = new DefaultHttpContext();
        var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>())
        {
            ["SessionVariable"] = "admin"
        };

        var controller = new RatingController(null, null, null, ratingService.Object)
        {
            TempData = tempData
        };

        ControllerExtensions.WithIdentity(controller, "1", "gosho@abv.bg", "Administrator");

        var result = await controller.Approve(1);

        var route = Assert.IsType<RedirectToActionResult>(result);

        Assert.Equal("All", route.ActionName);
        Assert.Equal("Appointment", route.ControllerName);
        Assert.Equal("Administration", route.RouteValues.Values.First());
    }

    [Fact]
    public async Task DeleteShouldRedirectToDoctorAppointmentsIfSuccessful()
    {
        var ratingService = new Mock<IRatingService>();
        ratingService.Setup(x => x.DeleteRating(1))
            .ReturnsAsync(true);

        var httpContext = new DefaultHttpContext();
        var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>())
        {
            ["SessionVariable"] = "admin"
        };

        var controller = new RatingController(null, null, null, ratingService.Object)
        {
            TempData = tempData
        };

        ControllerExtensions.WithIdentity(controller, "1", "gosho@abv.bg", "Doctor");

        var result = await controller.Delete(1);

        var route = Assert.IsType<RedirectToActionResult>(result);

        Assert.Equal("DoctorPastAppointments", route.ActionName);
        Assert.Equal("Appointment", route.ControllerName);
    }

    [Fact]
    public async Task DeleteShouldRedirectToAdminAreaIfUserIsAdmin()
    {
        var ratingService = new Mock<IRatingService>();
        ratingService.Setup(x => x.DeleteRating(1))
            .ReturnsAsync(true);

        var httpContext = new DefaultHttpContext();
        var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>())
        {
            ["SessionVariable"] = "admin"
        };

        var controller = new RatingController(null, null, null, ratingService.Object)
        {
            TempData = tempData
        };

        ControllerExtensions.WithIdentity(controller, "1", "gosho@abv.bg", "Administrator");

        var result = await controller.Delete(1);

        var route = Assert.IsType<RedirectToActionResult>(result);

        Assert.Equal("All", route.ActionName);
        Assert.Equal("Appointment", route.ControllerName);
        Assert.Equal("Administration", route.RouteValues.Values.First());
    }

    [Fact]
    public async Task DeleteShouldRedirectToDoctorAppointmentsIfSuccessfulIfRatingIsNotSuccessful()
    {
        var ratingService = new Mock<IRatingService>();
        ratingService.Setup(x => x.DeleteRating(1))
            .ReturnsAsync(false);

        var httpContext = new DefaultHttpContext();
        var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>())
        {
            ["SessionVariable"] = "admin"
        };

        var controller = new RatingController(null, null, null, ratingService.Object)
        {
            TempData = tempData
        };

        ControllerExtensions.WithIdentity(controller, "1", "gosho@abv.bg", "Doctor");

        var result = await controller.Delete(1);

        var route = Assert.IsType<RedirectToActionResult>(result);

        Assert.Equal("DoctorPastAppointments", route.ActionName);
        Assert.Equal("Appointment", route.ControllerName);
    }

    [Fact]
    public async Task DeleteShouldRedirectToAdminAreaIfUserIsAdminIfRatingIsNotSuccessful()
    {
        var ratingService = new Mock<IRatingService>();
        ratingService.Setup(x => x.DeleteRating(1))
            .ReturnsAsync(false);

        var httpContext = new DefaultHttpContext();
        var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>())
        {
            ["SessionVariable"] = "admin"
        };

        var controller = new RatingController(null, null, null, ratingService.Object)
        {
            TempData = tempData
        };

        ControllerExtensions.WithIdentity(controller, "1", "gosho@abv.bg", "Administrator");

        var result = await controller.Delete(1);

        var route = Assert.IsType<RedirectToActionResult>(result);

        Assert.Equal("All", route.ActionName);
        Assert.Equal("Appointment", route.ControllerName);
        Assert.Equal("Administration", route.RouteValues.Values.First());
    }
}