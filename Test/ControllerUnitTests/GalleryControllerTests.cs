namespace Test.ControllerUnitTests;

using System.Collections.Generic;
using System.Threading.Tasks;
using Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using Neonatology.Controllers;
using Services.AppointmentCauseService;
using Services.FileService;
using ViewModels.Appointments;
using ViewModels.Gallery;
using Xunit;

public class GalleryControllerTests
{
    [Fact]
    public async Task AllShouldReturnCorrectCountAndViewModel()
    {
        var model = new GalleryViewModel();
        
        var fileService = new Mock<IFileService>();
        fileService.Setup(x => x.GetGalleryImagesAsync(1, 8))
            .ReturnsAsync(model);

        var appointmentCauses = new List<AppointmentCauseViewModel>();

        var appCauseService = new Mock<IAppointmentCauseService>();
        appCauseService.Setup(x => x.GetAllCauses())
            .ReturnsAsync(appointmentCauses);

        var controller = new GalleryController(fileService.Object, null);

        var result = await controller.All(1);
        var route = Assert.IsType<ViewResult>(result);

        Assert.IsType<GalleryViewModel>(route.Model);
    }

    [Fact]
    public void AddShouldReturnViewWithModel()
    {
        var httpContext = new DefaultHttpContext();
        var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>())
        {
            ["SessionVariable"] = "admin"
        };

        var controller = new GalleryController(null, null)
        {
            TempData = tempData
        };

        ControllerExtensions.WithIdentity(controller, "1", "gosho@abv.bg", "Doctor");

        var result = controller.Add();

        var route = Assert.IsType<ViewResult>(result);
        Assert.IsType<UploadImageModel>(route.Model);
    }

    [Fact]
    public async Task HttpPostAddShouldReturnRediractToActionToAllWhenSuccessful()
    {
        var fileService = new Mock<IFileService>();

        var httpContext = new DefaultHttpContext();
        var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>())
        {
            ["SessionVariable"] = "admin"
        };

        var controller = new GalleryController(fileService.Object, null)
        {
            TempData = tempData
        };

        ControllerExtensions.WithIdentity(controller, "1", "gosho@abv.bg", "Doctor");

        var model = new UploadImageModel()
        {
            Images = new List<IFormFile>()
        };
        
        var result = await controller.Add(model);
        var route = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("All", route.ActionName);
    }
}