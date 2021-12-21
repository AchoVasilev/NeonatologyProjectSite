namespace Neonatology.Controllers
{
    using System.Threading.Tasks;

    using CloudinaryDotNet;

    using Data.Models;

    using Infrastructure;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    using Services.DoctorService;
    using Services.ImageService;

    using ViewModels.Galery;

    public class GaleryController : Controller
    {
        private readonly IImageService imageService;
        private readonly Cloudinary cloudinary;
        private readonly IDoctorService doctorService;

        public GaleryController(IImageService imageService, Cloudinary cloudinary, IDoctorService doctorService)
        {
            this.imageService = imageService;
            this.cloudinary = cloudinary;
            this.doctorService = doctorService;
        }

        public async Task<IActionResult> All()
        {
            var model = await this.imageService.GetGaleryImagesAsync();

            return View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> All(UploadImageModel model)
        {
            var userId = this.User.GetId();
            var doctorId = await this.doctorService.GetDoctorIdByUserId(userId);

            if (string.IsNullOrWhiteSpace(doctorId) == true)
            {
                return Unauthorized();
            }

            await this.imageService.UploadImages(cloudinary, model.Images);

            return RedirectToAction(nameof(All), "Galery", new { area = "" });
        }
    }
}
