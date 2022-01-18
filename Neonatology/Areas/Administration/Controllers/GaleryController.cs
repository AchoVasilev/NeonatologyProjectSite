namespace Neonatology.Areas.Administration.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using CloudinaryDotNet;

    using global::Services.FileService;
    using global::Services.FileService.FileServiceModels;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    using Neonatology.Areas.Administration.Services;
    using Neonatology.Areas.Administration.ViewModels.Galery;

    using static Common.GlobalConstants.Messages;
    using static Common.GlobalConstants.FileConstants;

    public class GaleryController : BaseController
    {
        private readonly IGaleryService galeryService;
        private readonly IFileService fileService;
        private readonly Cloudinary cloudinary;

        public GaleryController(IGaleryService galeryService, IFileService fileService, Cloudinary cloudinary)
        {
            this.galeryService = galeryService;
            this.fileService = fileService;
            this.cloudinary = cloudinary;
        }

        public async Task<IActionResult> All()
        {
            var model = new GaleryModel
            {
                GaleryImages = await this.galeryService.GetGaleryImages()
            };

            return View(model);
        }

        public async Task<IActionResult> Delete(string id)
        {
            var result = await this.galeryService.Delete(id);

            if (result == false)
            {
                this.TempData["Message"] = ErrorDeleting;
                return RedirectToAction(nameof(All));
            }

            this.TempData["Message"] = SuccessfulDelete;
            return RedirectToAction(nameof(All));
        }

        [HttpPost]
        public async Task<IActionResult> Add(ICollection<IFormFile> images)
        {
            var files = new List<IFileServiceModel>();

            foreach (var image in images)
            {
                var file = await this.fileService.UploadImage(cloudinary, image, DefaultFolderName);
                files.Add(file);
            }

            return RedirectToAction(nameof(All));
        }
    }
}