namespace Neonatology.Controllers
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using Services.AppointmentService;
    using Services.DateTimeParser;

    using ViewModels.Appointments;

    using static Common.GlobalConstants;
    using static Common.GlobalConstants.Messages;

    [AllowAnonymous]
    public class AppointmentController : BaseController
    {
        private readonly IDateTimeParserService dateTimeParserService;
        private readonly IAppointmentService appointmentService;

        public AppointmentController(IDateTimeParserService dateTimeParserService, IAppointmentService appointmentService)
        {
            this.dateTimeParserService = dateTimeParserService;
            this.appointmentService = appointmentService;
        }

        public IActionResult MakeAnAppointment(string doctorId = DoctorId)
        {
            var viewModel = new AppointmentViewModel
            {
                DoctorId = doctorId
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> MakeAnAppointment(AppointmentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            DateTime dateTime;

            try
            {
                dateTime = this.dateTimeParserService.ConvertStrings(model.Date, model.Time);
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(MakeAnAppointment), new { model.DoctorId });
            }

            var result = await this.appointmentService.AddAsync(model.DoctorId, model, dateTime);

            if (result == false)
            {
                return RedirectToAction("MakeAnAppointment", new { model.DoctorId });
            }

            if (result == true)
            {
                this.TempData["Message"] = string.Format(SuccessfullAppointment, model.Date, model.Time);
            }

            return RedirectToAction("Index", "Home", new { area = "" });
        }
    }
}
