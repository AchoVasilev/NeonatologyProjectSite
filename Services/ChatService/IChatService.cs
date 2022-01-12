namespace Services.ChatService
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Data.Models;

    using ViewModels.Chat;

    public interface IChatService
    {
        Task AddUserToGroup(string groupName, string senderName, string receiverName);

        Task<string> SendMessageToUser(string senderName, string receiverName, string message, string groupName);

        Task<ICollection<Message>> ExtractAllMessages(string group);

        Task<ICollection<LoadMoreMessagesViewModel>> LoadMoreMessages(string group, int messagesSkipCount, ApplicationUser currentUser, string receiverFullname, string senderFullname);

        Task<ICollection<ChatConversationsViewModel>> GetAllMessages(string userId);

        Task<string> GetLastMessage(string currentUserId, string userId);

        Task<string> GetLastActivityAsync(string currentUserId, string userId);

        Task<string> GetGroupId(string currentUserId, string userId);

        Task<string> GetGroupName(string groupId);
    }
}
