namespace Neonatology.Controllers;

using System.Diagnostics;
using System.Threading.Tasks;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Mvc;

using Services.DoctorService;

using ViewModels.ErrorViewModel;
using ViewModels.Home;

using static Common.Constants.WebConstants.RouteTemplates;
public class HomeController : BaseController
{
    private readonly IDoctorService doctorService;

    public HomeController(IDoctorService doctorService) 
        => this.doctorService = doctorService;

    [Route(HomeIndexSlash)]
    [Route(HomeIndex)]
    public async Task<IActionResult> Index()
    {
        if (this.User.IsAdmin())
        {
            return this.RedirectToAction("Index", "Home", new { Area = "Administration" });
        }

        var model = new HomeViewModel()
        {
            DoctorId = await this.doctorService.GetDoctorId()
        };

        return this.View(model);
    }

    [Route(HomeError404)]
    public IActionResult Error404() 
        => this.View();

    [Route(HomeError400)]
    public IActionResult Error400() 
        => this.View();

    public IActionResult Privacy() 
        => this.View();

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() 
        => this.View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
}