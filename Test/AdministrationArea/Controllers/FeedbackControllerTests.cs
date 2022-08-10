namespace Test.AdministrationArea.Controllers;

using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Mocks;
using Moq;
using Neonatology.Services.FeedbackService;
using Neonatology.ViewModels.Feedback;
using Neonatology.Web.Areas.Administration.Controllers;
using Xunit;

public class FeedbackControllerTests
{
    [Fact]
    public async Task AllShouldReturnViewAndModel()
    {
        // Arrange
        var model = new List<FeedbackViewModel>();
        var service = new Mock<IFeedbackService>();
        service.Setup(x => x.GetAll())
            .ReturnsAsync(model);

        var controller = new FeedbackController(service.Object, null);

        // Act
        var result = await controller.All();

        // Assert 
        var route = Assert.IsType<ViewResult>(result);
        Assert.IsAssignableFrom<ICollection<FeedbackViewModel>>(route.Model);
    }

    [Fact]
    public async Task InformationShouldReturnCorrectViewAndModel()
    {
        // Arrange
        var service = new Mock<IFeedbackService>();
        service.Setup(x => x.GetById(1))
            .ReturnsAsync(new FeedbackViewModel());

        var mapper = MapperMock.Instance;
        // Act
        var controller = new FeedbackController(service.Object, null);
        var result = await controller.Information(1);

        // Assert 
        var route = Assert.IsType<ViewResult>(result);
        Assert.IsType<FeedbackViewModel>(route.Model);
    }

    [Fact]
    public async Task DeleteShouldReturnRedirectToAllWithTempDataMessage()
    {
        // Arrange
        var service = new Mock<IFeedbackService>();
        service.Setup(x => x.Delete(1))
            .ReturnsAsync(true);

        var httpContext = new DefaultHttpContext();
        var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>())
        {
            ["SessionVariable"] = "admin"
        };

        var controller = new FeedbackController(service.Object, null)
        {
            TempData = tempData
        };

        // Act
        var result = await controller.Delete(1);

        // Assert 
        var route = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("All", route.ActionName);
    }

    [Fact]
    public async Task ReplyShouldReturnViewAndModel()
    {
        // Arrange
        var model = new FeedbackViewModel()
        {
            Comment = "asdasdasdasd",
            FirstName = "Gosho",
            LastName = "Ivanov",
            Email = "gosho@gosho.bg",
            Id = 5,
            Type = "typetype",
        };

        var service = new Mock<IFeedbackService>();
        service.Setup(x => x.GetById(5))
            .ReturnsAsync(model);

        var email = new Mock<IEmailSender>();



        var controller = new FeedbackController(service.Object, email.Object);
        // Act
        var result = await controller.Reply(5);

        // Assert 
        var route = Assert.IsType<ViewResult>(result);
        Assert.IsType<FeedbackReplyModel>(route.Model);
    }

    [Fact]
    public async Task ReplyShouldReturnRedirectToAction()
    {
        // Arrange
        var model = new FeedbackReplyModel()
        {
            FeedbackId = 1,
            ReceiverEmail = "mail@abv.bg",
            Subject = "mailmail",
            Text = "hello"
        };
            
        var service = new Mock<IFeedbackService>();
        service.Setup(x => x.SolveFeedback(1))
            .Returns(Task.CompletedTask);

        var mailSender = new Mock<IEmailSender>();
        mailSender.Setup(x => x.SendEmailAsync("mail@abv.bg", "mailmail", "hello"))
            .Returns(Task.CompletedTask);
            
        var httpContext = new DefaultHttpContext();
        var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>())
        {
            ["SessionVariable"] = "admin"
        };
            
        var controller = new FeedbackController(service.Object, mailSender.Object)
        {
            TempData = tempData
        };

        // Act
        var result = await controller.Reply(model);

        // Assert 
        var route = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("All", route.ActionName);
    }
}