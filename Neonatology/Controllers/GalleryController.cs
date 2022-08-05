namespace Neonatology.Controllers
{
    using System.Threading.Tasks;

    using CloudinaryDotNet;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Services.FileService;

    using ViewModels.Gallery;

    using static Common.GlobalConstants;
    using static Common.GlobalConstants.FileConstants;

    public class GalleryController : BaseController
    {
        private const int GalleryItemsPerPage = 8;
        private readonly IFileService fileService;
        private readonly Cloudinary cloudinary;

        public GalleryController(IFileService fileService, Cloudinary cloudinary)
        {
            this.fileService = fileService;
            this.cloudinary = cloudinary;
        }

        public async Task<IActionResult> All([FromQuery] int page)
        {
            var model = await this.fileService.GetGalleryImagesAsync(page, GalleryItemsPerPage);

            return this.View(model);
        }

        [Authorize(Roles = $"{DoctorConstants.DoctorRoleName}, {AdministratorRoleName}")]
        public IActionResult Add()
        {
            return this.View(new UploadImageModel());
        }

        [Authorize(Roles = $"{DoctorConstants.DoctorRoleName}, {AdministratorRoleName}")]
        [HttpPost]
        public async Task<IActionResult> Add(UploadImageModel model)
        {
            if (model.Images == null)
            {
                return this.View(new UploadImageModel());
            }

            foreach (var image in model.Images)
            {
                var result = await this.fileService.UploadImage(this.cloudinary, image, DefaultFolderName);

                if (result != null)
                {
                    await this.fileService.AddImageToDatabase(result);
                }
            }

            return this.RedirectToAction(nameof(this.All), "Gallery", new { area = "", page = 1 });
        }
    }
}
