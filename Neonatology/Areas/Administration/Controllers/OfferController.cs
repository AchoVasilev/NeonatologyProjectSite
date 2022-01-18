namespace Neonatology.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using global::Services.OfferService;

    using Microsoft.AspNetCore.Mvc;

    using ViewModels.Administration.Offer;

    using static Common.GlobalConstants.Messages;

    public class OfferController : BaseController
    {
        private readonly IOfferService offerService;

        public OfferController(IOfferService offerService)
        {
            this.offerService = offerService;
        }

        public async Task<IActionResult> All()
        {
            var services = await this.offerService.GetAllAsync();

            return View(services);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var isDeleted = await this.offerService.DeleteOffer(id);
            if (isDeleted == false)
            {
                this.TempData["Message"] = ErrorDeletingMsg;
                return RedirectToAction(nameof(All));
            }

            this.TempData["Message"] = SuccessfulDeleteMsg;

            return RedirectToAction(nameof(All));
        }

        public IActionResult Add() 
            => View(new CreateOfferFormModel());

        [HttpPost]
        public async Task<IActionResult> Add(CreateOfferFormModel model)
        {
            await this.offerService.AddOffer(model);

            this.TempData["Message"] = SuccessfulAddedItemMsg;

            return RedirectToAction(nameof(All));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var model = await this.offerService.GetOffer(id);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditOfferFormModel model)
        {
            var isEdited = await this.offerService.EditOffer(model);
            if (isEdited == false)
            {
                this.TempData["Message"] = ErrorDeletingMsg;
                return RedirectToAction(nameof(All));
            }

            this.TempData["Message"] = SuccessfulEditMsg;

            return RedirectToAction(nameof(All));
        }
    }
}
