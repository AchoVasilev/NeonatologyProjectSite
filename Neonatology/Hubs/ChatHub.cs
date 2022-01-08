namespace Neonatology.Hubs
{
    using System.Threading.Tasks;

    using Ganss.XSS;

    using Microsoft.AspNetCore.SignalR;

    using Services.MessageService;
    using Services.NotificationService;
    using Services.UserService;

    using ViewModels.Chat;

    public class ChatHub : Hub
    {
        private readonly IMessageService messageService;
        private readonly INotificationService notificationService;

        public ChatHub(IMessageService messageService, INotificationService notificationService)
        {
            this.messageService = messageService;
            this.notificationService = notificationService;
        }

        public async Task SendMessage(string message, string receiverId)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                return;
            }

            var senderId = Context.UserIdentifier;

            await this.messageService.CreateMessageAsync(message, senderId, receiverId);

            var notificationId = await this.notificationService.AddMessageNotification(message, receiverId, senderId);

            await Clients.All.SendAsync(
                "ReceiveMessage",
                new ChatMessageWithUserViewModel
                {
                    SenderId = senderId,
                    Content = new HtmlSanitizer().Sanitize(message.Trim())
                });
        }
    }
}
