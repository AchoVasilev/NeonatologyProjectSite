namespace Neonatology.Controllers
{
    using System.Threading.Tasks;

    using CloudinaryDotNet;

    using Infrastructure;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using Services.DoctorService;
    using Services.FileService;

    using ViewModels.Galery;

    using static Common.GlobalConstants;
    using static Common.GlobalConstants.FileConstants;

    public class GalleryController : BaseController
    {
        private const int GaleryItemsPerPage = 8;
        private readonly IFileService fileService;
        private readonly IDoctorService doctorService;
        private readonly Cloudinary cloudinary;

        public GalleryController(IFileService fileService, IDoctorService doctorService, Cloudinary cloudinary)
        {
            this.fileService = fileService;
            this.doctorService = doctorService;
            this.cloudinary = cloudinary;
        }

        public async Task<IActionResult> All([FromQuery] int page)
        {
            var model = await this.fileService.GetGaleryImagesAsync(page, GaleryItemsPerPage);

            return View(model);
        }

        [Authorize(Roles = DoctorConstants.DoctorRoleName)]
        [Authorize(Roles = AdministratorRoleName)]
        [HttpPost]
        public async Task<IActionResult> All(UploadImageModel model)
        {
            var userId = this.User.GetId();
            var doctorId = await this.doctorService.GetDoctorIdByUserId(userId);

            if (string.IsNullOrWhiteSpace(doctorId) == true)
            {
                return Unauthorized();
            }

            foreach (var image in model.Images)
            {
                var result = await this.fileService.UploadImage(cloudinary, image, DefaultFolderName);

                if (result != null)
                {
                    await this.fileService.AddImageToDatabase(result);
                }
            }

            return RedirectToAction(nameof(All), "Galery", new { area = "" });
        }
    }
}
