namespace Neonatology.Controllers
{
    using System.Threading.Tasks;

    using Infrastructure;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using Services.CityService;
    using Services.ProfileService;

    using ViewModels.Profile;

    using static Common.GlobalConstants;
    using static Common.GlobalConstants.Messages;

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

            return View(model);
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

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditProfileFormModel model)
        {
            var city = this.cityService.GetCityById(model.CityId);

            if (city is null)
            {
                ModelState.AddModelError("cityId", "City with this id does not exist");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var isEdited = await this.profileService.EditProfileAsync(model);

            if (isEdited == false)
            {
                this.TempData["Message"] = UnsuccessfulEditMsg;
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        [AllowAnonymous]
        public IActionResult AllUsers()
        {
            return View();
        }
    }
}
