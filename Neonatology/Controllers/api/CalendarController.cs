namespace Neonatology.Controllers.api
{
    using System;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Infrastructure;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity.UI.Services;
    using Microsoft.AspNetCore.Mvc;

    using Services.AppointmentCauseService;
    using Services.AppointmentService;
    using Services.PatientService;
    using Services.SlotService;

    using ViewModels.Appointments;
    using ViewModels.Slot;

    using static Common.GlobalConstants;
    using static Common.GlobalConstants.Messages;

    [ApiController]
    [Route("[controller]")]
    public class CalendarController : ControllerBase
    {
        private readonly IAppointmentService appointmentService;
        private readonly ISlotService slotService;
        private readonly IPatientService patientService;
        private readonly IAppointmentCauseService appointmentCauseService;
        private readonly IEmailSender emailSender;

        public CalendarController(
            ISlotService slotService,
            IAppointmentService appointmentService,
            IEmailSender emailSender,
            IAppointmentCauseService appointmentCauseService,
            IPatientService patientService)
        {
            this.slotService = slotService;
            this.appointmentService = appointmentService;
            this.emailSender = emailSender;
            this.patientService = patientService;
            this.appointmentCauseService = appointmentCauseService;
        }

        [Authorize(Roles = DoctorRoleName)]
        [HttpGet]
        public async Task<JsonResult> GetDoctorAppointments()
        {
            var result = await this.appointmentService.GetAllAppointments();

            foreach (var res in result)
            {
                res.DateTime = res.DateTime.ToLocalTime();
                res.End = res.DateTime.ToLocalTime();
            }

            return new JsonResult(result);
        }

        [Authorize(Roles = DoctorRoleName)]
        [HttpPost("generate")]
        public async Task<IActionResult> GenerateSlots([FromBody] SlotInputModel model)
        {
            if (model.Start.Date < DateTime.Now.Date)
            {
                return BadRequest(new { response = DateBeforeNowErrorMsg });
            }

            if (model.Start >= model.End)
            {
                return BadRequest(new { response = StartDateIsAfterEndDateMsg });
            }

            var result = await this.slotService.GenerateSlots(model.Start.ToLocalTime(), model.End.ToLocalTime(), model.SlotDurationMinutes);

            return new JsonResult(result);
        }

        [AllowAnonymous]
        [HttpGet("getSlots")]
        public async Task<JsonResult> GetCalendarSlots()
        {
            var slots = await this.slotService.GetSlots();

            return new JsonResult(slots);
        }

        [Authorize(Roles = DoctorRoleName)]
        [HttpPut("{id}")]
        public async Task<IActionResult> EditSlot(SlotEditModel model)
        {
            var isEdited = await this.slotService.EditSlot(model.Id, model.Status);

            if (isEdited == false)
            {
                return BadRequest(new { message = FailedSlotEditMsg });
            }

            return Ok(model.Id);
        }

        [AllowAnonymous]
        [HttpGet("appointments")]
        public async Task<IActionResult> TakenAppointments()
        {
            var appointments = await this.appointmentService.GetTakenAppointmentSlots();

            if (appointments == null)
            {
                return NoContent();
            }

            return new JsonResult(appointments);
        }

        [AllowAnonymous]
        [HttpPost("makeAppointment/{id}")]
        public async Task<IActionResult> MakeAnAppointment(CreateAppointmentModel model, int id)
        {
            var cause = this.appointmentCauseService.GetAppointmentCauseByIdAsync(model.AppointmentCauseId);
            if (cause == null)
            {
                return BadRequest(new { message = AppointmentCauseWrongId });
            }
            
            if (model.Start.Date < DateTime.Now.Date)
            {
                return BadRequest(new { message = AppointmentBeforeNowErrorMsg });
            }

            await this.slotService.DeleteSlotById(id);

            var result = await this.appointmentService.AddAsync(model.DoctorId, model);

            if (result == false)
            {
                return BadRequest(new { message = TakenDateMsg });
            }

            var emailMsg = string.Format(AppointmentMakeEmailMsg, model.Start.ToLocalTime().Hour, model.Start.ToString("dd/MM/yyyy"));

            await this.emailSender.SendEmailAsync(model.Email, SuccessfulApointmentEmailMsgSubject, emailMsg);

            return Ok(new
            {
                message = string.Format(SuccessfullAppointment, model.Start.ToString("dd/MM/yyyy"), model.Start.ToLocalTime().Hour),
            });
        }

        [Authorize(Roles = PatientRoleName)]
        [HttpPost("makePatientAppointment/{id}")]
        public async Task<IActionResult> MakePatientAppointment(PatientAppointmentCreateModel model, int id)
        {
            var cause = this.appointmentCauseService.GetAppointmentCauseByIdAsync(model.AppointmentCauseId);
            if (cause == null)
            {
                return BadRequest(new { message = AppointmentCauseWrongId });
            }

            var userId = this.User.GetId();
            var userEmail = this.User.FindFirst(ClaimTypes.Email).Value;
            var patientId = await this.patientService.GetPatientIdByUserIdAsync(userId);

            model.PatientId = patientId;
            var result = await this.appointmentService.AddAsync(model.DoctorId, model);

            if (model.Start.Date < DateTime.Now.Date)
            {
                return BadRequest(new { response = AppointmentBeforeNowErrorMsg });
            }

            await this.slotService.DeleteSlotById(id);

            if (result == false)
            {
                return BadRequest(new { message = TakenDateMsg });
            }

            var emailMsg = string.Format(AppointmentMakeEmailMsg, model.Start.Hour.ToString("hh/mm"), model.Start.ToString("dd/MM/yyyy"));

            await this.emailSender.SendEmailAsync(userEmail, SuccessfulApointmentEmailMsgSubject, emailMsg);

            return Ok(new
            {
                message = string.Format(SuccessfullAppointment, model.Start.ToString("dd/MM/yyyy"), model.Start.Hour.ToString("hh/mm")),
            });
        }
    }
}
