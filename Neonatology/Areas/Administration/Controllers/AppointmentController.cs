﻿namespace Neonatology.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using AutoMapper;

    using global::Services.AppointmentService;

    using Microsoft.AspNetCore.Mvc;

    using ViewModels.Administration.Appointment;

    using static Common.GlobalConstants.Messages;

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

            return View(model);
        }

        public async Task<IActionResult> Information(int id)
        {
            var appointment = await this.appointmentService.GetAppointmentByIdAsync(id);

            if (appointment == null)
            {
                this.TempData["Message"] = AppointmentDoesntExistErrorMsg;
                return RedirectToAction(nameof(All));
            }

            var model = this.mapper.Map<AdminAppointmentViewModel>(appointment);

            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var result = await this.appointmentService.DeleteAppointment(id);
            if (result == false)
            {
                this.TempData["Message"] = ErrorDeletingMsg;
                return RedirectToAction(nameof(All));
            }

            this.TempData["Message"] = SuccessfulDeleteMsg;

            return RedirectToAction(nameof(All));
        }
    }
}
