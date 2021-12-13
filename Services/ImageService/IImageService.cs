namespace Services.ImageService
{
    using System.Threading.Tasks;

    using CloudinaryDotNet;

    using Data.Models;

    using Microsoft.AspNetCore.Http;
    public interface IImageService
    {
        Task UploadImage(Cloudinary cloudinary, IFormFile image, Doctor doctor);
    }
}
