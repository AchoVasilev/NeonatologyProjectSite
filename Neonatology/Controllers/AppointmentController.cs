namespace Neonatology.Controllers
{
    using System;
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
    using static Common.GlobalConstants.Messages;

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
        public IActionResult MakeAnAppointment(string doctorId = DoctorId)
        {
            var viewModel = new CreateAppointmentModel
            {
                DoctorId = doctorId
            };

            return View(viewModel);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> MakeAnAppointment(CreateAppointmentModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            DateTime dateTime;

            try
            {
                dateTime = this.dateTimeParserService.ConvertStrings(model.Date, model.Time);
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(MakeAnAppointment), new { model.DoctorId });
            }

            var result = await this.appointmentService.AddAsync(model.DoctorId, model, dateTime);

            if (result == false)
            {
                return RedirectToAction("MakeAnAppointment", new { model.DoctorId });
            }

            if (result == true)
            {
                this.TempData["Message"] = string.Format(SuccessfullAppointment, model.Date, model.Time);
            }

            return RedirectToAction("Index", "Home", new { area = "" });
        }

        [Authorize(Roles = PatientRoleName)]
        public IActionResult MakePatientAppointment(string doctorId = DoctorId)
        {
            var viewModel = new PatientAppointmentCreateModel
            {
                DoctorId = doctorId
            };

            return View(viewModel);
        }

        [Authorize(Roles = PatientRoleName)]
        [HttpPost]
        public async Task<IActionResult> MakePatientAppointment(PatientAppointmentCreateModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            DateTime dateTime;

            try
            {
                dateTime = this.dateTimeParserService.ConvertStrings(model.Date, model.Time);
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(MakeAnAppointment), new { model.DoctorId });
            }

            var userId = this.User.GetId();
            var patientId = await this.patientService.GetPatientIdByUserIdAsync(userId);

            model.PatientId = patientId;
            var result = await this.appointmentService.AddAsync(model.DoctorId, model, dateTime);

            if (result == false)
            {
                return RedirectToAction("MakePatientAppointment", new { model.DoctorId });
            }

            if (result == true)
            {
                this.TempData["Message"] = string.Format(SuccessfullAppointment, model.Date, model.Time);
            }

            return RedirectToAction("MyAppointments", "Appointment", new { area = "" });
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
