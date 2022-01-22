namespace Neonatology.Areas.Administration.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using AutoMapper;

    using global::Services.CityService;
    using global::Services.PatientService;
    using global::Services.ProfileService;
    using global::ViewModels.Profile;

    using Microsoft.AspNetCore.Mvc;

    using ViewModels.Administration.User;

    using static Common.GlobalConstants.Messages;

    public class UserController : BaseController
    {
        private readonly IPatientService patientService;
        private readonly IMapper mapper;
        private readonly ICityService cityService;
        private readonly IProfileService profileService;

        public UserController(IPatientService patientService, IMapper mapper, ICityService cityService, IProfileService profileService)
        {
            this.patientService = patientService;
            this.mapper = mapper;
            this.cityService = cityService;
            this.profileService = profileService;
        }

        public async Task<IActionResult> All()
        {
            var models = await this.patientService.GetAllPatients();
            var users = this.mapper.Map<ICollection<UserViewModel>>(models);

            return View(users);
        }

        public async Task<IActionResult> Edit(string id)
        {
            var patientData = await this.profileService.GetPatientData(id);

            if (patientData == null)
            {
                return new StatusCodeResult(404);
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

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditProfileFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var isEdited = await this.profileService.EditProfileAsync(model);

            if (isEdited == false)
            {
                this.TempData["Message"] = UnsuccessfulEditMsg;
                model.Cities = await this.cityService.GetAllCities();

                return View(model);
            }

            return RedirectToAction(nameof(All));
        }
    }
}
