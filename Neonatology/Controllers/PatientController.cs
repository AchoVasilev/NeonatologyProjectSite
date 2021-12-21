namespace Neonatology.Controllers
{
    using System.Threading.Tasks;

    using Infrastructure;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using Services.PatientService;

    using ViewModels.Patient;

    using static Common.GlobalConstants;

    [Authorize(Roles = PatientRoleName)]
    public class PatientController : BaseController
    {
        private readonly IPatientService patientService;

        public PatientController(IPatientService patientService)
        {
            this.patientService = patientService;
        }

        public async Task<IActionResult> Finish()
        {
            var patient = await this.patientService.GetPatientByUserIdAsync(this.User.GetId());
            var model = new CreatePatientFormModel()
            {
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                PhoneNumber = patient.Phone
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Finish(CreatePatientFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userId = this.User.GetId();
            var patientId = await this.patientService.GetPatientIdByUserIdAsync(userId);

            if (string.IsNullOrWhiteSpace(patientId) == false)
            {
                var isEdited = await this.patientService.EditPatientAsync(patientId, model);

                if (isEdited)
                {
                    return RedirectToAction("MyAppointments", "Appointment", new { area = "" });
                }
            }

            await this.patientService.CreatePatientAsync(model, userId);

            return RedirectToAction("MyAppointments", "Appointment", new { area = "" });
        }
    }
}
