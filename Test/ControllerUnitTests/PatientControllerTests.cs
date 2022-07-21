namespace Test.ControllerUnitTests
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;

    using Moq;

    using Neonatology.Controllers;

    using Services.PatientService;

    using Helpers;

    using ViewModels.Patient;

    using Xunit;

    public class PatientControllerTests
    {
        [Fact]
        public void PatientControllerShouldHaveAuthorizeAttribute()
        {
            var controller = new PatientController(null, null);
            var actualAttribute = controller.GetType()
                .GetCustomAttributes(typeof(AuthorizeAttribute), true);

            Assert.Equal(typeof(AuthorizeAttribute), actualAttribute[0].GetType());
        }

        [Fact]
        public void FinishShouldReturnViewWithModel()
        {
            var controller = new PatientController(null, null);
            ControllerExtensions.WithIdentity(controller, "1", "gosho", "Patient");

            var result = controller.Finish();
            var route = Assert.IsType<ViewResult>(result);
            Assert.IsType<CreatePatientFormModel>(route.Model);
        }

        [Fact]
        public async Task FinishShouldRedirectToMyAppointmentsActionInAppointmentControllerWhenSuccesful()
        {
            var model = new CreatePatientFormModel();
            var patientService = new Mock<IPatientService>();
            var environment = new Mock<IWebHostEnvironment>();
            patientService.Setup(x => x.CreatePatientAsync(model, "1", "root"));

            var controller = new PatientController(patientService.Object, environment.Object);
            ControllerExtensions.WithIdentity(controller, "1", "gosho", "Patient");

            var result = await controller.Finish(model);
            var root = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", root.ActionName);
            Assert.Equal("Home", root.ControllerName);
        }
    }
}
