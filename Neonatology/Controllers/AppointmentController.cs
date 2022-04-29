namespace Neonatology.Controllers
{
    using System.Threading.Tasks;

    using Infrastructure;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using Services.AppointmentCauseService;
    using Services.AppointmentService;
    using Services.DoctorService;
    using Services.PatientService;

    using ViewModels.Appointments;

    using static Common.GlobalConstants;
    using static Common.GlobalConstants.MessageConstants;

    public class AppointmentController : BaseController
    {
        private readonly IAppointmentService appointmentService;
        private readonly IPatientService patientService;
        private readonly IDoctorService doctorService;
        private readonly IAppointmentCauseService appointmentCauseService;

        public AppointmentController(
            IAppointmentService appointmentService,
            IPatientService patientService,
            IDoctorService doctorService,
            IAppointmentCauseService appointmentCauseService)
        {
            this.appointmentService = appointmentService;
            this.patientService = patientService;
            this.doctorService = doctorService;
            this.appointmentCauseService = appointmentCauseService;
        }

        [AllowAnonymous]
        public async Task<IActionResult> MakeAnAppointment()
        {
            var viewModel = new CreateAppointmentModel
            {
                DoctorId = await this.doctorService.GetDoctorId(),
                AppointmentCauses = await this.appointmentCauseService.GetAllCauses()
            };

            return View(viewModel);
        }

        [Authorize(Roles = PatientRoleName)]
        public async Task<IActionResult> MakePatientAppointment()
        {
            var patient = await this.patientService.GetPatientByUserIdAsync(this.User.GetId());

            if (string.IsNullOrWhiteSpace(patient.FirstName) || 
                string.IsNullOrWhiteSpace(patient.LastName) || 
                string.IsNullOrWhiteSpace(patient.Phone))
            {
                this.TempData["Message"] = PatientProfileIsNotFinishedMsg;

                return RedirectToAction("Finish", "Patient", new { area = "" });
            }

            var viewModel = new PatientAppointmentCreateModel
            {
                DoctorId = await this.doctorService.GetDoctorId(),
                AppointmentCauses = await this.appointmentCauseService.GetAllCauses()
            };

            return View(viewModel);
        }

        [Authorize(Roles = PatientRoleName)]
        public async Task<IActionResult> MyUpcomingAppointments([FromQuery] int page)
        {
            var userId = this.User.GetId();
            var patientId = await this.patientService.GetPatientIdByUserIdAsync(userId);

            var model = await this.appointmentService.GetUpcomingUserAppointments(patientId, ItemsPerPage, page);

            return View(model);
        }

        [Authorize(Roles = PatientRoleName)]
        public async Task<IActionResult> MyPastAppointments([FromQuery] int page)
        {
            var userId = this.User.GetId();
            var patientId = await this.patientService.GetPatientIdByUserIdAsync(userId);

            var model = await this.appointmentService.GetPastUserAppointments(patientId, ItemsPerPage, page);

            return View(model);
        }

        [Authorize(Roles = DoctorConstants.DoctorRoleName)]
        public async Task<IActionResult> DoctorUpcomingAppointments([FromQuery] int page)
        {
            var userId = this.User.GetId();
            var doctorId = await this.doctorService.GetDoctorIdByUserId(userId);

            var model = await this.appointmentService.GetUpcomingDoctorAppointments(doctorId, ItemsPerPage, page);

            return View(model);
        }

        [Authorize(Roles = DoctorConstants.DoctorRoleName)]
        public async Task<IActionResult> DoctorPastAppointments([FromQuery] int page)
        {
            var userId = this.User.GetId();
            var doctorId = await this.doctorService.GetDoctorIdByUserId(userId);

            var model = await this.appointmentService.GetPastDoctorAppointments(doctorId, ItemsPerPage, page);

            return View(model);
        }

        [Authorize(Roles = DoctorConstants.DoctorRoleName)]
        public async Task<IActionResult> TodaysAppointments()
        {
            var appointments = await this.appointmentService.GetTodaysAppointments(this.User.GetId());

            return View(appointments);
        }
    }
}