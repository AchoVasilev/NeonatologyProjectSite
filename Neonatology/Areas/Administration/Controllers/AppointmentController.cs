namespace Neonatology.Areas.Administration.Controllers;

using System.Threading.Tasks;

using AutoMapper;

using Services.AppointmentService;

using Microsoft.AspNetCore.Mvc;

using ViewModels.Administration.Appointment;

using static Common.GlobalConstants.MessageConstants;

public class AppointmentController : BaseController
{
    private readonly IAppointmentService appointmentService;
    private readonly IMapper mapper;

    public AppointmentController(IAppointmentService appointmentService, IMapper mapper)
    {
        this.appointmentService = appointmentService;
        this.mapper = mapper;
    }

    public async Task<IActionResult> All()
    {
        var model = await this.appointmentService.GetAllAppointments();

        return this.View(model);
    }

    public async Task<IActionResult> Information(int id)
    {
        var appointment = await this.appointmentService.GetAppointmentByIdAsync(id);

        if (appointment is null)
        {
            this.TempData["Message"] = AppointmentDoesntExistErrorMsg;
            return this.RedirectToAction(nameof(this.All));
        }

        var model = this.mapper.Map<AdminAppointmentViewModel>(appointment);

        return this.View(model);
    }

    public async Task<IActionResult> Delete(int id)
    {
        var result = await this.appointmentService.DeleteAppointment(id);
        if (result.Failed)
        {
            this.TempData["Message"] = result.Error;
            return this.RedirectToAction(nameof(this.All));
        }

        this.TempData["Message"] = SuccessfulDeleteMsg;

        return this.RedirectToAction(nameof(this.All));
    }
}