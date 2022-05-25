namespace Services.FileService
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using CloudinaryDotNet;
    using CloudinaryDotNet.Actions;

    using Data;

    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;

    using Services.FileService.FileServiceModels;

    using ViewModels.Gallery;

    public class FileService : IFileService
    {
        private readonly NeonatologyDbContext data;

        public FileService(NeonatologyDbContext data)
        {
            this.data = data;
        }

        public async Task<IFileServiceModel> UploadImage(Cloudinary cloudinary, IFormFile image, string folderName)
        {
            if (image == null)
            {
                return null;
            }

            var AllowedExtensions = new[] { "jpg", "png", "gif", "jpeg" };

            var extension = Path.GetExtension(image.FileName).TrimStart('.');

            if (!AllowedExtensions.Any(x => extension.EndsWith(x)))
            {
                throw new Exception($"Invalid image extension {extension}");
            }

            string imageName = image.FileName;

            byte[] destinationImage;
            using (var memoryStream = new MemoryStream())
            {
                await image.CopyToAsync(memoryStream);
                destinationImage = memoryStream.ToArray();
            }

            var imageModel = new ImageServiceModel();
            using (var ms = new MemoryStream(destinationImage))
            {
                // Cloudinary doesn't work with [?, &, #, \, %, <, >]
                imageName = imageName.Replace("&", "And");
                imageName = imageName.Replace("#", "sharp");
                imageName = imageName.Replace("?", "questionMark");
                imageName = imageName.Replace("\\", "right");
                imageName = imageName.Replace("%", "percent");
                imageName = imageName.Replace(">", "greater");
                imageName = imageName.Replace("<", "lower");

                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(imageName, ms),
                    PublicId = $"{folderName}/{imageName}",
                };

                var uploadResult = cloudinary.Upload(uploadParams);

                imageModel.Extension = extension;
                imageModel.Uri = uploadResult.SecureUrl.AbsoluteUri;
                imageModel.Name = imageName;
            }

            return imageModel;
        }

        public async Task<IFileServiceModel> UploadFile(Cloudinary cloudinary, IFormFile file, string folderName)
        {
            if (file == null)
            {
                return null;
            }

            var AllowedExtensions = new[] { "txt", "text", "docx", "doc", "pdf", "ppt", "xls", "xlsx", "zip", "rar" };

            var extension = Path.GetExtension(file.FileName).TrimStart('.');

            if (!AllowedExtensions.Any(x => extension.EndsWith(x)))
            {
                throw new Exception($"Invalid file extension {extension}");
            }

            string fileName = file.FileName;

            byte[] destinationImage;
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                destinationImage = memoryStream.ToArray();
            }

            var fileModel = new FileServiceModel();
            using (var ms = new MemoryStream(destinationImage))
            {
                // Cloudinary doesn't work with [?, &, #, \, %, <, >]
                fileName = fileName.Replace("&", "And");
                fileName = fileName.Replace("#", "sharp");
                fileName = fileName.Replace("?", "questionMark");
                fileName = fileName.Replace("\\", "right");
                fileName = fileName.Replace("%", "percent");
                fileName = fileName.Replace(">", "greater");
                fileName = fileName.Replace("<", "lower");

                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(fileName, ms),
                    PublicId = $"{folderName}/{fileName}",
                };

                var uploadResult = cloudinary.Upload(uploadParams, "auto");

                fileModel.Extension = extension;
                fileModel.Uri = uploadResult.SecureUrl.AbsoluteUri;
                fileModel.Name = fileName;
            }

            return fileModel;
        }

        public async Task AddImageToDatabase(IFileServiceModel model)
        {
            var image = new Data.Models.Image
            {
                Extension = model.Extension,
                Url = model.Uri
            };

            await data.Images.AddAsync(image);
            await data.SaveChangesAsync();
        }

        public async Task<GalleryViewModel> GetGaleryImagesAsync(int page, int itemsPerPage)
        {
            var images = await this.data.Images
                .Where(x => string.IsNullOrWhiteSpace(x.UserId) &&
                x.IsDeleted == false && x.Name != "NoAvatarProfileImage.png")
                .OrderBy(x => x.CreatedOn)
                .Skip((page - 1) * itemsPerPage)
                .Take(itemsPerPage)
                .ToListAsync();

            var count = await this.data.Images
                .Where(x => string.IsNullOrWhiteSpace(x.UserId) && x.IsDeleted == false && x.Name != "NoAvatarProfileImage.png")
                .CountAsync();

            var viewModel = new GalleryViewModel
            {
                ItemCount = count,
                ItemsPerPage = itemsPerPage,
                PageNumber = page
            };

            foreach (var url in images)
            {
                viewModel.ImageUrls.Add(url.Url);
            }

            return viewModel;
        }

        public async Task DeleteFile(Cloudinary cloudinary, string name, string folderName)
        {
            var delParams = new DeletionParams($"{folderName}/{name}");

            await cloudinary.DestroyAsync(delParams);
        }
    }
}
