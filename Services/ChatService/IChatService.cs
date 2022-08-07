namespace Services.ChatService;

using System.Collections.Generic;
using System.Threading.Tasks;
using Common;
using Data.Models;

using Microsoft.AspNetCore.Http;

using ViewModels.Chat;

public interface IChatService : ITransientService
{
    Task AddUserToGroup(string groupName, string senderName, string receiverName);

    Task<string> SendMessageToUser(string senderName, string receiverName, string message, string groupName);

    Task<SendFilesResponseViewModel> SendMessageWitFilesToUser(IList<IFormFile> files, string group, string toUsername, string fromUsername, string message);

    Task<ICollection<Message>> ExtractAllMessages(string group);

    Task<ICollection<LoadMoreMessagesViewModel>> LoadMoreMessages(string group, int messagesSkipCount, ApplicationUser currentUser, string receiverFullname, string senderFullname);

    Task<ICollection<ChatConversationsViewModel>> GetAllMessages(string userId, int page, int itemsPerPage);

    Task<string> GetLastMessage(string currentUserId, string userId);

    Task<string> GetLastActivityAsync(string currentUserId, string userId);

    Task<string> GetGroupId(string currentUserId, string userId);

    Task<string> GetGroupName(string groupId);
}