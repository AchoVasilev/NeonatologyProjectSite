namespace Neonatology.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using global::Services.RatingService;

    using Microsoft.AspNetCore.Mvc;

    using static Common.GlobalConstants.MessageConstants;

    public class RatingController : BaseController
    {
        private readonly IRatingService ratingService;

        public RatingController(IRatingService ratingService)
        {
            this.ratingService = ratingService;
        }

        public async Task<IActionResult> All()
        {
            var result = await this.ratingService.GetRatings();

            return this.View(result);
        }

        public async Task<IActionResult> Approve(int id)
        {
            var isApproved = await this.ratingService.ApproveRating(id);
            if (isApproved == false)
            {
                this.TempData["Message"] = ErrorApprovingRating;
                return this.RedirectToAction(nameof(this.All));
            }

            this.TempData["Message"] = SuccessfullyApprovedRating;
            return this.RedirectToAction(nameof(this.All));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var isDeleted = await this.ratingService.DeleteRating(id);
            if (isDeleted == false)
            {
                this.TempData["Message"] = ErrorDeletingMsg;
                return this.RedirectToAction(nameof(this.All), "Rating", new { area = "Administration" });
            }

            this.TempData["Message"] = SuccessfullyDeletedRating;
            return this.RedirectToAction(nameof(this.All), "Rating", new {area = "Administration"});
        }
    }
}
