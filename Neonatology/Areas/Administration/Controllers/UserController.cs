namespace Neonatology.Areas.Administration.Controllers;

using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Services.CityService;
using Services.PatientService;
using Services.ProfileService;
using ViewModels.Profile;
using Microsoft.AspNetCore.Mvc;
using ViewModels.Administration.User;
using static Common.Constants.GlobalConstants.MessageConstants;
using Data.Models;
using Microsoft.AspNetCore.Identity;

public class UserController : BaseController
{
    private readonly ICityService cityService;
    private readonly IMapper mapper;
    private readonly IPatientService patientService;
    private readonly IProfileService profileService;
    private readonly UserManager<ApplicationUser> userManager;

    public UserController(IPatientService patientService, IMapper mapper, ICityService cityService,
        IProfileService profileService, UserManager<ApplicationUser> userManager)
    {
        this.patientService = patientService;
        this.mapper = mapper;
        this.cityService = cityService;
        this.profileService = profileService;
        this.userManager = userManager;
    }

    public async Task<IActionResult> All()
    {
        var models = await this.patientService.GetAllPatients();
        var users = this.mapper.Map<ICollection<UserViewModel>>(models);

        return this.View(users);
    }

    public async Task<IActionResult> Edit(string id)
    {
        var patientData = await this.profileService.GetPatientData(id);

        if (patientData is null)
        {
            return this.NotFound();
        }

        var cityId = await this.cityService.GetCityIdByName(patientData.CityName);
        var model = new EditProfileFormModel
        {
            Id = patientData.Id,
            UserId = patientData.UserId,
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

        var editModel = this.mapper.Map<EditProfileModel>(model);
        var result = await this.profileService.EditProfileAsync(editModel);

        if (result.Failed)
        {
            this.TempData["Message"] = result.Error;
            model.Cities = await this.cityService.GetAllCities();

            return this.View(model);
        }

        return this.RedirectToAction(nameof(this.All));
    }

    public async Task<IActionResult> Delete(string userId)
    {
        var user = await this.userManager.FindByIdAsync(userId);
        if (user is null)
        {
            return this.NotFound($"Не успяхме да заредим потребител с номер '{this.userManager.GetUserId(this.User)}'.");
        }

        var result = await this.userManager.DeleteAsync(user);
        var patientIsDeleted = await this.patientService.DeletePatient(userId);

        if (!result.Succeeded || !patientIsDeleted)
        {
            this.TempData["Message"] = UnsuccessfulEditMsg;
        }

        return this.RedirectToAction(nameof(this.All));
    }
}