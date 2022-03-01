﻿namespace Neonatology.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using Services.CityService;
    using Services.DoctorService;

    using ViewModels.Doctor;
    using ViewModels.Slot;

    using static Common.GlobalConstants;
    using static Common.GlobalConstants.MessageConstants;

    [Authorize(Roles = DoctorConstants.DoctorRoleName)]
    public class DoctorController : BaseController
    {
        private readonly IDoctorService doctorService;
        private readonly ICityService cityService;

        public DoctorController(IDoctorService doctorService, ICityService cityService)
        {
            this.doctorService = doctorService;
            this.cityService = cityService;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Profile()
        {
            var doctorId = await this.doctorService.GetDoctorId();
            var viewModel = await this.doctorService.GetDoctorById(doctorId);

            return View(viewModel);
        }

        public async Task<IActionResult> Edit()
        {
            var doctorId = await this.doctorService.GetDoctorId();
            var doctorInfo = await this.doctorService.GetDoctorById(doctorId);

            var model = new DoctorEditFormModel()
            {
                Id = doctorInfo.Id,
                FirstName = doctorInfo.FullName.Split(' ')[0],
                LastName = doctorInfo.FullName.Split(' ')[1],
                PhoneNumber = doctorInfo.PhoneNumber,
                Age = doctorInfo.Age,
                Biography = doctorInfo.Biography,
                UserImageUrl = doctorInfo.UserImageUrl,
                Email = doctorInfo.Email,
                YearsOfExperience = doctorInfo.YearsOfExperience,
                Cities = await this.cityService.GetAllCities(),
                Addresses = await this.doctorService.GetDoctorAddressesById(doctorInfo.Id)
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(DoctorEditFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var isEdited = await this.doctorService.EditDoctorAsync(model);

            if (isEdited == false)
            {
                this.TempData["Message"] = UnsuccessfulEditMsg;
                return View(model);
            }

            return RedirectToAction(nameof(Profile));
        }

        public IActionResult Calendar()
        {
            return View(new SlotEditModel());
        }
    }
}
