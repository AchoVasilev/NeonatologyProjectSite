namespace Neonatology.Hubs
{
    using System.Linq;
    using System.Threading.Tasks;

    using Data.Models;

    using Ganss.XSS;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.SignalR;
    using Microsoft.EntityFrameworkCore;

    using Services.ChatService;
    using Services.NotificationService;

    [Authorize]
    public class ChatHub : Hub
    {
        private readonly IChatService chatService;
        private readonly INotificationService notificationService;
        private readonly IHubContext<NotificationHub> notificationHub;
        private readonly UserManager<ApplicationUser> userManager;

        public ChatHub(
            IChatService chatService,
            INotificationService notificationService,
            IHubContext<NotificationHub> notificationHub, 
            UserManager<ApplicationUser> userManager)
        {
            this.chatService = chatService;
            this.notificationService = notificationService;
            this.notificationHub = notificationHub;
            this.userManager = userManager;
        }

        public async Task AddToGroup(string groupName, string receiverName, string senderName, string senderFullName)
        {
            await this.Groups.AddToGroupAsync(this.Context.ConnectionId, groupName);
            await this.chatService.AddUserToGroup(groupName, senderName, receiverName);

            await this.Clients.Group(groupName).SendAsync("ReceiveMessage", senderFullName, $"{senderFullName} се присъдени към група {groupName}");
        }

        public async Task SendMessage(string senderUsername, string receiverUsername, string message, string group, string senderFullName)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                return;
            }

            var receiverId = await this.chatService
                .SendMessageToUser(senderUsername, receiverUsername, message, group);

            await this.Clients.User(receiverId)
                .SendAsync("ReceiveMessage", senderFullName, new HtmlSanitizer().Sanitize(message.Trim()));

            var notificationId = await this.notificationService
                .AddMessageNotification(message, receiverUsername, senderUsername, group);

            var count = await this.notificationService
                .GetUserNotificationsCount(receiverId);

            await this.notificationHub
                .Clients
                .User(receiverId)
                .SendAsync("ReceiveNotification", count, true);

            var notification = await this.notificationService.GetNotificationById(notificationId);

            await this.notificationHub.Clients.User(receiverId)
                .SendAsync("VisualizeNotification", notification);
        }

        public async Task ReceiveMessage(string senderUsername, string message, string group, string senderFullName)
        {
            var user = await this.userManager.Users.FirstOrDefaultAsync(x => x.UserName == senderUsername);
            await this.Clients.User(user.Id).SendAsync("SendMessage", senderUsername, new HtmlSanitizer().Sanitize(message.Trim()));
        }

        public async Task UpdateMessageNotifications(string fromUsername, string username)
        {
            var toId = await this.notificationService.UpdateMessageNotifications(fromUsername, username);

            if (toId != string.Empty)
            {
                var count = await this.notificationService.GetUserNotificationsCount(username);
                await this.notificationHub
                    .Clients
                    .User(toId)
                    .SendAsync("ReceiveNotification", count, false);
            }
        }
    }
}