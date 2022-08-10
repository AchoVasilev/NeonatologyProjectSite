namespace Neonatology.Web.Controllers.api;

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.AppointmentService;
using Services.SlotService;
using ViewModels.Slot;
using static Common.Constants.GlobalConstants;
using static Common.Constants.WebConstants.RouteTemplates;

[Authorize(Roles = DoctorConstants.DoctorRoleName)]
public class DoctorCalendarController : ApiController
{
    private readonly IAppointmentService appointmentService;
    private readonly ISlotService slotService;

    public DoctorCalendarController(IAppointmentService appointmentService, ISlotService slotService)
    {
        this.appointmentService = appointmentService;
        this.slotService = slotService;
    }

    [HttpGet]
    [Route(DoctorCalendarGetGabrovoAppointments)]
    public async Task<JsonResult> GetDoctorGabrovoAppointments()
    {
        var result = await this.appointmentService.GetGabrovoAppointments();

        return new JsonResult(result);
    }

    [HttpGet]
    [Route(DoctorCalendarGetPlevenAppointments)]
    public async Task<JsonResult> GetDoctorPlevenAppointments()
    {
        var result = await this.appointmentService.GetPlevenAppointments();

        return new JsonResult(result);
    }

    [HttpPost]
    [Route(DoctorCalendarGenerate)]
    public async Task<IActionResult> GenerateSlots([FromBody] SlotInputModel model)
    {
        var startDate = DateTime.Parse(model.StartDate);
        var endDate = DateTime.Parse(model.EndDate);
        
        var result = await this.slotService
            .GenerateSlots(startDate, endDate, model.SlotDurationMinutes, model.AddressId);

        if (result.Failed)
        {
            return this.BadRequest(new { response = result.Error });
        }
        
        return new JsonResult(result);
    }

    [HttpGet]
    [Route(DoctorCalendarGetGabrovoSlots)]
    public async Task<JsonResult> GetCalendarGabrovoSlots()
    {
        var slots = await this.slotService.GetGabrovoSlots();

        return new JsonResult(slots);
    }

    [HttpGet]
    [Route(DoctorCalendarGetPlevenSlots)]
    public async Task<JsonResult> GetCalendarPlevenSlots()
    {
        var slots = await this.slotService.GetPlevenSlots();

        return new JsonResult(slots);
    }

    [HttpPut]
    [Route(DoctorCalendarEditSlot)]
    public async Task<IActionResult> EditSlot(SlotEditModel model)
    {
        var result = await this.slotService.EditSlot(model.Id, model.Status, model.Text);

        if (result.Failed)
        {
            return this.BadRequest(new { message = result.Error });
        }

        return this.Ok(model.Id);
    }
}