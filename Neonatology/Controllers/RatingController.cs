namespace Neonatology.Controllers
{
    using System.Threading.Tasks;

    using Infrastructure;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using Services.AppointmentService;
    using Services.DoctorService;
    using Services.PatientService;
    using Services.RatingService;

    using ViewModels.Rating;

    using static Common.GlobalConstants;
    using static Common.GlobalConstants.Messages;

    [Authorize]
    public class RatingController : BaseController
    {
        private readonly IAppointmentService appointmentService;
        private readonly IDoctorService doctorService;
        private readonly IPatientService patientService;
        private readonly IRatingService ratingService;
        public RatingController(
            IAppointmentService appointmentService,
            IDoctorService doctorService,
            IPatientService patientService,
            IRatingService ratingService)
        {
            this.appointmentService = appointmentService;
            this.doctorService = doctorService;
            this.patientService = patientService;
            this.ratingService = ratingService;
        }

        [Authorize(Roles = PatientRoleName)]
        public IActionResult RateAppointment(int appointmentId)
        {
            var model = new CreateRatingFormModel()
            {
                AppointmentId = appointmentId
            };

            return View(model);
        }

        [Authorize(Roles = PatientRoleName)]
        [HttpPost]
        public async Task<IActionResult> RateAppointment(CreateRatingFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(RateAppointment), new { model.AppointmentId });
            }

            var userId = this.User.GetId();
            var appointment = await this.appointmentService.GetUserAppointmentAsync(userId, model.AppointmentId);

            if (appointment == null)
            {
                return new StatusCodeResult(404);
            }

            if (appointment.IsRated)
            {
                this.TempData["Message"] = RatedAppointment;
                return RedirectToAction("MyAppointments", "Appointment");
            }

            var doctorId = await this.doctorService.GetDoctorIdByAppointmentId(model.AppointmentId);
            if (doctorId == null)
            {
                return new StatusCodeResult(404);
            }

            var patientId = await this.patientService.GetPatientIdByUserIdAsync(userId);
            if (patientId == null)
            {
                return new StatusCodeResult(404);
            }

            model.PatientId = patientId;
            model.DoctorId = doctorId;

            var isRated = await this.ratingService.AddAsync(model);

            if (isRated == false)
            {
                return new StatusCodeResult(404);
            }

            this.TempData["Message"] = string.Format(SuccessfulRating, model.Number);

            return RedirectToAction("MyAppointments", "Appointment", new { area = "" });
        }

        public async Task<IActionResult> Approve(int appointmentId)
        {
            var isApproved = await this.ratingService.ApproveRating(appointmentId);

            if (isApproved == false)
            {
                this.TempData["Message"] = ErrorApprovingRating;
            }

            this.TempData["Message"] = SuccessfullyApprovedRating;
            return RedirectToAction("DoctorAppointments", "Appointment", new { area = "" });
        }

        public async Task<IActionResult> Delete(int appointmentId)
        {
            var isApproved = await this.ratingService.DeleteRating(appointmentId);

            if (isApproved == false)
            {
                this.TempData["Message"] = ErrorApprovingRating;
            }

            this.TempData["Message"] = SuccessfullyApprovedRating;
            return RedirectToAction("DoctorAppointments", "Appointment", new { area = "" });
        }
    }
}
