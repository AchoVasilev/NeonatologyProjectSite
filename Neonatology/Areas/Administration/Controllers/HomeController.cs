namespace Neonatology.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using global::Services.AppointmentService;
    using global::Services.PatientService;
    using global::Services.RatingService;

    using Microsoft.AspNetCore.Mvc;

    using ViewModels.Administration.Home;

    public class HomeController : BaseController
    {
        private readonly IAppointmentService appointmentService;
        private readonly IPatientService patientService;
        private readonly IRatingService ratingService;

        public HomeController(IAppointmentService appointmentService, IPatientService patientService, IRatingService ratingService)
        {
            this.appointmentService = appointmentService;
            this.patientService = patientService;
            this.ratingService = ratingService;
        }

        public async Task<IActionResult> Index()
        {
            var model = new IndexViewModel
            {
                LatestPatientsRegisterCount = await this.patientService.GetLastThisMonthsRegisteredCount(),
                TotalPatientsCount = await this.patientService.GetPatientsCount(),
                TotalRatingsCount = await this.ratingService.GetRatingsCount(),
                TotalAppointmentsCount = await this.appointmentService.GetTotalAppointmentsCount(),
                AllAppointments = await this.appointmentService.GetAllAppointments()
            };

            return this.View(model);
        }
    }
}
