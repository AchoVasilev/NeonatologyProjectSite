namespace Test.ControllerUnitTests;

using System.Threading.Tasks;
using Helpers.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Mocks;
using Moq;
using Neonatology.Services.FeedbackService;
using Neonatology.ViewModels.Feedback;
using Neonatology.Web.Controllers;
using Xunit;

public class FeedbackControllerTests
{
    [Fact]
    public void SendShouldReturnViewWithModel()
    {
        var controller = new FeedbackController(null, null);

        var result = controller.Send();

        var route = Assert.IsType<ViewResult>(result);

        Assert.IsType<FeedbackInputModel>(route.Model);
    }

    [Fact]
    public async Task SendShouldRedirectToIndexHomeIfSuccessfulWithTempDataMessage()
    {
        var serviceMock = new Mock<IFeedbackService>();
        var mapper = MapperMock.Instance;
        
        var controller = new FeedbackController(serviceMock.Object, mapper);

        var httpContext = new DefaultHttpContext();
        var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>())
        {
            ["SessionVariable"] = "admin"
        };
        controller.TempData = tempData;
        var model = new FeedbackInputModel
        {
            Comment = "asdasdasd",
            FirstName = "Gosho",
            LastName = "Peshev",
            Email = "Gosho@gosho.bg",
            Type = "GoshoEpechen"
        };

        var result = await controller.Send(model);

        var route = Assert.IsType<RedirectToActionResult>(result);

        Assert.Equal("Index", route.ActionName);
        Assert.Equal("Home", route.ControllerName);
    }

    [Fact]
    public async Task MyFeedbacksShouldReturnViewAndModelWhenSuccessful()
    {
        var serviceMock = new Mock<IFeedbackService>();
        
        var controller = new FeedbackController(serviceMock.Object, null);

        var httpContext = new DefaultHttpContext();
        var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>())
        {
            ["SessionVariable"] = "admin"
        };
            
        controller.TempData = tempData;
        ControllerExtensions.WithIdentity(controller, "1", "Gosho@gosho.bg", "Patient");
            
        var model = new FeedbackInputModel
        {
            Comment = "asdasdasd",
            FirstName = "Gosho",
            LastName = "Peshev",
            Email = "Gosho@gosho.bg",
            Type = "GoshoEpechen"
        };

        var sentResult = await controller.Send(model);

        var myFeedbacks = await controller.MyFeedbacks("Gosho@gosho.bg");

        var route = Assert.IsType<ViewResult>(myFeedbacks);
    }
}