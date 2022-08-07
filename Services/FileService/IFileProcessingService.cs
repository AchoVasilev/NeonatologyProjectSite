namespace Services.FileService;

using System.Threading.Tasks;
using CloudinaryDotNet;
using Common;
using FileServiceModels;
using Microsoft.AspNetCore.Http;

public interface IFileProcessingService : ITransientService
{
    Task<byte[]> GenerateByteArrayFromFile(IFormFile file);

    Task<IFileServiceModel> ProcessByteArray(byte[] array, Cloudinary cloudinary,
        IFileServiceModel imageModel, string imageName, string folderName, string extension);
}