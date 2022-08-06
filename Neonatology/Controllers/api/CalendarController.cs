namespace Neonatology.Controllers.api;

using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Infrastructure.Extensions;
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
            return this.BadRequest(new { message = MessageConstants.AppointmentCauseWrongId });
        }

        if (startDate.Date < DateTime.Now.Date && startDate.Hour < DateTime.Now.Hour)
        {
            return this.BadRequest(new { message = MessageConstants.AppointmentBeforeNowErrorMsg });
        }

        if (await this.patientService.PatientExists(model.Email))
        {
            return this.BadRequest(new { message = MessageConstants.PatientIsRegistered });
        }

        var result = await this.appointmentService.AddAsync(model.DoctorId, model, startDate, endDate);
        if (result == false)
        {
            return this.BadRequest(new { message = MessageConstants.TakenDateMsg });
        }

        var slotId = await this.slotService.DeleteSlotById(int.Parse(id));
        if (slotId == 0)
        {
            return this.BadRequest(new { message = MessageConstants.TakenDateMsg });
        }

        var emailMsg = string
            .Format(MessageConstants.AppointmentMakeEmailMsg,
                startDate.ToString(DateTimeFormats.TimeFormat), startDate.ToString(DateTimeFormats.DateFormat));

        try
        {
            await this.emailSender.SendEmailAsync(model.Email, SuccessfulAppointmentEmailMsgSubject, emailMsg);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        return this.Ok(new
        {
            message = string
                .Format(MessageConstants.SuccessfulAppointment,
                    startDate.ToString(DateTimeFormats.DateFormat), startDate.ToString(DateTimeFormats.TimeFormat)),
            slotId
        });
    }

    [Authorize(Roles = PatientRoleName)]
    [HttpPost("makePatientAppointment/{id}")]
    public async Task<IActionResult> MakePatientAppointment(PatientAppointmentCreateModel model, string id)
    {
        var startDate = DateTime.Parse(model.Start);
        var endDate = DateTime.Parse(model.End);
        var cause = await this.appointmentCauseService.GetAppointmentCauseByIdAsync(model.AppointmentCauseId);
        if (cause == null)
        {
            return this.BadRequest(new { message = MessageConstants.AppointmentCauseWrongId });
        }

        if (startDate.Date < DateTime.Now.Date && startDate.Hour < DateTime.Now.Hour)
        {
            return this.BadRequest(new { message = MessageConstants.AppointmentBeforeNowErrorMsg });
        }

        var userId = this.User.GetId();
        var userEmail = this.User.FindFirst(ClaimTypes.Email).Value;
        var patientId = await this.patientService.GetPatientIdByUserId(userId);

        model.PatientId = patientId;
        var result = await this.appointmentService.AddAsync(model.DoctorId, model, startDate, endDate);

        if (result == false)
        {
            return this.BadRequest(new { message = MessageConstants.TakenDateMsg });
        }

        await this.slotService.DeleteSlotById(int.Parse(id));

        var emailMsg = string
            .Format(MessageConstants.AppointmentMakeEmailMsg, 
                startDate.ToString(DateTimeFormats.TimeFormat), startDate.ToString(DateTimeFormats.DateFormat));

        try
        {
            await this.emailSender.SendEmailAsync(userEmail, SuccessfulAppointmentEmailMsgSubject, emailMsg);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        return this.Ok(new
        {
            message = string
                .Format(MessageConstants.SuccessfulAppointment, 
                    startDate.ToString(DateTimeFormats.DateFormat), startDate.ToString(DateTimeFormats.TimeFormat)),
        });
    }
}