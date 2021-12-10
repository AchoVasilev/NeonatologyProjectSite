namespace Neonatology.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using ViewModels.Appointments;

    [AllowAnonymous]
    public class AppointmentController : BaseController
    {
        public IActionResult MakeAnAppointment(string doctorId)
        {
            var viewModel = new AppointmentViewModel
            {
                DoctorId = doctorId
            };

            return View(viewModel);
        }
    }
}
