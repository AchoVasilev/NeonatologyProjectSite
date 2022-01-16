﻿namespace Neonatology.Controllers.api
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
    using static Common.GlobalConstants.Messages;
    using static Common.GlobalConstants.DateTimeFormats;

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
        [HttpGet("getSlots")]
        public async Task<JsonResult> GetCalendarSlots()
        {
            var slots = await this.slotService.GetPatientSlots();
            
            return new JsonResult(slots);
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
        public async Task<IActionResult> MakeAnAppointment(CreateAppointmentModel model, string id)
        {
            var cause = this.appointmentCauseService.GetAppointmentCauseByIdAsync(model.AppointmentCauseId);
            if (cause == null)
            {
                return BadRequest(new { message = AppointmentCauseWrongId });
            }
            
            if (model.Start.Date < DateTime.Now.Date && model.Start.Hour < DateTime.Now.Hour)
            {
                return BadRequest(new { message = AppointmentBeforeNowErrorMsg });
            }

            if (await this.patientService.PatientExists(model.Email))
            {
                return BadRequest(new { message = PatientIsRegistered });
            }

            var result = await this.appointmentService.AddAsync(model.DoctorId, model);
            if (result == false)
            {
                return BadRequest(new { message = TakenDateMsg });
            }

            await this.slotService.DeleteSlotById(int.Parse(id));

            var emailMsg = string
                .Format(AppointmentMakeEmailMsg, model.Start.ToString(TimeFormat), model.Start.ToString(DateFormat));
            await this.emailSender.SendEmailAsync(model.Email, SuccessfulApointmentEmailMsgSubject, emailMsg);

            return Ok(new
            {
                message = string
                    .Format(SuccessfullAppointment, model.Start.ToString(DateFormat), model.Start.ToString(TimeFormat)),
            });
        }

        [Authorize(Roles = PatientRoleName)]
        [HttpPost("makePatientAppointment/{id}")]
        public async Task<IActionResult> MakePatientAppointment(PatientAppointmentCreateModel model, string id)
        {
            var cause = await this.appointmentCauseService.GetAppointmentCauseByIdAsync(model.AppointmentCauseId);
            if (cause == null)
            {
                return BadRequest(new { message = AppointmentCauseWrongId });
            }

            if (model.Start.Date < DateTime.Now.Date && model.Start.Hour < DateTime.Now.Hour)
            {
                return BadRequest(new { message = AppointmentBeforeNowErrorMsg });
            }

            var userId = this.User.GetId();
            var userEmail = this.User.FindFirst(ClaimTypes.Email).Value;
            var patientId = await this.patientService.GetPatientIdByUserIdAsync(userId);

            model.PatientId = patientId;
            var result = await this.appointmentService.AddAsync(model.DoctorId, model);

            if (result == false)
            {
                return BadRequest(new { message = TakenDateMsg });
            }

            await this.slotService.DeleteSlotById(int.Parse(id));

            var emailMsg = string
                .Format(AppointmentMakeEmailMsg, model.Start.ToString(TimeFormat), model.Start.ToString(DateFormat));

            await this.emailSender.SendEmailAsync(userEmail, SuccessfulApointmentEmailMsgSubject, emailMsg);

            return Ok(new
            {
                message = string
                    .Format(SuccessfullAppointment, model.Start.ToString(DateFormat), model.Start.ToString(TimeFormat)),
            });
        }
    }
}
