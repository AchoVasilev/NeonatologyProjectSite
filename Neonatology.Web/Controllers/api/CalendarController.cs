namespace Neonatology.Web.Controllers.api;

using System;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Services.AppointmentCauseService;
using Services.AppointmentService;
using Services.PatientService;
using Services.SlotService;
using ViewModels.Appointments;
using static Common.Constants.GlobalConstants;
using static Common.Constants.WebConstants.RouteTemplates;

public class CalendarController : ApiController
{
    private readonly IAppointmentService appointmentService;
    private readonly ISlotService slotService;
    private readonly IPatientService patientService;
    private readonly IAppointmentCauseService appointmentCauseService;
    private readonly IEmailSender emailSender;
    private readonly IMapper mapper;

    public CalendarController(
        ISlotService slotService,
        IAppointmentService appointmentService,
        IEmailSender emailSender,
        IAppointmentCauseService appointmentCauseService,
        IPatientService patientService, 
        IMapper mapper)
    {
        this.slotService = slotService;
        this.appointmentService = appointmentService;
        this.emailSender = emailSender;
        this.patientService = patientService;
        this.mapper = mapper;
        this.appointmentCauseService = appointmentCauseService;
    }

    [AllowAnonymous]
    [HttpGet]
    [Route(CalendarGetGabrovoSlots)]
    public async Task<JsonResult> GetCalendarGabrovoSlots()
    {
        var slots = await this.slotService.GetFreeGabrovoSlots();
            
        return new JsonResult(slots);
    }

    [AllowAnonymous]
    [HttpGet]
    [Route(CalendarGetPlevenSlots)]
    public async Task<JsonResult> GetCalendarPlevenSlots()
    {
        var slots = await this.slotService.GetFreePlevenSlots();

        return new JsonResult(slots);
    }
        
    [AllowAnonymous]
    [HttpPost]
    [Route(CalendarMakeAnAppointment)]
    public async Task<IActionResult> MakeAnAppointment(CreateAppointmentModel model, string id)
    {
        var startDate = DateTime.Parse(model.Start);
        var endDate = DateTime.Parse(model.End);

        var cause = await this.appointmentCauseService.AppointmentCauseExists(model.AppointmentCauseId);
        if (cause.Failed)
        {
            return this.BadRequest(new { message = cause.Error });
        }

        if (await this.patientService.PatientExists(model.Email))
        {
            return this.BadRequest(new { message = MessageConstants.PatientIsRegistered });
        }

        var serviceModel = this.mapper.Map<CreateAppointmentServiceModel>(model);
        var result = await this.appointmentService.AddAsync(model.DoctorId, serviceModel, startDate, endDate);
        if (result.Failed)
        {
            return this.BadRequest(new { message = result.Error });
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
    [HttpPost]
    [Route(CalendarMakePatientAppointment)]
    public async Task<IActionResult> MakePatientAppointment(PatientAppointmentCreateModel model, string id)
    {
        var startDate = DateTime.Parse(model.Start);
        var endDate = DateTime.Parse(model.End);
        var cause = await this.appointmentCauseService.AppointmentCauseExists(model.AppointmentCauseId);
        if (cause.Failed)
        {
            return this.BadRequest(new { message = cause.Error });
        }

        var userId = this.User.GetId();
        var patientId = await this.patientService.GetPatientIdByUserId(userId);

        model.PatientId = patientId;
        
        var serviceModel = this.mapper.Map<CreatePatientAppointmentModel>(model);
        
        var result = await this.appointmentService.AddAsync(model.DoctorId, serviceModel, startDate, endDate);
        if (result.Failed)
        {
            return this.BadRequest(new { message = result.Error });
        }

        var slotId = await this.slotService.DeleteSlotById(int.Parse(id));
        if (slotId == 0)
        {
            return this.BadRequest(new { message = MessageConstants.TakenDateMsg });
        }
        
        var emailMsg = string
            .Format(MessageConstants.AppointmentMakeEmailMsg, 
                startDate.ToString(DateTimeFormats.TimeFormat), startDate.ToString(DateTimeFormats.DateFormat));

        var userEmail = this.User.FindFirst(ClaimTypes.Email).Value;

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