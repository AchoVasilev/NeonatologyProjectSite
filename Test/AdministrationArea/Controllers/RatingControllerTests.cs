namespace Test.AdministrationArea.Controllers;

using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using Neonatology.Services.RatingService;
using Neonatology.ViewModels.Administration.Rating;
using Neonatology.Web.Areas.Administration.Controllers;
using Xunit;

public class RatingControllerTests
{
    [Fact]
    public async Task AllShouldReturnViewWithModel()
    {
        var service = new Mock<IRatingService>();
        var model = new List<RatingViewModel>();

        service.Setup(x => x.GetRatings())
            .ReturnsAsync(model);

        var controller = new RatingController(service.Object);

        var result = await controller.All();

        var route = Assert.IsType<ViewResult>(result);
        Assert.IsAssignableFrom<List<RatingViewModel>>(route.Model);
    }

    [Fact]
    public async Task ApproveShouldReturnRedirectToActionWhenSuccessful()
    {
        var service = new Mock<IRatingService>();
        service.Setup(x => x.ApproveRating(1))
            .ReturnsAsync(true);

        var httpContext = new DefaultHttpContext();
        var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>())
        {
            ["SessionVariable"] = "admin"
        };

        var controller = new RatingController(service.Object)
        {
            TempData = tempData
        };

        var result = await controller.Approve(1);
        var route = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("All", route.ActionName);
    }

    [Fact]
    public async Task ApproveShouldReturnRedirectToActionWhenNotSuccessful()
    {
        var service = new Mock<IRatingService>();
        service.Setup(x => x.ApproveRating(1))
            .ReturnsAsync(false);

        var httpContext = new DefaultHttpContext();
        var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>())
        {
            ["SessionVariable"] = "admin"
        };

        var controller = new RatingController(service.Object)
        {
            TempData = tempData
        };

        var result = await controller.Approve(1);
        var route = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("All", route.ActionName);
    }

    [Fact]
    public async Task DeleteShouldReturnRedirectToActionWhenSuccessful()
    {
        var service = new Mock<IRatingService>();
        service.Setup(x => x.DeleteRating(1))
            .ReturnsAsync(true);

        var httpContext = new DefaultHttpContext();
        var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>())
        {
            ["SessionVariable"] = "admin"
        };

        var controller = new RatingController(service.Object)
        {
            TempData = tempData
        };

        var result = await controller.Delete(1);

        var route = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("All", route.ActionName);
    }

    [Fact]
    public async Task DeleteShouldReturnRedirectToActionWhenNotSuccessful()
    {
        var service = new Mock<IRatingService>();
        service.Setup(x => x.DeleteRating(1))
            .ReturnsAsync(false);

        var httpContext = new DefaultHttpContext();
        var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>())
        {
            ["SessionVariable"] = "admin"
        };

        var controller = new RatingController(service.Object)
        {
            TempData = tempData
        };

        var result = await controller.Delete(1);

        var route = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("All", route.ActionName);
    }
}