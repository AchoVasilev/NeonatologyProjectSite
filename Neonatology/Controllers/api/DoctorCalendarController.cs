namespace Neonatology.Controllers.api;

using System.Threading.Tasks;
using System;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using ViewModels.Slot;

using static Common.Constants.GlobalConstants;
using static Common.Constants.GlobalConstants.MessageConstants;
using Services.AppointmentService;
using Services.SlotService;

[ApiController]
[Authorize(Roles = DoctorConstants.DoctorRoleName)]
[Route("[controller]")]
public class DoctorCalendarController : ControllerBase
{
    private readonly IAppointmentService appointmentService;
    private readonly ISlotService slotService;

    public DoctorCalendarController(IAppointmentService appointmentService, ISlotService slotService)
    {
        this.appointmentService = appointmentService;
        this.slotService = slotService;
    }

    [HttpGet("gabrovo")]
    public async Task<JsonResult> GetDoctorGabrovoAppointments()
    {
        var result = await this.appointmentService.GetGabrovoAppointments();

        return new JsonResult(result);
    }

    [HttpGet("pleven")]
    public async Task<JsonResult> GetDoctorPlevenAppointments()
    {
        var result = await this.appointmentService.GetPlevenAppointments();

        return new JsonResult(result);
    }

    [HttpPost("generate")]
    public async Task<IActionResult> GenerateSlots([FromBody] SlotInputModel model)
    {
        var startDate = DateTime.Parse(model.StartDate);
        var endDate = DateTime.Parse(model.EndDate);

        if (startDate.Date < DateTime.Now.Date)
        {
            return this.BadRequest(new { response = DateBeforeNowErrorMsg });
        }

        if (startDate >= endDate)
        {
            return this.BadRequest(new { response = StartDateIsAfterEndDateMsg });
        }

        var result = await this.slotService
            .GenerateSlots(startDate, endDate, model.SlotDurationMinutes, model.AddressId);

        return new JsonResult(result);
    }

    [HttpGet("getSlots/gabrovo")]
    public async Task<JsonResult> GetCalendarGabrovoSlots()
    {
        var slots = await this.slotService.GetGabrovoSlots();

        return new JsonResult(slots);
    }

    [HttpGet("getSlots/pleven")]
    public async Task<JsonResult> GetCalendarPlevenSlots()
    {
        var slots = await this.slotService.GetPlevenSlots();

        return new JsonResult(slots);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> EditSlot(SlotEditModel model)
    {
        var isEdited = await this.slotService.EditSlot(model.Id, model.Status, model.Text);

        if (isEdited == false)
        {
            return this.BadRequest(new { message = FailedSlotEditMsg });
        }

        return this.Ok(model.Id);
    }
}