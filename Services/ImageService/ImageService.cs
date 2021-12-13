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

    public class ImageService : IImageService
    {
        //public async Task UploadImages(Cloudinary cloudinary, IEnumerable<IFormFile> images, int companyId, Car car)
        //{
        //    var AllowedExtensions = new[] { "jpg", "png", "gif", "jpeg" };

        //    foreach (var image in images)
        //    {
        //        var extension = Path.GetExtension(image.FileName).TrimStart('.');

        //        if (!AllowedExtensions.Any(x => extension.EndsWith(x)))
        //        {
        //            throw new Exception($"Invalid image extension {extension}");
        //        }

        //        string imageName = image.FileName;

        //        byte[] destinationImage;
        //        using (var memoryStream = new MemoryStream())
        //        {
        //            await image.CopyToAsync(memoryStream);
        //            destinationImage = memoryStream.ToArray();
        //        }

        //        using (var ms = new MemoryStream(destinationImage))
        //        {
        //            // Cloudinary doesn't work with &
        //            imageName = imageName.Replace("&", "And");

        //            var uploadParams = new ImageUploadParams()
        //            {
        //                File = new FileDescription(imageName, ms),
        //                PublicId = imageName,
        //            };

        //            var uploadResult = cloudinary.Upload(uploadParams);

        //            var dbImage = new Image()
        //            {
        //                CompanyId = companyId,
        //                Extension = extension,
        //                Url = uploadResult.SecureUrl.AbsoluteUri
        //            };

        //            car.CarImages.Add(dbImage);
        //        }
        //    }
        //}

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
    }
}
