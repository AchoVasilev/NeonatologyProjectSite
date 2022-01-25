namespace Test.ControllerUnitTests
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using Moq;

    using Neonatology.Controllers;

    using Services.NotificationService;

    using Test.Helpers;

    using ViewModels.Notification;

    using Xunit;

    public class NotificationControllerTests
    {
        [Fact]
        public void ControllerShouldHaveAuthorizeAttribute()
        {
            var controller = new NotificationController(null, null, null);
            var actualAttribute = controller.GetType()
                .GetCustomAttributes(typeof(AuthorizeAttribute), true);

            Assert.Equal(typeof(AuthorizeAttribute), actualAttribute[0].GetType());
        }

        [Fact]
        public async Task IndexShouldReturnViewWithCorrectModel()
        {
            var notificationService = new Mock<INotificationService>();
            notificationService.Setup(x => x.GetUserNotifications("gosho", "1", 5, 0))
                .ReturnsAsync(new NotificationModel());

            var controller = new NotificationController(notificationService.Object, null, null);
            ControllerExtensions.WithIdentity(controller, "1", "gosho", "Doctor");

            var result = await controller.Index();
            var root = Assert.IsType<ViewResult>(result);
            Assert.IsType<NotificationModel>(root.Model);
        }
    }
}
