namespace Test.AdministrationArea.Controllers;

using System.Collections.Generic;
using System.Threading.Tasks;
using Helpers.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Mocks;
using Moq;
using Neonatology.Services.OfferService;
using Neonatology.ViewModels.Administration.Offer;
using Neonatology.ViewModels.Offer;
using Neonatology.Web.Areas.Administration.Controllers;
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

        var controller = new OfferController(offerService.Object, null)
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

        var controller = new OfferController(offerService.Object, null)
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

        var controller = new OfferController(offerService.Object, null)
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

        var controller = new OfferController(null, null)
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
        var serviceModel = new CreateOfferServiceModel();
        offerService.Setup(x => x.AddOffer(serviceModel))
            .Returns(Task.CompletedTask);

        var mapper = MapperMock.Instance;
        var httpContext = new DefaultHttpContext();
        var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>())
        {
            ["SessionVariable"] = "admin"
        };

        var controller = new OfferController(offerService.Object, mapper)
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

        var controller = new OfferController(service.Object, null)
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
        var serviceModel = new EditOfferServiceModel();
        offerService.Setup(x => x.EditOffer(serviceModel))
            .ReturnsAsync(true);

        var mapper = MapperMock.Instance;
        var httpContext = new DefaultHttpContext();
        var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>())
        {
            ["SessionVariable"] = "admin"
        };

        var controller = new OfferController(offerService.Object, mapper)
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
        var serviceModel = new EditOfferServiceModel();
        offerService.Setup(x => x.EditOffer(serviceModel))
            .ReturnsAsync(false);

        var mapper = MapperMock.Instance;
        var httpContext = new DefaultHttpContext();
        var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>())
        {
            ["SessionVariable"] = "admin"
        };

        var controller = new OfferController(offerService.Object, mapper)
        {
            TempData = tempData
        };

        ControllerExtensions.WithIdentity(controller, "1", "gosho@abv.bg", "Administrator");

        var result = await controller.Edit(model);
        var route = Assert.IsType<RedirectToActionResult>(result);

        Assert.Equal("All", route.ActionName);
    }
}