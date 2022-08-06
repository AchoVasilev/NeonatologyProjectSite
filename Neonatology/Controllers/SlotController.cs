namespace Neonatology.Controllers;

using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Services.SlotService;

using static Common.Constants.GlobalConstants;

public class SlotController : BaseController
{
    private readonly ISlotService slotService;

    public SlotController(ISlotService slotService)
    {
        this.slotService = slotService;
    }

    [Authorize(Roles = DoctorConstants.DoctorRoleName)]
    public async Task<IActionResult> TodaySlots()
    {
        var takenSlots = await this.slotService.GetTodaysTakenSlots();

        return this.View(takenSlots);
    }
}