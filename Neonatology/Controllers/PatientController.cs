namespace Neonatology.Controllers
{
    using System.Threading.Tasks;

    using Infrastructure;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;

    using Services.PatientService;

    using ViewModels.Patient;

    [Authorize]
    public class PatientController : BaseController
    {
        private readonly IPatientService patientService;
        private readonly IWebHostEnvironment environment;

        public PatientController(IPatientService patientService, IWebHostEnvironment environment)
        {
            this.patientService = patientService;
            this.environment = environment;
        }

        public IActionResult Finish()
            =>
                this.View(new CreatePatientFormModel());

        [HttpPost]
        public async Task<IActionResult> Finish(CreatePatientFormModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            var userId = this.User.GetId();

            await this.patientService.CreatePatient(model, userId, $"{this.environment.WebRootPath}");

            return this.RedirectToAction("Index", "Home");
        }
    }
}
