namespace Neonatology.Controllers
{
    using System.Threading.Tasks;

    using CloudinaryDotNet;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using Services.DoctorService;
    using Services.FileService;

    using ViewModels.Gallery;

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

        [Authorize(Roles = $"{DoctorConstants.DoctorRoleName}, {AdministratorRoleName}")]
        public IActionResult Add()
        {
            return View(new UploadImageModel());
        }

        [Authorize(Roles = $"{DoctorConstants.DoctorRoleName}, {AdministratorRoleName}")]
        [HttpPost]
        public async Task<IActionResult> Add(UploadImageModel model)
        {
            if (model.Images == null)
            {
                return View(new UploadImageModel());
            }

            foreach (var image in model.Images)
            {
                var result = await this.fileService.UploadImage(cloudinary, image, DefaultFolderName);

                if (result != null)
                {
                    await this.fileService.AddImageToDatabase(result);
                }
            }

            return RedirectToAction(nameof(All), "Gallery", new { area = "", page = 1 });
        }
    }
}
