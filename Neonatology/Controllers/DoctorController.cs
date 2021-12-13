namespace Neonatology.Controllers
{
    using System.Threading.Tasks;

    using Infrastructure;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using Services.CityService;
    using Services.DoctorService;

    using ViewModels.Doctor;

    using static Common.GlobalConstants;

    public class DoctorController : BaseController
    {
        private readonly IDoctorService doctorService;
        private readonly ICityService cityService;

        public DoctorController(IDoctorService doctorService, ICityService cityService)
        {
            this.doctorService = doctorService;
            this.cityService = cityService;
        }

        public async Task<IActionResult> Profile()
        {
            var viewModel = await this.doctorService.GetDoctorById(DoctorId);

            return View(viewModel);
        }

        [Authorize(Roles = DoctorRoleName)]
        public async Task<IActionResult> Edit()
        {
            var userId = this.User.GetId();
            var userIsDoctor = await this.doctorService.UserIsDoctor(userId);

            if (!userIsDoctor)
            {
                return new StatusCodeResult(404);
            }

            var doctorId = await this.doctorService.GetDoctorIdByUserId(userId);
            var doctorInfo = await this.doctorService.GetDoctorById(doctorId);

            var model = new DoctorEditFormModel()
            {
                Id = doctorInfo.Id,
                FirstName = doctorInfo.FullName.Split(' ')[0],
                LastName = doctorInfo.FullName.Split(' ')[1],
                PhoneNumber = doctorInfo.PhoneNumber,
                Address = doctorInfo.Address,
                Age = doctorInfo.Age,
                Biography = doctorInfo.Biography,
                ImageUrl = doctorInfo.ImageUrl,
                Email = doctorInfo.Email,
                YearsOfExperience = doctorInfo.YearsOfExperience,
                Specializations = doctorInfo.Specializations,
                Cities = this.cityService.GetAllCities()
            };

            return View(model);
        }

        [Authorize(Roles = DoctorRoleName)]
        [HttpPost]
        public async Task<IActionResult> Edit(DoctorEditFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            return RedirectToAction();
        }

        [Authorize(Roles = DoctorRoleName)]
        public IActionResult EditDates()
        {
            return View();
        }
    }
}
