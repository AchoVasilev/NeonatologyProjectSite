namespace Infrastructure.Hubs;

using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Services.NotificationService;
using Services.UserService;

[Authorize]
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
        var userId = this.Context.UserIdentifier;

        if (userId != null)
        {
            var targetUser = await this.userService.GetUserByIdAsync(userId);
            var notificationsCount = await this.notificationService.GetUserNotificationsCount(targetUser.UserName);

            await this.Clients.User(targetUser.Id).SendAsync("ReceiveNotification", notificationsCount, isFirstNotificationSound);
        }
    }
}