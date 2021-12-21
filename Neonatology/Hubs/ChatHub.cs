namespace Neonatology.Hubs
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.SignalR;

    using Services.MessageService;

    using ViewModels.Chat;

    public class ChatHub : Hub
    {
        private readonly IMessageService messageService;

        public ChatHub(IMessageService messageService)
        {
            this.messageService = messageService;
        }

        public async Task SendMessage(string message, string receiverId)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                return;
            }

            var senderId = Context.UserIdentifier;

            await this.messageService.CreateMessageAsync(message, senderId, receiverId);
            await Clients.All.SendAsync(
                "ReceiveMessage",
                new ChatMessageWithUserViewModel
                {
                    SenderId = senderId,
                    Content = message
                });
        }
    }
}
