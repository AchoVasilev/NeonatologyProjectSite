namespace Neonatology.Controllers;

using System.Threading.Tasks;
using AutoMapper;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Services.CityService;
using Services.ProfileService;

using ViewModels.Profile;

using static Common.Constants.GlobalConstants;

[Authorize(Roles = $"{PatientRoleName}, {AdministratorRoleName}")]
public class ProfileController : BaseController
{
    private readonly IProfileService profileService;
    private readonly ICityService cityService;
    private readonly IMapper mapper;

    public ProfileController(IProfileService profileService, ICityService cityService, IMapper mapper)
    {
        this.profileService = profileService;
        this.cityService = cityService;
        this.mapper = mapper;
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
            return this.NotFound();
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

        var editModel = this.mapper.Map<EditProfileModel>(model);
        var editResult = await this.profileService.EditProfileAsync(editModel);

        if (editResult.Failed)
        {
            this.TempData["Message"] = editResult.Error;
            
            return this.View(model);
        }

        return this.RedirectToAction(nameof(this.Index));
    }

    [AllowAnonymous]
    public IActionResult AllUsers() 
        => this.View();
}