namespace Neonatology.Controllers.api
{
using System.Threading.Tasks;
using System;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using ViewModels.Slot;

    using static Common.GlobalConstants;
    using static Common.GlobalConstants.Messages;
    using Services.AppointmentService;
    using Services.SlotService;

    [ApiController]
    [Authorize(Roles = DoctorRoleName)]
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

        [HttpGet("getSlots")]
        public async Task<JsonResult> GetCalendarSlots()
        {
            var slots = await this.slotService.GetSlots();

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
