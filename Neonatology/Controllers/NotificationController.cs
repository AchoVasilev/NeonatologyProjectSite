namespace Neonatology.Controllers
{
    using System.Threading.Tasks;

    using Data.Models;

    using Infrastructure;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.SignalR;

    using Neonatology.Hubs;

    using Services.NotificationService;

    using static Common.GlobalConstants.NotificationConstants;

    [Authorize]
    public class NotificationController : BaseController
    {
        private readonly INotificationService notificationService;
        private readonly IHubContext<NotificationHub> notificationHub;
        private readonly UserManager<ApplicationUser> userManager;

        public NotificationController(
            INotificationService notificationService, 
            IHubContext<NotificationHub> notificationHub, 
            UserManager<ApplicationUser> userManager)
        {
            this.notificationService = notificationService;
            this.notificationHub = notificationHub;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var userName = this.User.Identity.Name;
            var userId = this.User.GetId();

            var model = await this.notificationService.GetUserNotifications(userName, userId, NotificationOnClick, 0);

            return View(model);
        }

        [HttpPost]
        [Route("/Notification/EditStatus")]
        public async Task<bool> EditStatus(string newStatus, string id)
        {
            var user = await this.userManager.GetUserAsync(this.User);
            var isEdited = await this.notificationService.EditStatus(user.Id, newStatus, id);
            await this.ChangeNotificationCounter(isEdited, user);

            return isEdited;
        }

        [HttpPost]
        [Route("/Notification/DeleteNotification")]
        public async Task<bool> DeleteNotification(string id)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);
            bool isDeleted = await this.notificationService.DeleteNotification(currentUser.Id, id);

            await this.ChangeNotificationCounter(isDeleted, currentUser);

            return isDeleted;
        }

        [HttpGet]
        [Route("/Notification/GetMoreNotitifications")]
        public async Task<IActionResult> GetMoreNotitification(int skip, int take)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);
            var result = await this.notificationService
                .GetUserNotifications(currentUser.UserName, currentUser.Id, take, skip);

            return new JsonResult(new { newNotifications = result.Notifications, hasMore = result.IsLessThanDefaultCount });
        }

        private async Task ChangeNotificationCounter(bool isForChange, ApplicationUser user)
        {
            if (isForChange)
            {
                int count = await this.notificationService.GetUserNotificationsCount(user.UserName);
                await this.notificationHub.Clients.User(user.Id).SendAsync("ReceiveNotification", count, false);
            }
        }
    }
}
