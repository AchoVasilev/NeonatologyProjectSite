namespace Neonatology.Controllers.api
{
using System.Threading.Tasks;
using System;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using ViewModels.Slot;

    using static Common.GlobalConstants;
    using static Common.GlobalConstants.MessageConstants;
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
            var result = await this.appointmentService.GetPlevenAppointments();

            foreach (var res in result)
            {
                res.DateTime = res.DateTime.ToLocalTime();
                res.End = res.DateTime.ToLocalTime();
            }

            return new JsonResult(result);
        }

        [HttpGet("pleven")]
        public async Task<JsonResult> GetDoctorPlevenAppointments()
        {
            var result = await this.appointmentService.GetPlevenAppointments();

            foreach (var res in result)
            {
                res.DateTime = res.DateTime.ToLocalTime();
                res.End = res.DateTime.ToLocalTime();
            }

            return new JsonResult(result);
        }

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

            var result = await this.slotService
                .GenerateSlots(model.Start.ToLocalTime(), model.End.ToLocalTime(), model.SlotDurationMinutes, model.AddressId);

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
            var isEdited = await this.slotService.EditSlot(model.Id, model.Status);

            if (isEdited == false)
            {
                return BadRequest(new { message = FailedSlotEditMsg });
            }

            return Ok(model.Id);
        }
    }
}
