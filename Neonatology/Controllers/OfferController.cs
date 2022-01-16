namespace Neonatology.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using Services.OfferService;

    public class OfferController : BaseController
    {
        private readonly IOfferService offerService;

        public OfferController(IOfferService offerService) 
            => this.offerService = offerService;

        public async Task<IActionResult> All()
        {
            var services = await this.offerService.GetAllAsync();

            return View(services);
        }
    }
}
