namespace Services.ChatService;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutoMapper;
using AutoMapper.QueryableExtensions;

using CloudinaryDotNet;

using Data;
using Data.Models;

using Ganss.XSS;

using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

using FileService;
using FileService.FileServiceModels;

using ViewModels.Chat;

using static global::Common.Constants.GlobalConstants.FileConstants;
using static global::Common.Constants.GlobalConstants.ChatConstants;
using static global::Common.Constants.GlobalConstants.DateTimeFormats;

public class ChatService : IChatService
{
    private readonly NeonatologyDbContext data;
    private readonly IMapper mapper;
    private readonly IFileService imageService;
    private readonly Cloudinary cloudinary;

    public ChatService(NeonatologyDbContext data, IMapper mapper, IFileService imageService, Cloudinary cloudinary)
    {
        this.data = data;
        this.mapper = mapper;
        this.imageService = imageService;
        this.cloudinary = cloudinary;
    }

    public async Task<string> SendMessageToUser(string senderName, string receiverName, string message, string groupName)
    {
        var receiver = await this.data.Users
            .FirstOrDefaultAsync(x => x.UserName == receiverName && x.IsDeleted == false);

        var sender = await this.data.Users
            .FirstOrDefaultAsync(x => x.UserName == senderName && x.IsDeleted == false);

        if (receiver is null || sender is null)
        {
            return null;
        }

        var group = await this.data.Groups
            .FirstOrDefaultAsync(x => x.Name.ToLower() == groupName.ToLower());

        var chatMessage = new Message
        {
            Sender = sender,
            Receiver = receiver,
            Group = group,
            Content = new HtmlSanitizer().Sanitize(message),
        };

        await this.data.Messages.AddAsync(chatMessage);
        await this.data.SaveChangesAsync();

        return receiver.Id;
    }

    public async Task AddUserToGroup(string groupName, string senderName, string receiverName)
    {
        var sender = await this.data.Users
            .FirstOrDefaultAsync(x => x.UserName == senderName);
        var receiver = await this.data.Users
            .FirstOrDefaultAsync(x => x.UserName == receiverName);
        var targetGroup = await this.data.Groups
            .FirstOrDefaultAsync(x => x.Name.ToLower() == groupName.ToLower());

        if (targetGroup == null)
        {
            targetGroup = new Group
            {
                Name = groupName
            };

            var targetToUser = new UserGroup
            {
                ApplicationUser = receiver,
                Group = targetGroup,
            };

            var targetFromUser = new UserGroup
            {
                ApplicationUser = sender,
                Group = targetGroup,
            };

            targetGroup.UserGroups.Add(targetToUser);
            targetGroup.UserGroups.Add(targetFromUser);

            await this.data.Groups.AddAsync(targetGroup);
            await this.data.SaveChangesAsync();
        }
    }

    public async Task<ICollection<Message>> ExtractAllMessages(string group)
    {
        var targetGroup = await this.data.Groups.FirstOrDefaultAsync(x => x.Name.ToLower() == group.ToLower());

        if (targetGroup != null)
        {
            var messages = this.data.Messages
                .Where(x => x.GroupId == targetGroup.Id && x.IsDeleted == false)
                .OrderByDescending(x => x.CreatedOn)
                .Take(MessagesCountPerScroll)
                .OrderBy(x => x.CreatedOn)
                .ToList();

            foreach (var message in messages)
            {
                message.Sender = await this.data.Users
                    .FirstOrDefaultAsync(x => x.Id == message.SenderId);

                message.Receiver = await this.data.Users
                    .FirstOrDefaultAsync(x => x.Id == message.ReceiverId);
            }

            return messages;
        }

        return new List<Message>();
    }

    public async Task<ICollection<LoadMoreMessagesViewModel>> LoadMoreMessages(string group, int messagesSkipCount, ApplicationUser currentUser, string receiverFullname, string senderFullname)
    {
        var result = new List<LoadMoreMessagesViewModel>();

        var targetGroup = await this.data.Groups.FirstOrDefaultAsync(x => x.Name.ToLower() == group.ToLower());

        if (targetGroup != null)
        {
            var messages = this.data.Messages
                .Where(x => x.GroupId == targetGroup.Id && x.IsDeleted == false)
                .OrderByDescending(x => x.CreatedOn)
                .Skip(messagesSkipCount)
                .Take(MessagesCountPerScroll)
                .ToList();

            foreach (var message in messages)
            {
                var currentMessageModel = new LoadMoreMessagesViewModel
                {
                    Id = message.Id,
                    Content = message.Content,
                    SendedOn = message.CreatedOn.ToString(DateTimeFormat),
                    CurrentUsername = currentUser.UserName,
                };

                // var messageFromUser = await this.data.Users
                //     .FirstOrDefaultAsync(x => x.Id == message.SenderId);

                currentMessageModel.FromUsername = senderFullname;
                currentMessageModel.ReceiverUsername = receiverFullname;

                result.Add(currentMessageModel);
            }
        }

        return result;
    }

    public async Task<ICollection<ChatConversationsViewModel>> GetAllMessages(string userId, int page, int itemsPerPage)
    {
        var sentMessages = this.data.Messages
            .Where(x => x.IsDeleted == false && (x.SenderId == userId || x.ReceiverId == userId))
            .OrderByDescending(x => x.CreatedOn)
            .Include(x => x.Sender.Image)
            .Select(x => x.Sender)
            .AsQueryable();

        var receivedMessages = this.data.Messages
            .Where(x => x.IsDeleted == false && (x.SenderId == userId || x.ReceiverId == userId))
            .OrderByDescending(x => x.CreatedOn)
            .Include(x => x.Receiver.Image)
            .Select(x => x.Receiver)
            .AsQueryable();

        var concatMessages = await sentMessages
            .Concat(receivedMessages)
            .Where(x => x.Id != userId)
            .Include(x => x.Image)
            .Distinct()
            .OrderBy(d => d.Id)
            .Skip((page - 1) * itemsPerPage)
            .Take(itemsPerPage)
            .ProjectTo<ChatConversationsViewModel>(this.mapper.ConfigurationProvider)
            .ToListAsync();

        return concatMessages;
    }

    public async Task<string> GetLastMessage(string currentUserId, string userId)
        => await this.data.Messages
            .Where(m => m.IsDeleted == false &&
                        ((m.ReceiverId == currentUserId && m.SenderId == userId) ||
                         m.ReceiverId == userId && m.SenderId == currentUserId))
            .OrderByDescending(x => x.CreatedOn)
            .Select(x => x.Content)
            .FirstOrDefaultAsync();

    public async Task<string> GetLastActivityAsync(string currentUserId, string userId)
        => await this.data.Messages
            .Where(m => !m.IsDeleted &&
                        ((m.ReceiverId == currentUserId && m.SenderId == userId) ||
                         (m.ReceiverId == userId && m.SenderId == currentUserId)))
            .OrderByDescending(m => m.CreatedOn)
            .Select(m => m.CreatedOn.ToString(DateTimeFormat))
            .FirstOrDefaultAsync();

    public async Task<string> GetGroupId(string currentUserId, string userId)
        => await this.data.Messages
            .Where(m => !m.IsDeleted &&
                        ((m.ReceiverId == currentUserId && m.SenderId == userId) ||
                         (m.ReceiverId == userId && m.SenderId == currentUserId)))
            .Select(x => x.GroupId)
            .FirstOrDefaultAsync();

    public async Task<string> GetGroupName(string groupId)
        => await this.data.Groups
            .Where(x => x.Id == groupId)
            .Select(x => x.Name)
            .FirstOrDefaultAsync();

    //public async Task<bool> IsUserAbleToChat(string username, string group, ApplicationUser currentUser)
    //{
    //    var targetUser = await this.data.Users.FirstOrDefaultAsync(x => x.UserName == username);
    //    var groupUsers = new List<string>() { currentUser.UserName, targetUser.UserName };
    //    var targetGroupName = string.Join(GlobalConstants.ChatGroupNameSeparator, groupUsers.OrderBy(x => x));

    //    if (targetGroupName != group)
    //    {
    //        return false;
    //    }

    //    //if (await this.userManager.IsInRoleAsync(currentUser, GlobalConstants.AdministratorRole))
    //    //{
    //    //    return true;
    //    //}

    //    if (currentUser.UserName == username)
    //    {
    //        return false;
    //    }

    //    return true;
    //}

    public async Task<SendFilesResponseViewModel> SendMessageWitFilesToUser(IList<IFormFile> files, string group, string toUsername, string fromUsername, string message)
    {
        var toUser = this.data.Users.FirstOrDefault(x => x.UserName == toUsername);
        var toId = toUser.Id;

        var fromUser = this.data.Users.FirstOrDefault(x => x.UserName == fromUsername);
        var fromId = fromUser.Id;

        await this.DeleteOldMessage(group);

        var newMessage = new Message
        {
            Sender = fromUser,
            Group = this.data.Groups.FirstOrDefault(x => x.Name.ToLower() == group.ToLower()),
            Receiver = toUser,
        };

        var messageContent = new StringBuilder();

        if (message != null)
        {
            messageContent.AppendLine($"{new HtmlSanitizer().Sanitize(message.Trim())}<hr style=\"margin-bottom: 8px !important;\" />");
        }

        var imagesContent = new StringBuilder();
        var filesContent = new StringBuilder();

        var imagesCount = files
            .Where(x => x.ContentType
                .Contains("image", StringComparison.CurrentCultureIgnoreCase))
            .Count();

        var result = new SendFilesResponseViewModel();

        if (imagesCount > 0)
        {
            result.HaveImages = true;
        }

        var filesCount = files
            .Where(x => !x.ContentType
                .Contains("image", StringComparison.CurrentCultureIgnoreCase))
            .Count();

        if (filesCount > 0)
        {
            result.HaveFiles = true;
        }

        foreach (var file in files)
        {
            var chatFile = new ChatImage
            {
                MessageId = newMessage.Id,
                GroupId = this.data.Groups.FirstOrDefault(x => x.Name.ToLower() == group.ToLower()).Id,
            };

            IFileServiceModel fileModel;

            if (file.ContentType.Contains("image", StringComparison.CurrentCultureIgnoreCase))
            {
                fileModel = await this.imageService.UploadImage(
                    this.cloudinary,
                    file,
                    ChatFolderName);

                chatFile.Name = string.Format(ChatFileName, chatFile.Id);

                imagesContent.AppendLine($"<span onclick=\"zoomChatImage('{fileModel.Uri}')\"><img src=\"{fileModel.Uri}\" style=\"margin-right: 10px; width: 27px; height: 35px; margin-top: 5px;\"></span>");
            }
            else
            {
                var fileExtension = Path.GetExtension(file.FileName);

                fileModel = await this.imageService.UploadFile(
                    this.cloudinary,
                    file,
                    ChatFolderName);

                chatFile.Name = string.Format(ChatFileName, $"{chatFile.Id}{fileExtension}");

                string[] sizes = { "B", "KB", "MB", "GB", "TB" };
                double fileLength = file.Length;
                int order = 0;
                while (fileLength >= 1024 && order < sizes.Length - 1)
                {
                    order++;
                    fileLength /= 1024;
                }

                string fileSize = string.Format("{0:0.##} {1}", fileLength, sizes[order]);

                filesContent.AppendLine($"<p><a href=\"{fileModel.Uri}\"><i class=\"fas fa-download\"></i> {file.FileName} - ({fileSize})</a></p>");
            }

            chatFile.Url = fileModel.Uri;
            newMessage.ChatImages.Add(chatFile);
        }

        if (imagesContent.Length == 0)
        {
            messageContent.AppendLine(filesContent.ToString().Trim());
        }
        else
        {
            messageContent.AppendLine(imagesContent.ToString().Trim());

            if (filesContent.Length != 0)
            {
                messageContent.AppendLine("<hr style=\"margin-bottom: 8px !important;\" />");
                messageContent.AppendLine(filesContent.ToString().Trim());
            }
        }

        newMessage.Content = messageContent.ToString().Trim();
        result.MessageContent = newMessage.Content;

        this.data.Messages.Add(newMessage);
        await this.data.SaveChangesAsync();

        result.SenderId = fromId;
        result.ReceiverId = toId;

        return result;
    }

    private async Task DeleteOldMessage(string group)
    {
        var targetGroup = await this.data.Groups.FirstOrDefaultAsync(x => x.Name.ToLower() == group.ToLower());

        if (targetGroup != null)
        {
            var messages = this.data.Messages
                .Where(x => x.GroupId == targetGroup.Id)
                .OrderBy(x => x.CreatedOn)
                .ToList();

            if (messages.Count > SavedChatMessagesCount)
            {
                var oldMessages = messages.Take(messages.Count - SavedChatMessagesCount);

                foreach (var oldMessage in oldMessages)
                {
                    var oldImages = this.data.ChatImages.Where(x => x.MessageId == oldMessage.Id).ToList();

                    foreach (var oldImage in oldImages)
                    {
                        await this.imageService.DeleteFile(
                            this.cloudinary,
                            oldImage.Name,
                            ChatFolderName);

                        oldImage.IsDeleted = true;
                        oldImage.DeletedOn = DateTime.UtcNow;
                    }

                    oldMessage.IsDeleted = true;
                    oldMessage.DeletedOn = DateTime.UtcNow;
                }

                await this.data.SaveChangesAsync();
            }
        }
    }
}