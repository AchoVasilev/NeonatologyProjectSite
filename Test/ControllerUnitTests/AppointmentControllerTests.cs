namespace Test.ControllerUnitTests
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;

    using Moq;

    using Neonatology.Controllers;

    using Services.AppointmentCauseService;
    using Services.AppointmentService;
    using Services.DoctorService;
    using Services.PatientService;

    using Helpers;

    using ViewModels.Appointments;
    using ViewModels.Patient;

    using Xunit;

    public class AppointmentControllerTests
    {
        [Fact]
        public async Task MakeAppointmentShouldReturnCorrectViewWithModel()
        {
            var doctorService = new Mock<IDoctorService>();
            doctorService.Setup(x => x.GetDoctorId())
                .ReturnsAsync("doc");

            var appointmentCauses = new List<AppointmentCauseViewModel>();

            var appCauseService = new Mock<IAppointmentCauseService>();
            appCauseService.Setup(x => x.GetAllCauses())
                .ReturnsAsync(appointmentCauses);

            var controller = new AppointmentController(null, null, doctorService.Object, appCauseService.Object);

            var result = await controller.MakeAnAppointment();
            var route = Assert.IsType<ViewResult>(result);

            Assert.IsType<CreateAppointmentModel>(route.Model);
        }

        [Fact]
        public void MakePatientAppointmentShouldHaveAuthorizeAttribute()
        {
            var controller = new AppointmentController(null, null, null, null);

            var actualAttribute = controller.GetType()
                .GetMethod("MakePatientAppointment")
                .GetCustomAttributes(typeof(AuthorizeAttribute), true);

            Assert.Equal(typeof(AuthorizeAttribute), actualAttribute[0].GetType());
        }

        [Fact]
        public async Task MakePatientAppointmentShouldReturnCorrectViewWithModel()
        {
            var patientModel = new PatientViewModel
            {
                Id = "1",
                FirstName = "Gosho",
                LastName = "Pesho",
                Phone = "098889902"
            };
            var patientService = new Mock<IPatientService>();
            patientService.Setup(x => x.GetPatientByUserId("1"))
                .ReturnsAsync(patientModel);

            var doctorService = new Mock<IDoctorService>();
            doctorService.Setup(x => x.GetDoctorId())
                .ReturnsAsync("doc");

            var appointmentCauses = new List<AppointmentCauseViewModel>();

            var appCauseService = new Mock<IAppointmentCauseService>();
            appCauseService.Setup(x => x.GetAllCauses())
                .ReturnsAsync(appointmentCauses);

            var controller = new AppointmentController(null, patientService.Object, doctorService.Object, appCauseService.Object);
            ControllerExtensions.WithIdentity(controller, "1", "gosho", "Patient");

            var result = await controller.MakePatientAppointment();

            var route = Assert.IsType<ViewResult>(result);
            Assert.IsType<PatientAppointmentCreateModel>(route.Model);
        }

        [Fact]
        public async Task MakePatientAppointmentShouldReturnRedirectToActionIfPropertiesOfThePatientViewModelAreNull()
        {
            var patientModel = new PatientViewModel
            {
                Id = "1",
                FirstName = " ",
                LastName = string.Empty,
                Phone = null
            };
            var patientService = new Mock<IPatientService>();
            patientService.Setup(x => x.GetPatientByUserId("1"))
                .ReturnsAsync(patientModel);

            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>())
            {
                ["SessionVariable"] = "admin"
            };

            var controller = new AppointmentController(null, patientService.Object, null, null)
            {
                TempData = tempData
            };
            ControllerExtensions.WithIdentity(controller, "1", "gosho", "Patient");

            var result = await controller.MakePatientAppointment();

            var route = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Finish", route.ActionName);
            Assert.Equal("Patient", route.ControllerName);
        }

        [Fact]
        public void MyUpcomingAppointmentsShouldHaveAuthorizeAttribute()
        {
            var controller = new AppointmentController(null, null, null, null);

            var actualAttribute = controller.GetType()
                .GetMethod("MyUpcomingAppointments")
                .GetCustomAttributes(typeof(AuthorizeAttribute), true);

            Assert.Equal(typeof(AuthorizeAttribute), actualAttribute[0].GetType());
        }

        [Fact]
        public void MyPastAppointmentsShouldHaveAuthorizeAttribute()
        {
            var controller = new AppointmentController(null, null, null, null);

            var actualAttribute = controller.GetType()
                .GetMethod("MyPastAppointments")
                .GetCustomAttributes(typeof(AuthorizeAttribute), true);

            Assert.Equal(typeof(AuthorizeAttribute), actualAttribute[0].GetType());
        }

        [Fact]
        public async Task MyUpcomingAppointmentsShouldReturnViewWithModel()
        {
            var patientService = new Mock<IPatientService>();
            patientService.Setup(x => x.GetPatientIdByUserId("1"))
                .ReturnsAsync("1");

            var appointment = new AllAppointmentsViewModel();
            var appointmentService = new Mock<IAppointmentService>();
            appointmentService.Setup(x => x.GetUpcomingUserAppointments("1", 8, 1))
                .ReturnsAsync(appointment);

            var controller = new AppointmentController(appointmentService.Object, patientService.Object, null, null);
            ControllerExtensions.WithIdentity(controller, "1", "gosho", "Patient");

            var result = await controller.MyUpcomingAppointments(1);

            var route = Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void DoctorUpcomingAppointmentsShouldHaveAuthorizeAttribute()
        {
            var controller = new AppointmentController(null, null, null, null);

            var actualAttribute = controller.GetType()
                .GetMethod("DoctorUpcomingAppointments")
                .GetCustomAttributes(typeof(AuthorizeAttribute), true);

            Assert.Equal(typeof(AuthorizeAttribute), actualAttribute[0].GetType());
        }

        [Fact]
        public void DoctorPastAppointmentsShouldHaveAuthorizeAttribute()
        {
            var controller = new AppointmentController(null, null, null, null);

            var actualAttribute = controller.GetType()
                .GetMethod("DoctorPastAppointments")
                .GetCustomAttributes(typeof(AuthorizeAttribute), true);

            Assert.Equal(typeof(AuthorizeAttribute), actualAttribute[0].GetType());
        }

        [Fact]
        public async Task DoctorAppointmentsShouldReturnViewWithModel()
        {
            var doctorService = new Mock<IDoctorService>();
            doctorService.Setup(x => x.GetDoctorIdByUserId("1"))
                .ReturnsAsync("1");

            var appointment = new AllAppointmentsViewModel()
            {
                Appointments = new List<AppointmentViewModel>(),
                ItemCount = 5,
                ItemsPerPage = 8,
                PageNumber = 1
            };

            var appointmentService = new Mock<IAppointmentService>();
            appointmentService.Setup(x => x.GetUpcomingDoctorAppointments("1", 8, 1))
                .ReturnsAsync(appointment);
            appointmentService.Setup(x => x.GetPastDoctorAppointments("1", 8, 1))
                .ReturnsAsync(appointment);

            var controller = new AppointmentController(appointmentService.Object, null, doctorService.Object, null);
            ControllerExtensions.WithIdentity(controller, "1", "gosho", "Patient");

            var result = await controller.DoctorUpcomingAppointments(1);

            var route = Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void TodaysAppointmentsShouldHaveAuthorizeAttribute()
        {
            var controller = new AppointmentController(null, null, null, null);

            var actualAttribute = controller.GetType()
                .GetMethod("TodaysAppointments")
                .GetCustomAttributes(typeof(AuthorizeAttribute), true);

            Assert.Equal(typeof(AuthorizeAttribute), actualAttribute[0].GetType());
        }

        [Fact]
        public async Task TodaysAppointmentsShouldReturnViewWithModel()
        {
            var appointment = new List<AppointmentViewModel>();
            var appointmentService = new Mock<IAppointmentService>();
            appointmentService.Setup(x => x.GetTodaysAppointments("1"))
                .ReturnsAsync(appointment);

            var controller = new AppointmentController(appointmentService.Object, null, null, null);
            ControllerExtensions.WithIdentity(controller, "1", "gosho", "Patient");

            var result = await controller.TodaysAppointments();

            var route = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<ICollection<AppointmentViewModel>>(route.Model);
        }
    }
}
