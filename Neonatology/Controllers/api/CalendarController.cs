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

    using static Common.GlobalConstants;

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

        [AllowAnonymous]
        [HttpGet("getSlots/gabrovo")]
        public async Task<JsonResult> GetCalendarGabrovoSlots()
        {
            var slots = await this.slotService.GetFreeGabrovoSlots();
            
            return new JsonResult(slots);
        }

        [AllowAnonymous]
        [HttpGet("getSlots/pleven")]
        public async Task<JsonResult> GetCalendarPlevenSlots()
        {
            var slots = await this.slotService.GetFreePlevenSlots();

            return new JsonResult(slots);
        }
        
        [AllowAnonymous]
        [HttpPost("makeAppointment/{id}")]
        public async Task<IActionResult> MakeAnAppointment(CreateAppointmentModel model, string id)
        {
            var startDate = DateTime.Parse(model.Start);
            var endDate = DateTime.Parse(model.End);

            var cause = await this.appointmentCauseService.GetAppointmentCauseByIdAsync(model.AppointmentCauseId);
            if (cause == null)
            {
                return BadRequest(new { message = MessageConstants.AppointmentCauseWrongId });
            }

            if (startDate.Date < DateTime.Now.Date && startDate.Hour < DateTime.Now.Hour)
            {
                return BadRequest(new { message = MessageConstants.AppointmentBeforeNowErrorMsg });
            }

            if (await this.patientService.PatientExists(model.Email))
            {
                return BadRequest(new { message = MessageConstants.PatientIsRegistered });
            }

            var result = await this.appointmentService.AddAsync(model.DoctorId, model, startDate, endDate);
            if (result == false)
            {
                return BadRequest(new { message = MessageConstants.TakenDateMsg });
            }

            var slotId = await this.slotService.DeleteSlotById(int.Parse(id));
            if (slotId == 0)
            {
                return BadRequest(new { message = MessageConstants.TakenDateMsg });
            }

            var emailMsg = string
                .Format(MessageConstants.AppointmentMakeEmailMsg,
                startDate.ToString(DateTimeFormats.TimeFormat), startDate.ToString(DateTimeFormats.DateFormat));

            try
            {
                await this.emailSender.SendEmailAsync(model.Email, SuccessfulApointmentEmailMsgSubject, emailMsg);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return Ok(new
            {
                message = string
                    .Format(MessageConstants.SuccessfullAppointment,
                    startDate.ToString(DateTimeFormats.DateFormat), startDate.ToString(DateTimeFormats.TimeFormat)),
                slotId
            });
        }

        [Authorize(Roles = PatientRoleName)]
        [HttpPost("makePatientAppointment/{id}")]
        public async Task<IActionResult> MakePatientAppointment(PatientAppointmentCreateModel model, string id)
        {
            var cause = await this.appointmentCauseService.GetAppointmentCauseByIdAsync(model.AppointmentCauseId);
            if (cause == null)
            {
                return BadRequest(new { message = MessageConstants.AppointmentCauseWrongId });
            }

            if (model.Start.Date < DateTime.Now.Date && model.Start.Hour < DateTime.Now.Hour)
            {
                return BadRequest(new { message = MessageConstants.AppointmentBeforeNowErrorMsg });
            }

            var userId = this.User.GetId();
            var userEmail = this.User.FindFirst(ClaimTypes.Email).Value;
            var patientId = await this.patientService.GetPatientIdByUserIdAsync(userId);

            model.PatientId = patientId;
            var result = await this.appointmentService.AddAsync(model.DoctorId, model);

            if (result == false)
            {
                return BadRequest(new { message = MessageConstants.TakenDateMsg });
            }

            await this.slotService.DeleteSlotById(int.Parse(id));

            var emailMsg = string
                .Format(MessageConstants.AppointmentMakeEmailMsg, 
                model.Start.ToString(DateTimeFormats.TimeFormat), model.Start.ToString(DateTimeFormats.DateFormat));

            try
            {
                await this.emailSender.SendEmailAsync(userEmail, SuccessfulApointmentEmailMsgSubject, emailMsg);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return Ok(new
            {
                message = string
                    .Format(MessageConstants.SuccessfullAppointment, 
                    model.Start.ToString(DateTimeFormats.DateFormat), model.Start.ToString(DateTimeFormats.TimeFormat)),
            });
        }
    }
}
