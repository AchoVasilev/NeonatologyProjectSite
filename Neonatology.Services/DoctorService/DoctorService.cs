namespace Neonatology.Services.DoctorService;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CloudinaryDotNet;
using Data;
using Data.Models;
using FileService;
using Microsoft.EntityFrameworkCore;
using Neonatology.Common.Models;
using ViewModels.Address;
using ViewModels.Doctor;
using static Neonatology.Common.Constants.GlobalConstants.FileConstants;
using static Neonatology.Common.Constants.GlobalConstants.MessageConstants;

public class DoctorService : IDoctorService
{
    private readonly NeonatologyDbContext data;
    private readonly IFileService fileService;
    private readonly IMapper mapper;
    private readonly Cloudinary cloudinary;

    public DoctorService(NeonatologyDbContext data, IMapper mapper, IFileService fileService, Cloudinary cloudinary)
    {
        this.data = data;
        this.mapper = mapper;
        this.fileService = fileService;
        this.cloudinary = cloudinary;
    }

    public async Task<DoctorProfileViewModel> GetDoctorById(string doctorId)
        => await this.data.Doctors
            .Where(x => x.Id == doctorId)
            .ProjectTo<DoctorProfileViewModel>(this.mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();

    public async Task<DoctorProfileViewModel> GetDoctorByUserId(string userId)
        => await this.data.Doctors
            .Where(x => x.UserId == userId)
            .AsNoTracking()
            .ProjectTo<DoctorProfileViewModel>(this.mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();

    public async Task<string> GetDoctorIdByUserId(string userId)
        => await this.data.Users
            .Where(x => x.Id == userId)
            .Select(x => x.DoctorId)
            .FirstOrDefaultAsync();

    public async Task<string> GetDoctorIdByAppointmentId(int appointmentId)
        => await this.data.Appointments
            .Where(x => x.Id == appointmentId && x.IsDeleted == false)
            .Select(x => x.DoctorId)
            .FirstOrDefaultAsync();

    public async Task<string> GetDoctorId()
        => await this.data.Doctors
            .Select(x => x.Id)
            .FirstOrDefaultAsync();

    public async Task<string> GetDoctorEmail(string doctorId)
        => await this.data.Doctors
            .Where(x => x.Id == doctorId)
            .Select(x => x.Email)
            .FirstOrDefaultAsync();

    public async Task<ICollection<EditAddressFormModel>> GetDoctorAddressesById(string doctorId)
        => await this.data.Addresses
            .Where(x => x.DoctorId == doctorId && x.IsDeleted == false)
            .ProjectTo<EditAddressFormModel>(this.mapper.ConfigurationProvider)
            .ToListAsync();

    public async Task<OperationResult> EditDoctorAsync(DoctorEditModel model)
    {
        var doctor = await this.data.Doctors.FirstOrDefaultAsync(x => x.Id == model.Id);
        if (doctor is null)
        {
            return UnauthorizedErrorMsg;
        }

        if (model.Picture != null)
        {
            var result = await this.fileService.UploadImage(this.cloudinary, model.Picture, DefaultFolderName);
            if (result is null)
            {
                return UploadFailErrorMsg;
            }

            doctor.User.Image.IsDeleted = true;
            doctor.User.Image.DeletedOn = DateTime.UtcNow;

            doctor.User.Image = new Image
            {
                Extension = result.Extension,
                Url = result.Uri,
                RemoteImageUrl = model.UserImageUrl
            };
        }

        doctor.FirstName = model.FirstName;
        doctor.LastName = model.LastName;
        doctor.PhoneNumber = model.PhoneNumber;
        doctor.YearsOfExperience = model.YearsOfExperience;
        doctor.Age = model.Age;
        doctor.Biography = model.Biography;
            
        doctor.ModifiedOn = DateTime.UtcNow;
            
        await this.data.SaveChangesAsync();

        return true;
    }
}