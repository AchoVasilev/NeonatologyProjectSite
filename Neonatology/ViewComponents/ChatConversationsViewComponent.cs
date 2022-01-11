namespace Neonatology.ViewComponents
{
    using System.Threading.Tasks;

    using Infrastructure;

    using Microsoft.AspNetCore.Mvc;

    using Services.ChatService;

    [ViewComponent(Name = "ChatConversations")]
    public class ChatConversationsViewComponent : ViewComponent
    {
        private readonly IChatService chatService;

        public ChatConversationsViewComponent(IChatService chatService)
        {
            this.chatService = chatService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var currentUserId = UserClaimsPrincipal.GetId();
            var converstations = await this.chatService.GetAllMessages(currentUserId);

            foreach (var user in converstations)
            {
                user.LastMessage = await this.chatService.GetLastMessage(currentUserId, user.Id);
                user.LastMessageActivity = await this.chatService.GetLastActivityAsync(currentUserId, user.Id);

                var groupId = await this.chatService.GetGroupId(currentUserId, user.Id);
                user.GroupName = await this.chatService.GetGroupName(groupId);
            }

            return View(converstations);

        }
    }
}
