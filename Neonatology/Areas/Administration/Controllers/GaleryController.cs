namespace Neonatology.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using Neonatology.Areas.Administration.Services;

    public class GaleryController : BaseController
    {
        private readonly IGaleryService galeryService;

        public GaleryController(IGaleryService galeryService)
        {
            this.galeryService = galeryService;
        }

        public async Task<IActionResult> All()
        {
            var model = await this.galeryService.GetGaleryImages();
            return View(model);
        }
    }
}
