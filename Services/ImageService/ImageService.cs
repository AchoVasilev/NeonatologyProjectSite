namespace Services.ImageService
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using CloudinaryDotNet.Actions;
    using CloudinaryDotNet;
    using Microsoft.AspNetCore.Http;
    using Data.Models;
    using System.Collections.Generic;
    using Data;
    using ViewModels.Galery;
    using Microsoft.EntityFrameworkCore;

    public class ImageService : IImageService
    {
        private readonly NeonatologyDbContext data;

        public ImageService(NeonatologyDbContext data)
        {
            this.data = data;
        }

        public async Task UploadImages(Cloudinary cloudinary, IEnumerable<IFormFile> images)
        {
            var AllowedExtensions = new[] { "jpg", "png", "gif", "jpeg" };

            foreach (var image in images)
            {
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

                using (var ms = new MemoryStream(destinationImage))
                {
                    // Cloudinary doesn't work with &
                    imageName = imageName.Replace("&", "And");

                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(imageName, ms),
                        PublicId = $"pediamed/{imageName}",
                    };

                    var uploadResult = cloudinary.Upload(uploadParams);

                    var dbImage = new Image()
                    {
                        Extension = extension,
                        Url = uploadResult.SecureUrl.AbsoluteUri
                    };

                    await this.data.Images.AddAsync(dbImage);
                    await this.data.SaveChangesAsync();
                }
            }
        }

        public async Task UploadImage(Cloudinary cloudinary, IFormFile image, Doctor doctor)
        {
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

            using (var ms = new MemoryStream(destinationImage))
            {
                // Cloudinary doesn't work with &
                imageName = imageName.Replace("&", "And");

                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(imageName, ms),
                    PublicId = imageName,
                };

                var uploadResult = cloudinary.Upload(uploadParams);

                var dbImage = new Image()
                {
                    Extension = extension,
                    Url = uploadResult.SecureUrl.AbsoluteUri
                };

                doctor.Image = dbImage;
            }
        }

        public async Task<UploadImageModel> GetGaleryImagesAsync()
        {
            var images = await this.data.Images
                .Where(x => string.IsNullOrWhiteSpace(x.DoctorId) && x.IsDeleted == false)
                .ToListAsync();

            var model = new UploadImageModel();

            foreach (var image in images)
            {
                model.Urls.Add(image.Url);  
            }

            return model;
        }
    }
}
