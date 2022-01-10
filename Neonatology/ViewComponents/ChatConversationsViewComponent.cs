namespace Neonatology.ViewComponents
{
    using System.Threading.Tasks;

    using Infrastructure;

    using Microsoft.AspNetCore.Mvc;

    using Services.ChatService;

    [ViewComponent(Name = "ChatConversations")]
    public class ChatConversationsViewComponent : ViewComponent
    {
        private readonly IChatService messageService;

        public ChatConversationsViewComponent(IChatService messageService)
        {
            this.messageService = messageService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var currentUserId = UserClaimsPrincipal.GetId();
            string converstations = null;//await this.messageService.GetAllMessages(currentUserId);

            //foreach (var user in converstations)
            //{
            //    user.LastMessage = await this.messageService.GetLastMessage(currentUserId, user.Id);
            //    user.LastMessageActivity = await this.messageService.GetLastActivityAsync(currentUserId, user.Id);
            //}

            return View(converstations);

        }
    }
}
