﻿namespace Neonatology.Controllers;

using System.Threading.Tasks;

using Infrastructure;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Services.CityService;
using Services.ProfileService;

using ViewModels.Profile;

using static Common.GlobalConstants;

[Authorize(Roles = $"{PatientRoleName}, {AdministratorRoleName}")]
public class ProfileController : BaseController
{
    private readonly IProfileService profileService;
    private readonly ICityService cityService;

    public ProfileController(IProfileService profileService, ICityService cityService)
    {
        this.profileService = profileService;
        this.cityService = cityService;
    }

    public async Task<IActionResult> Index()
    {
        var model = await this.profileService.GetPatientData(this.User.GetId());

        return this.View(model);
    }

    public async Task<IActionResult> Edit(string userId)
    {
        var patientData = await this.profileService.GetPatientData(userId);

        if (patientData is null)
        {
            return new StatusCodeResult(404);
        }

        var cityId = await this.cityService.GetCityIdByName(patientData.CityName);
        var model = new EditProfileFormModel()
        {
            Id = patientData.Id,
            FirstName = patientData.FirstName,
            LastName = patientData.LastName,
            PhoneNumber = patientData.Phone,
            UserImageUrl = patientData.UserImageUrl,
            Email = patientData.UserEmail,
            CityId = cityId,
            Cities = await this.cityService.GetAllCities()
        };

        return this.View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(EditProfileFormModel model)
    {
        if (!this.ModelState.IsValid)
        {
            return this.View(model);
        }
        
        var result = await this.cityService.CityExists(model.CityId);

        if (result.Failed)
        {
            this.TempData["Message"] = result.Error;
            
            return this.View(model);
        }

        var editResult = await this.profileService.EditProfileAsync(model);

        if (editResult.Failed)
        {
            this.TempData["Message"] = editResult.Error;
            
            return this.View(model);
        }

        return this.RedirectToAction(nameof(this.Index));
    }

    [AllowAnonymous]
    public IActionResult AllUsers()
    {
        return this.View();
    }
}