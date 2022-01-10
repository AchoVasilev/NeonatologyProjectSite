namespace Services.ChatService
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Common;

    using Data;
    using Data.Models;

    using Ganss.XSS;

    using Microsoft.EntityFrameworkCore;

    using ViewModels.Chat;

    using static Common.GlobalConstants;

    public class ChatService : IChatService
    {
        private readonly NeonatologyDbContext data;

        public ChatService(NeonatologyDbContext data)
        {
            this.data = data;
        }

        public async Task<string> SendMessageToUser(string senderName, string receiverName, string message, string groupName)
        {
            var receiverId = await this.data.Users
                .Where(x => x.UserName == receiverName)
                .Select(x => x.Id)
                .FirstOrDefaultAsync();

            var sender = await this.data.Users
                .FirstOrDefaultAsync(x => x.UserName == senderName);
            var group = await this.data.Groups
                .FirstOrDefaultAsync(x => x.Name.ToLower() == groupName.ToLower());

            var chatMessage = new Message
            {
                Sender = sender,
                ReceiverId = receiverId,
                Group = group,
                Content = new HtmlSanitizer().Sanitize(message),
            };

            await this.data.Messages.AddAsync(chatMessage);
            await this.data.SaveChangesAsync();

            return receiverId;
        }

        public async Task AddUserToGroup(string groupName, string senderName, string receiverName)
        {
            var args = groupName.Split(ChatGroupNameSeparator, System.StringSplitOptions.RemoveEmptyEntries);

            var sender = await this.data.Users.FirstOrDefaultAsync(x => x.UserName == args[0]);
            var receiver = await this.data.Users.FirstOrDefaultAsync(x => x.UserName == args[1]);
            var targetGroup = await this.data.Groups.FirstOrDefaultAsync(x => x.Name.ToLower() == groupName.ToLower());

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
                    .Where(x => x.GroupId == targetGroup.Id)
                    .OrderByDescending(x => x.CreatedOn)
                    .Take(GlobalConstants.MessagesCountPerScroll)
                    .OrderBy(x => x.CreatedOn)
                    .ToList();

                foreach (var message in messages)
                {
                    message.Sender = await this.data.Users
                        .FirstOrDefaultAsync(x => x.Id == message.SenderId);
                }

                return messages;
            }

            return new List<Message>();
        }

        public async Task<ICollection<LoadMoreMessagesViewModel>> LoadMoreMessages(string group, int messagesSkipCount, ApplicationUser currentUser)
        {
            var result = new List<LoadMoreMessagesViewModel>();

            var targetGroup = await this.data.Groups.FirstOrDefaultAsync(x => x.Name.ToLower() == group.ToLower());

            if (targetGroup != null)
            {
                var messages = this.data.Messages
                    .Where(x => x.GroupId == targetGroup.Id)
                    .OrderByDescending(x => x.CreatedOn)
                    .Skip(messagesSkipCount)
                    .Take(GlobalConstants.MessagesCountPerScroll)
                    .ToList();

                foreach (var message in messages)
                {
                    var currentMessageModel = new LoadMoreMessagesViewModel
                    {
                        Id = message.Id,
                        Content = message.Content,
                        SendedOn = message.CreatedOn.ToLocalTime().ToString("dd/mm/yyyy hh:mm:ss tt"),
                        CurrentUsername = currentUser.UserName,
                    };

                    var messageFromUser = await this.data.Users
                        .FirstOrDefaultAsync(x => x.Id == message.SenderId);

                    currentMessageModel.FromUsername = messageFromUser.UserName;

                    result.Add(currentMessageModel);
                }
            }

            return result;
        }

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
    }
}
