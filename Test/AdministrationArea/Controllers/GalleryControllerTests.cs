namespace Test.AdministrationArea.Controllers;

using System.Collections.Generic;
using System.Threading.Tasks;
using Helpers.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using Neonatology.Services.Administration;
using Neonatology.Services.FileService;
using Neonatology.ViewModels.Administration.Gallery;
using Neonatology.ViewModels.Gallery;
using Neonatology.Web.Areas.Administration.Controllers;
using Xunit;
using GalleryViewModel = Neonatology.ViewModels.Administration.Gallery.GalleryViewModel;

public class GalleryControllerTests
{
    [Fact]
    public async Task AllShouldReturnCorrectCountAndViewModel()
    {
        var images = new List<GalleryViewModel>();
        var galleryServiceMock = new Mock<IGalleryService>();
        galleryServiceMock.Setup(x => x.GetGalleryImages())
            .ReturnsAsync(images);
        
        var controller = new GalleryController(galleryServiceMock.Object, null, null);

        var result = await controller.All();
        var route = Assert.IsType<ViewResult>(result);

        Assert.IsType<GalleryModel>(route.Model);
    }

    [Fact]
    public async Task DeleteShouldReturnRedirectToActionIfSuccessful()
    {
        var httpContext = new DefaultHttpContext();
        var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>())
        {
            ["SessionVariable"] = "admin"
        };

        var galleryServiceMock = new Mock<IGalleryService>();
        galleryServiceMock.Setup(x => x.Delete("1"))
            .ReturnsAsync(true);
    
        var controller = new GalleryController(galleryServiceMock.Object, null, null)
        {
            TempData = tempData
        };
    
        ControllerExtensions.WithIdentity(controller, "1", "gosho@abv.bg", "Doctor");
    
        var result = await controller.Delete("1");
    
        var route = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("All", route.ActionName);
    }
    
    [Fact]
    public async Task DeleteShouldReturnRedirectToActionIfDeleteIsNotSuccessful()
    {
        var httpContext = new DefaultHttpContext();
        var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>())
        {
            ["SessionVariable"] = "admin"
        };

        var galleryServiceMock = new Mock<IGalleryService>();
        galleryServiceMock.Setup(x => x.Delete("1"))
            .ReturnsAsync(true);
    
        var controller = new GalleryController(galleryServiceMock.Object, null, null)
        {
            TempData = tempData
        };
    
        ControllerExtensions.WithIdentity(controller, "1", "gosho@abv.bg", "Doctor");
    
        var result = await controller.Delete("1");
    
        var route = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("All", route.ActionName);
    }
    
    [Fact]
    public void AddShouldReturnViewWithModel()
    {
        var httpContext = new DefaultHttpContext();
        var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>())
        {
            ["SessionVariable"] = "admin"
        };

        var controller = new GalleryController(null, null, null)
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

        var controller = new GalleryController(null, fileService.Object, null)
        {
            TempData = tempData
        };

        ControllerExtensions.WithIdentity(controller, "1", "gosho@abv.bg", "Administrator");

        var model = new UploadImageModel()
        {
            Images = new List<IFormFile>()
        };
        
        var result = await controller.Add(model);
        var route = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("All", route.ActionName);
    }
}