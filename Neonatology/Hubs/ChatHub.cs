namespace Neonatology.Hubs
{
    using System.Threading.Tasks;

    using Ganss.XSS;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.SignalR;

    using Services.ChatService;
    using Services.NotificationService;
    using Services.UserService;

    [Authorize]
    public class ChatHub : Hub
    {
        private readonly IChatService chatService;
        private readonly INotificationService notificationService;
        private readonly IHubContext<NotificationHub> notificationHub;
        private readonly IUserService userService;

        public ChatHub(
            IChatService chatService,
            INotificationService notificationService,
            IHubContext<NotificationHub> notificationHub, 
            IUserService userService)
        {
            this.chatService = chatService;
            this.notificationService = notificationService;
            this.notificationHub = notificationHub;
            this.userService = userService;
        }

        public async Task AddToGroup(string groupName, string receiverName, string senderName, string senderFullName)
        {
            await this.Groups.AddToGroupAsync(this.Context.ConnectionId, groupName);
            await this.chatService.AddUserToGroup(groupName, senderName, receiverName);

            await this.Clients.Group(groupName).SendAsync("ReceiveMessage", senderName, $"{senderFullName} се присъдени към група {groupName}");
        }

        public async Task SendMessage(string senderUsername, string receiverUsername, string message, string group, string senderFullName)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                return;
            }

            var receiverId = await this.chatService
                .SendMessageToUser(senderUsername, receiverUsername, message, group);
            var sender = await this.userService.FindByUserNameAsync(senderUsername);

            await this.Clients.User(receiverId)
                .SendAsync("ReceiveMessage", senderFullName, new HtmlSanitizer().Sanitize(message.Trim()), sender.Image.Url);

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
            var user = await this.userService.FindByUserNameAsync(senderUsername);
            await this.Clients.User(user.Id).SendAsync("SendMessage", senderUsername, new HtmlSanitizer().Sanitize(message.Trim()), user.Image.Url);
        }

        public async Task UpdateMessageNotifications(string fromUsername, string receiverUsername)
        {
            var toId = await this.notificationService.UpdateMessageNotifications(fromUsername, receiverUsername);

            if (toId != string.Empty)
            {
                var count = await this.notificationService.GetUserNotificationsCount(receiverUsername);
                await this.notificationHub
                    .Clients
                    .User(toId)
                    .SendAsync("ReceiveNotification", count, false);
            }
        }
    }
}