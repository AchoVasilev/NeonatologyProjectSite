namespace Test.AdministrationArea.Controllers;

using System.Collections.Generic;
using System.Threading.Tasks;
using Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using Neonatology.Areas.Administration.Controllers;
using Services.OfferService;
using ViewModels.Administration.Offer;
using ViewModels.Offer;
using Xunit;

public class OfferControllerTests
{
    [Fact]
    public async Task AllShouldReturnViewResultWithModel()
    {
        var offerService = new Mock<IOfferService>();
        var model = new List<OfferViewModel>();

        offerService.Setup(x => x.GetAllAsync())
            .ReturnsAsync(model);

        var httpContext = new DefaultHttpContext();
        var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>())
        {
            ["SessionVariable"] = "admin"
        };

        var controller = new OfferController(offerService.Object)
        {
            TempData = tempData
        };

        ControllerExtensions.WithIdentity(controller, "1", "gosho@abv.bg", "Administrator");

        var result = await controller.All();
        var route = Assert.IsType<ViewResult>(result);

        Assert.IsAssignableFrom<List<OfferViewModel>>(route.Model);
    }

    [Fact]
    public async Task DeleteShouldReturnRedirectToActionWhenSuccessful()
    {
        var offerService = new Mock<IOfferService>();

        offerService.Setup(x => x.DeleteOffer(1))
            .ReturnsAsync(true);

        var httpContext = new DefaultHttpContext();
        var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>())
        {
            ["SessionVariable"] = "admin"
        };

        var controller = new OfferController(offerService.Object)
        {
            TempData = tempData
        };

        ControllerExtensions.WithIdentity(controller, "1", "gosho@abv.bg", "Administrator");

        var result = await controller.Delete(1);
        var route = Assert.IsType<RedirectToActionResult>(result);

        Assert.Equal("All", route.ActionName);
    }

    [Fact]
    public async Task DeleteShouldReturnRedirectToActionWhenNotSuccessful()
    {
        var offerService = new Mock<IOfferService>();

        offerService.Setup(x => x.DeleteOffer(1))
            .ReturnsAsync(false);

        var httpContext = new DefaultHttpContext();
        var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>())
        {
            ["SessionVariable"] = "admin"
        };

        var controller = new OfferController(offerService.Object)
        {
            TempData = tempData
        };

        ControllerExtensions.WithIdentity(controller, "1", "gosho@abv.bg", "Administrator");

        var result = await controller.Delete(1);
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

        var controller = new OfferController(null)
        {
            TempData = tempData
        };

        ControllerExtensions.WithIdentity(controller, "1", "gosho@abv.bg", "Administrator");

        var result = controller.Add();

        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public async Task AddShouldReturnRedirectToActionAll()
    {
        var offerService = new Mock<IOfferService>();
        var model = new CreateOfferFormModel();
        offerService.Setup(x => x.AddOffer(model))
            .Returns(Task.CompletedTask);

        var httpContext = new DefaultHttpContext();
        var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>())
        {
            ["SessionVariable"] = "admin"
        };

        var controller = new OfferController(offerService.Object)
        {
            TempData = tempData
        };

        ControllerExtensions.WithIdentity(controller, "1", "gosho@abv.bg", "Administrator");

        var result = await controller.Add(model);
        var route = Assert.IsType<RedirectToActionResult>(result);

        Assert.Equal("All", route.ActionName);
    }

    [Fact]
    public async Task EditShouldReturnViewWithModel()
    {
        var model = new EditOfferFormModel();

        var service = new Mock<IOfferService>();
        service.Setup(x => x.GetOffer(1))
            .ReturnsAsync(model);

        var httpContext = new DefaultHttpContext();
        var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>())
        {
            ["SessionVariable"] = "admin"
        };

        var controller = new OfferController(service.Object)
        {
            TempData = tempData
        };

        ControllerExtensions.WithIdentity(controller, "1", "gosho@abv.bg", "Administrator");

        var result = await controller.Edit(1);

        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public async Task EditShouldReturnRedirectToActionAll()
    {
        var offerService = new Mock<IOfferService>();
        var model = new EditOfferFormModel();
        offerService.Setup(x => x.EditOffer(model))
            .ReturnsAsync(true);

        var httpContext = new DefaultHttpContext();
        var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>())
        {
            ["SessionVariable"] = "admin"
        };

        var controller = new OfferController(offerService.Object)
        {
            TempData = tempData
        };

        ControllerExtensions.WithIdentity(controller, "1", "gosho@abv.bg", "Administrator");

        var result = await controller.Edit(model);
        var route = Assert.IsType<RedirectToActionResult>(result);

        Assert.Equal("All", route.ActionName);
    }

    [Fact]
    public async Task EditShouldReturnRedirectToActionAllWhenNotSuccessful()
    {
        var offerService = new Mock<IOfferService>();
        var model = new EditOfferFormModel();
        offerService.Setup(x => x.EditOffer(model))
            .ReturnsAsync(false);

        var httpContext = new DefaultHttpContext();
        var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>())
        {
            ["SessionVariable"] = "admin"
        };

        var controller = new OfferController(offerService.Object)
        {
            TempData = tempData
        };

        ControllerExtensions.WithIdentity(controller, "1", "gosho@abv.bg", "Administrator");

        var result = await controller.Edit(model);
        var route = Assert.IsType<RedirectToActionResult>(result);

        Assert.Equal("All", route.ActionName);
    }
}