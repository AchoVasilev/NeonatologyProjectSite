namespace Neonatology.Controllers
{
    using System.Threading.Tasks;

    using Infrastructure;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using Services.AppointmentService;
    using Services.DateTimeParser;
    using Services.DoctorService;
    using Services.PatientService;

    using ViewModels.Appointments;

    using static Common.GlobalConstants;

    public class AppointmentController : BaseController
    {
        private readonly IDateTimeParserService dateTimeParserService;
        private readonly IAppointmentService appointmentService;
        private readonly IPatientService patientService;
        private readonly IDoctorService doctorService;

        public AppointmentController(
            IDateTimeParserService dateTimeParserService,
            IAppointmentService appointmentService,
            IPatientService patientService,
            IDoctorService doctorService)
        {
            this.dateTimeParserService = dateTimeParserService;
            this.appointmentService = appointmentService;
            this.patientService = patientService;
            this.doctorService = doctorService;
        }

        [AllowAnonymous]
        public async Task<IActionResult> MakeAnAppointment()
        {
            var viewModel = new CreateAppointmentModel
            {
                DoctorId = await this.doctorService.GetDoctorId()
            };

            return View(viewModel);
        }

        [Authorize(Roles = PatientRoleName)]
        public async Task<IActionResult> MakePatientAppointment()
        {
            var viewModel = new PatientAppointmentCreateModel
            {
                DoctorId = await this.doctorService.GetDoctorId()
            };

            return View(viewModel);
        }

        [Authorize(Roles = PatientRoleName)]
        public async Task<IActionResult> MyAppointments()
        {
            var userId = this.User.GetId();
            var patientId = await this.patientService.GetPatientIdByUserIdAsync(userId);

            var model = new AllAppointmentsViewModel()
            {
                Upcoming = this.appointmentService.GetUpcomingUserAppointments(patientId),
                Past = this.appointmentService.GetPastUserAppointments(patientId)
            };

            return View(model);
        }

        [Authorize(Roles = DoctorRoleName)]
        public async Task<IActionResult> DoctorAppointments()
        {
            var userId = this.User.GetId();
            var doctorId = await this.doctorService.GetDoctorIdByUserId(userId);

            var model = new AllAppointmentsViewModel()
            {
                Upcoming = this.appointmentService.GetUpcomingDoctorAppointments(doctorId),
                Past = this.appointmentService.GetPastDoctorAppointments(doctorId)
            };

            return View(model);
        }
    }
}
