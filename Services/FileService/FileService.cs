namespace Services.FileService;

using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using FileServiceModels;
using ViewModels.Gallery;

public class FileService : IFileService
{
    private readonly NeonatologyDbContext data;
    private readonly IFileProcessingService fileProcessingService;
    public FileService(NeonatologyDbContext data, IFileProcessingService fileProcessingService)
    {
        this.data = data;
        this.fileProcessingService = fileProcessingService;
    }

    public async Task<IFileServiceModel> UploadImage(Cloudinary cloudinary, IFormFile image, string folderName)
    {
        if (image is null)
        {
            return null;
        }

        var allowedExtensions = new[] { "jpg", "png", "gif", "jpeg" };

        var extension = Path.GetExtension(image.FileName).TrimStart('.');

        if (!allowedExtensions.Any(x => extension.EndsWith(x)))
        {
            throw new Exception($"Invalid image extension {extension}");
        }

        var imageName = image.FileName;

        var destinationImage = await this.fileProcessingService.GenerateByteArrayFromFile(image);

        var imageModel = new ImageServiceModel();
        imageModel = await this.fileProcessingService.ProcessByteArray(destinationImage, cloudinary, imageModel, imageName,
            folderName, extension) as ImageServiceModel;

        return imageModel;
    }

    public async Task<IFileServiceModel> UploadFile(Cloudinary cloudinary, IFormFile file, string folderName)
    {
        if (file is null)
        {
            return null;
        }

        var allowedExtensions = new[] { "txt", "text", "docx", "doc", "pdf", "ppt", "xls", "xlsx", "zip", "rar" };

        var extension = Path.GetExtension(file.FileName).TrimStart('.');

        if (!allowedExtensions.Any(x => extension.EndsWith(x)))
        {
            throw new Exception($"Invalid file extension {extension}");
        }

        var fileName = file.FileName;

        var destinationImage = await this.fileProcessingService.GenerateByteArrayFromFile(file);

        var fileModel = new FileServiceModel();
        fileModel = await this.fileProcessingService.ProcessByteArray(destinationImage, cloudinary, fileModel, fileName,
            folderName, extension) as FileServiceModel;

        return fileModel;
    }

    public async Task AddImageToDatabase(IFileServiceModel model)
    {
        var image = new Data.Models.Image
        {
            Extension = model.Extension,
            Url = model.Uri
        };

        await this.data.Images.AddAsync(image);
        await this.data.SaveChangesAsync();
    }

    public async Task<GalleryViewModel> GetGalleryImagesAsync(int page, int itemsPerPage)
    {
        var images = await this.data.Images
            .Where(x => string.IsNullOrWhiteSpace(x.UserId) &&
                        x.IsDeleted == false && x.Name != "NoAvatarProfileImage.png")
            .OrderBy(x => x.CreatedOn)
            .Skip((page - 1) * itemsPerPage)
            .Take(itemsPerPage)
            .AsNoTracking()
            .ToListAsync();

        var count = await this.data.Images
            .Where(x => string.IsNullOrWhiteSpace(x.UserId) && x.IsDeleted == false &&
                        x.Name != "NoAvatarProfileImage.png")
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