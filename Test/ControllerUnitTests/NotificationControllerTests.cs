namespace Test.ControllerUnitTests
{
    using System.Security.Claims;
    using System.Threading;
    using System.Threading.Tasks;
    using Data.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.SignalR;
    using Moq;

    using Neonatology.Controllers;
    using Services.NotificationService;

    using Helpers;
    using Infrastructure.Hubs;
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

        [Fact]
        public async Task EditStatusShouldReturnTrueIfSuccessful()
        {
            var user = new ApplicationUser()
            {
                Id = "1",
                UserName = "gosho"
            };
            
            var notificationService = new Mock<INotificationService>();
            var hub = new Mock<IHubContext<NotificationHub>>();
            hub.Setup(x => x.Clients.User(user.Id)
                    .SendCoreAsync(It.IsAny<string>(), It.IsAny<object?[]>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
            
            var userManager = new Mock<UserManager<ApplicationUser>>(Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);
            userManager.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(user);
            notificationService.Setup(x => x.EditStatus("1", "Прочетено", "2"))
                .ReturnsAsync(true);

            var controller = new NotificationController(notificationService.Object, hub.Object, userManager.Object);
            ControllerExtensions.WithIdentity(controller, "1", "gosho", "Doctor");

            var result = await controller.EditStatus("Прочетено", "2");
            
            Assert.True(result);
        }

        [Fact]
        public async Task DeleteNotificationShouldReturnTrueWhenSuccessful()
        {
            var user = new ApplicationUser()
            {
                Id = "1",
                UserName = "gosho"
            };
            
            var notificationService = new Mock<INotificationService>();
            var hub = new Mock<IHubContext<NotificationHub>>();
            hub.Setup(x => x.Clients.User(user.Id)
                    .SendCoreAsync(It.IsAny<string>(), It.IsAny<object?[]>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
            
            var userManager = new Mock<UserManager<ApplicationUser>>(Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);
            userManager.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(user);
            notificationService.Setup(x => x.DeleteNotification("1", "2"))
                .ReturnsAsync(true);

            var controller = new NotificationController(notificationService.Object, hub.Object, userManager.Object);
            ControllerExtensions.WithIdentity(controller, "1", "gosho", "Doctor");

            var result = await controller.DeleteNotification("2");
            
            Assert.True(result);
        }

        [Fact]
        public async Task GetMoreNotificationsShouldReturnJsonResult()
        {
            var user = new ApplicationUser()
            {
                Id = "1",
                UserName = "gosho"
            };
            
            var notificationService = new Mock<INotificationService>();
            var hub = new Mock<IHubContext<NotificationHub>>();
            hub.Setup(x => x.Clients.User(user.Id)
                    .SendCoreAsync(It.IsAny<string>(), It.IsAny<object?[]>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
            
            var userManager = new Mock<UserManager<ApplicationUser>>(Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);
            userManager.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(user);
            notificationService.Setup(x => x.GetUserNotifications("gosho", "1", 2, 0))
                .ReturnsAsync(new NotificationModel());

            var controller = new NotificationController(notificationService.Object, hub.Object, userManager.Object);
            ControllerExtensions.WithIdentity(controller, "1", "gosho", "Doctor");

            var result = await controller.GetMoreNotitification(0, 2);

            Assert.IsType<JsonResult>(result);
        }
    }
}
