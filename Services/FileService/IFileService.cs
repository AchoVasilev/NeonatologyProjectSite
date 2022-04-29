namespace Services.FileService
{
    using System.Threading.Tasks;

    using CloudinaryDotNet;

    using Microsoft.AspNetCore.Http;

    using Services.FileService.FileServiceModels;

    using ViewModels.Galery;

    public interface IFileService
    {
        Task<IFileServiceModel> UploadImage(Cloudinary cloudinary, IFormFile image, string folderName);

        Task<IFileServiceModel> UploadFile(Cloudinary cloudinary, IFormFile file, string folderName);

        Task DeleteFile(Cloudinary cloudinary, string name, string folderName);

        Task AddImageToDatabase(IFileServiceModel model);

        Task<GalleryViewModel> GetGaleryImagesAsync(int page, int itemsPerPage);
    }
}
