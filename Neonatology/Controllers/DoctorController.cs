namespace Neonatology.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using Services.DoctorService;

    using static Common.GlobalConstants;

    public class DoctorController : BaseController
    {
        private readonly IDoctorService doctorService;

        public DoctorController(IDoctorService doctorService)
        {
            this.doctorService = doctorService;
        }

        public async Task<IActionResult> Profile()
        {
            var viewModel = await this.doctorService.DoctoryById(DoctorId);

            return View(viewModel);
        }
    }
}
