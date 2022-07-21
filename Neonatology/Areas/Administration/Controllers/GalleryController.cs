namespace Neonatology.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using CloudinaryDotNet;

    using global::Services.FileService;
    using Microsoft.AspNetCore.Mvc;

    using Services;
    using ViewModels.Administration.Galery;

    using static Common.GlobalConstants.MessageConstants;
    using static Common.GlobalConstants.FileConstants;
    using ViewModels.Gallery;

    public class GalleryController : BaseController
    {
        private readonly IGalleryService galleryService;
        private readonly IFileService fileService;
        private readonly Cloudinary cloudinary;

        public GalleryController(IGalleryService galleryService, IFileService fileService, Cloudinary cloudinary)
        {
            this.galleryService = galleryService;
            this.fileService = fileService;
            this.cloudinary = cloudinary;
        }

        public async Task<IActionResult> All()
        {
            var model = new GaleryModel
            {
                GaleryImages = await this.galleryService.GetGaleryImages()
            };

            return this.View(model);
        }

        public async Task<IActionResult> Delete(string id)
        {
            var result = await this.galleryService.Delete(id);

            if (result == false)
            {
                this.TempData["Message"] = ErrorDeletingMsg;
                return this.RedirectToAction(nameof(this.All));
            }

            this.TempData["Message"] = SuccessfulDeleteMsg;
            return this.RedirectToAction(nameof(this.All));
        }

        public IActionResult Add()
        {
            return this.View(new UploadImageModel());
        }

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

            return this.RedirectToAction(nameof(this.All));
        }
    }
}