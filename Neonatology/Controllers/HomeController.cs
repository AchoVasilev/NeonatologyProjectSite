namespace Neonatology.Controllers
{
    using System.Diagnostics;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    using Services.DoctorService;

    using ViewModels.ErrorViewModel;
    using ViewModels.Home;

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;
        private readonly IDoctorService doctorService;

        public HomeController(ILogger<HomeController> logger, IDoctorService doctorService)
        {
            this.logger = logger;
            this.doctorService = doctorService;
        }

        public async Task<IActionResult> Index()
        {
            var model = new HomeViewModel()
            {
                DoctorId = await this.doctorService.GetDoctorId()
            };

            return View(model);
        }

        [Route("/Home/Error/404")]
        public IActionResult Error404()
        {
            return View();
        }

        [Route("/Home/Error/400")]
        public IActionResult Error400()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
