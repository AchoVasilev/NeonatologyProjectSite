namespace Neonatology.Hubs
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.SignalR;

    using Services.NotificationService;
    using Services.UserService;

    public class NotificationHub : Hub
    {
        private readonly IUserService userService;
        private readonly INotificationService notificationService;

        public NotificationHub(IUserService userService, INotificationService notificationService)
        {
            this.userService = userService;
            this.notificationService = notificationService;
        }

        public async Task GetUserNotificationCount(bool isFirstNotificationSound)
        {
            var username = this.Context.User.Identity.Name;

            if (username != null)
            {
                var targetUser = await this.userService.GetUserByUsernameAsync(username);
                var notificationsCount = await this.notificationService.GetUserNotificationsCount(username);

                await this.Clients.User(targetUser.Id).SendAsync("ReceiveNotification", notificationsCount, isFirstNotificationSound);
            }
        }
    }
}