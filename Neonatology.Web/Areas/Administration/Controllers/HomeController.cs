namespace Neonatology.Web.Areas.Administration.Controllers;

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Services.AppointmentService;
using Services.PatientService;
using Services.RatingService;
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
        => this.View(new IndexViewModel
        {
            LatestPatientsRegisterCount = await this.patientService.GetLastThisMonthsRegisteredCount(),
            TotalPatientsCount = await this.patientService.GetPatientsCount(),
            TotalRatingsCount = await this.ratingService.GetRatingsCount(),
            TotalAppointmentsCount = await this.appointmentService.GetTotalAppointmentsCount(),
            AllAppointments = await this.appointmentService.GetAllAppointments()
        });
}