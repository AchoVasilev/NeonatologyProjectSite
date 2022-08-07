namespace Services.FileService;

using System.IO;
using System.Threading.Tasks;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using FileServiceModels;
using Microsoft.AspNetCore.Http;

public class FileProcessingService : IFileProcessingService
{
    public async Task<byte[]> GenerateByteArrayFromFile(IFormFile file)
    {
        byte[] destinationImage;

        using (var ms = new MemoryStream())
        {
            await file.CopyToAsync(ms);
            destinationImage = ms.ToArray();
        }

        return destinationImage;
    }

    public async Task<IFileServiceModel> ProcessByteArray(byte[] array, Cloudinary cloudinary,
        IFileServiceModel imageModel, string imageName, string folderName, string extension)
    {
        using (var ms = new MemoryStream(array))
        {
            // Cloudinary doesn't work with [?, &, #, \, %, <, >]
            imageName = imageName.Replace("&", "And")
                .Replace("#", "sharp")
                .Replace("?", "questionMark")
                .Replace("\\", "right")
                .Replace("%", "percent")
                .Replace(">", "greater")
                .Replace("<", "lower");

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(imageName, ms),
                PublicId = $"{folderName}/{imageName}",
            };

            var uploadResult = await cloudinary.UploadAsync(uploadParams);

            imageModel.Extension = extension;
            imageModel.Uri = uploadResult.SecureUrl.AbsoluteUri;
            imageModel.Name = imageName;
        }

        return imageModel;
    }
}