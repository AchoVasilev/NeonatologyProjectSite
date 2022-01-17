namespace Neonatology.Areas.Administration.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class HomeController : BaseController
    {
        public IActionResult Index()
            => View();
    }
}
