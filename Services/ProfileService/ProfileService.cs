namespace Services.ProfileService
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    using CloudinaryDotNet;

    using Data;
    using Data.Models;

    using Microsoft.EntityFrameworkCore;

    using Services.FileService;

    using ViewModels.Profile;

    using static Common.GlobalConstants.FileConstants;

    public class ProfileService : IProfileService
    {
        private readonly NeonatologyDbContext data;
        private readonly IFileService fileService;
        private readonly IMapper mapper;
        private readonly Cloudinary cloudinary;

        public ProfileService(NeonatologyDbContext data, IFileService fileService, IMapper mapper, Cloudinary cloudinary)
        {
            this.data = data;
            this.fileService = fileService;
            this.mapper = mapper;
            this.cloudinary = cloudinary;
        }

        public async Task<ProfileViewModel> GetPatientData(string userId)
        {
            var patientData = await this.data.Patients
                 .Where(x => x.UserId == userId && x.IsDeleted == false)
                 .ProjectTo<ProfileViewModel>(this.mapper.ConfigurationProvider)
                 .FirstOrDefaultAsync();

            return patientData;
        }

        public async Task<bool> EditProfileAsync(EditProfileFormModel model)
        {
            var patient = await this.data.Users.FindAsync(model.Id);

            if (patient == null)
            {
                return false;
            }

            var result = await this.fileService.UploadImage(cloudinary, model.Picture, ProfileFolderName);
            if (result == null)
            {
                return false;
            }

            patient.Patient.FirstName = model.FirstName;
            patient.Patient.LastName = model.LastName;
            patient.Patient.Phone = model.PhoneNumber;
            patient.Patient.CityId = model.CityId;
            patient.Patient.ModifiedOn = DateTime.UtcNow;

            patient.Patient.Image.IsDeleted = true;
            patient.Patient.Image.DeletedOn = DateTime.UtcNow;

            patient.Patient.Image = new Image
            {
                Extension = result.Extension,
                Url = result.Uri,
            };

            patient.Email = model.Email;
            patient.PhoneNumber = model.PhoneNumber;

            await this.data.SaveChangesAsync();

            return true;
        }
    }
}
