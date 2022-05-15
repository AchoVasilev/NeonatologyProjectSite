namespace Test.ControllerUnitTests;

using System.Collections.Generic;
using System.Threading.Tasks;
using Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using Neonatology.Controllers;
using Services.DoctorService;
using ViewModels.Gallery;
using ViewModels.Home;
using Xunit;

public class HomeControllerTests
{
    [Fact]
    public async Task IndexShouldReturnView()
    {
        var doctorService = new Mock<IDoctorService>();
        doctorService.Setup(x => x.GetDoctorId())
            .ReturnsAsync("1");
        
        var httpContext = new DefaultHttpContext();
        var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>())
        {
            ["SessionVariable"] = "admin"
        };

        var controller = new HomeController(doctorService.Object)
        {
            TempData = tempData
        };

        ControllerExtensions.WithIdentity(controller, "1", "gosho@abv.bg", "Doctor");

        var model = new UploadImageModel()
        {
            Images = new List<IFormFile>()
        };
        
        var result = await controller.Index();
        var route = Assert.IsType<ViewResult>(result);
        Assert.IsType<HomeViewModel>(route.Model);
    }
    
    [Fact]
    public async Task IndexShouldReturnRedirectToActionIfUserIsAdmin()
    {
        var doctorService = new Mock<IDoctorService>();
        doctorService.Setup(x => x.GetDoctorId())
            .ReturnsAsync("1");
        
        var httpContext = new DefaultHttpContext();
        var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>())
        {
            ["SessionVariable"] = "admin"
        };

        var controller = new HomeController(doctorService.Object)
        {
            TempData = tempData
        };

        ControllerExtensions.WithIdentity(controller, "1", "gosho@abv.bg", "Administrator");

        var model = new UploadImageModel()
        {
            Images = new List<IFormFile>()
        };
        
        var result = await controller.Index();
        var route = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", route.ActionName);
    }

    [Fact]
    public void Error404ShouldReturnView()
    {
        var controller = new HomeController(null);

        var result = controller.Error404();
        Assert.IsType<ViewResult>(result);
    }
    
    [Fact]
    public void Error400ShouldReturnView()
    {
        var controller = new HomeController(null);

        var result = controller.Error400();
        Assert.IsType<ViewResult>(result);
    }
    
    [Fact]
    public void PrivacyShouldReturnView()
    {
        var controller = new HomeController(null);

        var result = controller.Privacy();
        Assert.IsType<ViewResult>(result);
    }
}