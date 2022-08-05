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
    using static Common.GlobalConstants.MessageConstants;

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

            return this.View(model);
        }

        [Authorize(Roles = PatientRoleName)]
        [HttpPost]
        public async Task<IActionResult> RateAppointment(CreateRatingFormModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.RedirectToAction(nameof(RateAppointment), new { model.AppointmentId });
            }

            var userId = this.User.GetId();
            var appointment = await this.appointmentService.GetUserAppointmentAsync(userId, model.AppointmentId);

            if (appointment is null)
            {
                return new StatusCodeResult(404);
            }

            if (appointment.IsRated)
            {
                this.TempData["Message"] = RatedAppointment;
                return this.RedirectToAction("MyPastAppointments", "Appointment", new { area = "", page = 1 });
            }

            var doctorId = await this.doctorService.GetDoctorIdByAppointmentId(model.AppointmentId);
            if (doctorId is null)
            {
                return new StatusCodeResult(404);
            }

            var patientId = await this.patientService.GetPatientIdByUserId(userId);
            if (patientId is null)
            {
                return new StatusCodeResult(404);
            }

            model.PatientId = patientId;
            model.DoctorId = doctorId;

            var isRated = await this.ratingService.AddAsync(model);

            if (isRated == false)
            {
                this.TempData["Message"] = ErrorRatingAppointmentMsg;
                return this.RedirectToAction("MyPastAppointments", "Appointment", new { area = "", page = 1 });
            }

            this.TempData["Message"] = string.Format(SuccessfulRating, model.Number);

            return this.RedirectToAction("MyPastAppointments", "Appointment", new { area = "", page = 1 });
        }

        [Authorize(Roles = $"{DoctorConstants.DoctorRoleName}, {AdministratorRoleName}")]
        public async Task<IActionResult> Approve(int appointmentId)
        {
            var isApproved = await this.ratingService.ApproveRating(appointmentId);
            var userIsAdmin = this.User.IsAdmin();

            if (isApproved == false)
            { 
                this.TempData["Message"] = ErrorApprovingRating;

                if (userIsAdmin)
                {
                    return this.RedirectToAction("All", "Appointment", new { area = "Administration" });
                }

                return this.RedirectToAction("DoctorPastAppointments", "Appointment", new { area = "", page = 1 });
            }

            this.TempData["Message"] = SuccessfullyApprovedRating;

            if (userIsAdmin)
            {
                return this.RedirectToAction("All", "Appointment", new { area = "Administration" });
            }

            return this.RedirectToAction("DoctorPastAppointments", "Appointment", new { area = "", page = 1 });
        }

        [Authorize(Roles = $"{DoctorConstants.DoctorRoleName}, {AdministratorRoleName}")]
        public async Task<IActionResult> Delete(int appointmentId)
        {
            var isDeleted = await this.ratingService.DeleteRating(appointmentId);
            var userIsAdmin = this.User.IsAdmin();

            if (isDeleted == false)
            {
                this.TempData["Message"] = ErrorApprovingRating;
                if (userIsAdmin)
                {
                    return this.RedirectToAction("All", "Appointment", new { area = "Administration" });
                }

                return this.RedirectToAction("DoctorPastAppointments", "Appointment", new { area = "", page = 1 });
            }

            this.TempData["Message"] = SuccessfullyApprovedRating;
            if (userIsAdmin)
            {
                return this.RedirectToAction("All", "Appointment", new { area = "Administration" });
            }

            return this.RedirectToAction("DoctorPastAppointments", "Appointment", new { area = "", page = 1 });
        }
    }
}