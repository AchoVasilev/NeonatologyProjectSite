﻿namespace Neonatology.Services.ProfileService;

using System;
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
using ViewModels.Profile;
using static Neonatology.Common.Constants.GlobalConstants.FileConstants;
using static Neonatology.Common.Constants.GlobalConstants.MessageConstants;

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

    public async Task<OperationResult> EditProfileAsync(EditProfileModel model)
    {
        var patient = await this.data.Users
            .Where(x => x.PatientId == model.Id)
            .Include(x => x.Image)
            .FirstOrDefaultAsync();

        if (patient is null)
        {
            return ProfileDoesNotExistErrorMsg;
        }

        if (model.Picture != null)
        {
            var result = await this.fileService.UploadImage(this.cloudinary, model.Picture, ProfileFolderName);
            if (result is null)
            {
                return UnsuccessfulEditMsg;
            }

            await this.fileService.DeleteFile(this.cloudinary, patient.Image.Name, ProfileFolderName);
            patient.Image.IsDeleted = true;
            patient.Image.DeletedOn = DateTime.UtcNow;

            patient.Image = new Image
            {
                Extension = result.Extension,
                Url = result.Uri,
            };
        }

        patient.Patient.FirstName = model.FirstName;
        patient.Patient.LastName = model.LastName;
        patient.Patient.Phone = model.PhoneNumber;
        patient.Patient.CityId = model.CityId;
        patient.Patient.ModifiedOn = DateTime.UtcNow;

        patient.Email = model.Email;
        patient.PhoneNumber = model.PhoneNumber;

        await this.data.SaveChangesAsync();

        return true;
    }
}