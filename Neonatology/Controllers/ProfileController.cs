namespace Neonatology.Controllers
{
    using System.Threading.Tasks;

    using Infrastructure;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using Services.ProfileService;
    using static Common.GlobalConstants;

    [Authorize(Roles = PatientRoleName)]
    public class ProfileController : BaseController
    {
        private readonly IProfileService profileService;

        public ProfileController(IProfileService profileService)
        {
            this.profileService = profileService;
        }

        public async Task<IActionResult> Index()
        {
            var model = await this.profileService.GetPatientData(this.User.GetId());

            return View(model);
        }

        public IActionResult AllUsers()
        {
            return View();
        }
    }
}
