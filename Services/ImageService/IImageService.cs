namespace Services.ImageService
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using CloudinaryDotNet;

    using Data.Models;

    using Microsoft.AspNetCore.Http;

    using ViewModels.Galery;

    public interface IImageService
    {
        Task UploadImages(Cloudinary cloudinary, IEnumerable<IFormFile> images);

        Task UploadImage(Cloudinary cloudinary, IFormFile image, Doctor doctor);

        Task<UploadImageModel> GetGaleryImagesAsync();
    }
}
