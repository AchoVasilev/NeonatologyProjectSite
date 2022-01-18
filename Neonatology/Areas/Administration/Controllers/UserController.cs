namespace Neonatology.Areas.Administration.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using AutoMapper;

    using global::Services.PatientService;

    using Microsoft.AspNetCore.Mvc;

    using Neonatology.Areas.Administration.ViewModels.User;

    public class UserController : BaseController
    {
        private readonly IPatientService patientService;
        private readonly IMapper mapper;

        public UserController(IPatientService patientService, IMapper mapper)
        {
            this.patientService = patientService;
            this.mapper = mapper;
        }

        public async Task<IActionResult> All()
        {
            var models = await this.patientService.GetAllPatients();
            var users = this.mapper.Map <ICollection<UserViewModel>>(models);

            return View(users);
        }
    }
}
