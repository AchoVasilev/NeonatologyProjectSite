namespace Neonatology.Services.FileService;

using System.Threading.Tasks;
using CloudinaryDotNet;
using Common;
using FileServiceModels;
using Microsoft.AspNetCore.Http;
using ViewModels.Gallery;

public interface IFileService : ITransientService
{
    Task<IFileServiceModel> UploadImage(Cloudinary cloudinary, IFormFile image, string folderName);

    Task<IFileServiceModel> UploadFile(Cloudinary cloudinary, IFormFile file, string folderName);

    Task DeleteFile(Cloudinary cloudinary, string name, string folderName);

    Task AddImageToDatabase(IFileServiceModel model);

    Task<GalleryViewModel> GetGalleryImagesAsync(int page, int itemsPerPage);
}