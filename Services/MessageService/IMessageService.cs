namespace Services.MessageService
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using ViewModels.Chat;

    public interface IMessageService
    {
        Task CreateMessageAsync(string content, string senderId, string receiverId);

        Task<ICollection<ChatConversationsViewModel>> GetAllMessages(string userId);

        Task<string> GetLastMessage(string currentUserId, string userId);

        Task<string> GetLastActivityAsync(string currentUserId, string userId);

        Task<IEnumerable<ChatMessageWithUserViewModel>> GetAllWithUserAsync(string currentUserId, string userId);
    }
}
