namespace Test.ControllerUnitTests
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;

    using Moq;

    using Neonatology.Controllers;

    using Services.FeedbackService;

    using ViewModels.Feedback;

    using Xunit;

    public class FeedbackControllerTests
    {
        [Fact]
        public void SendShouldReturnViewWithModel()
        {
            var controller = new FeedbackController(null);

            var result = controller.Send();

            var route = Assert.IsType<ViewResult>(result);

            Assert.IsType<FeedbackInputModel>(route.Model);
        }

        [Fact]
        public async Task SendShouldRedirectToIndexHomeIfSuccessfulWithTempDataMessage()
        {
            var serviceMock = new Mock<IFeedbackService>();

            var controller = new FeedbackController(serviceMock.Object);

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
    }
}
