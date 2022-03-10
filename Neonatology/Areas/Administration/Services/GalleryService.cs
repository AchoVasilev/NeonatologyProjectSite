namespace Neonatology.Areas.Administration.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    using CloudinaryDotNet;

    using Data;

    using global::Services.FileService;

    using Microsoft.EntityFrameworkCore;

    using ViewModels.Administration.Galery;
    using static Common.GlobalConstants.FileConstants;

    public class GalleryService : IGalleryService
    {
        private readonly NeonatologyDbContext data;
        private readonly IMapper mapper;
        private readonly IFileService fileService;
        private readonly Cloudinary cloudinary;

        public GalleryService(NeonatologyDbContext data, IMapper mapper, IFileService fileService, Cloudinary cloudinary)
        {
            this.data = data;
            this.mapper = mapper;
            this.fileService = fileService;
            this.cloudinary = cloudinary;
        }

        public async Task<ICollection<GaleryViewModel>> GetGaleryImages()
            => await this.data.Images
                .Where(x => string.IsNullOrWhiteSpace(x.UserId) && x.IsDeleted == false)
                .OrderByDescending(x => x.CreatedOn)
                .ProjectTo<GaleryViewModel>(this.mapper.ConfigurationProvider)
                .ToListAsync();

        public async Task<bool> Delete(string id)
        {
            var image = await this.data.Images
                .Where(x => x.Id == id && x.IsDeleted == false)
                .FirstOrDefaultAsync();

            if (image == null)
            {
                return false;
            }

            image.IsDeleted = true;
            image.DeletedOn = DateTime.UtcNow;
            await this.fileService.DeleteFile(this.cloudinary, image.Name, DefaultFolderName);

            await this.data.SaveChangesAsync();

            return true;
        }
    }
}
