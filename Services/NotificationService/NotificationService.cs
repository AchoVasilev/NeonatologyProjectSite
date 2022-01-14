namespace Services.NotificationService
{
    using System;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    using Data;
    using Data.Models;
    using Data.Models.Enums;

    using Ganss.XSS;

    using Microsoft.EntityFrameworkCore;

    using ViewModels.Notification;

    using static Common.GlobalConstants.NotificationConstants;

    public class NotificationService : INotificationService
    {
        private readonly NeonatologyDbContext data;

        private const string url = "/Chat/WithUser/{0}";
        private const string notificationType = "Message";

        public NotificationService(NeonatologyDbContext data)
        {
            this.data = data;
        }

        public async Task<int> GetUserNotificationsCount(string receiverId)
            => await this.data.Notifications
                         .Where(x => x.ReceiverId == receiverId &&
                         x.NotificationStatus == NotificationStatus.Unread &&
                         x.IsDeleted == false)
                         .CountAsync();

        public async Task<string> AddMessageNotification(string message, string receiverUsername, string senderUsername)
        {
            var notificationTypeId = await this.data.NotificationTypes
                .Where(x => x.Name == notificationType)
                .Select(x => x.Id)
                .FirstOrDefaultAsync();

            var sender = await this.data.Users.FirstOrDefaultAsync(x => x.UserName == senderUsername);
            var receiver = await this.data.Users.FirstOrDefaultAsync(x => x.UserName == receiverUsername);

            var notification = new Notification
            {
                Sender = sender,
                Receiver = receiver,
                NotificationStatus = NotificationStatus.Unread,
                Link = string.Format(url, receiverUsername),
                Text = new HtmlSanitizer().Sanitize(message.Trim()),
                NotificationTypeId = notificationTypeId,
            };

            var receiverNotifications = await this.data.Notifications
                .Where(x => x.NotificationTypeId == notificationTypeId &&
                x.ReceiverId == receiver.Id &&
                x.SenderId == sender.Id &&
                x.IsDeleted == false)
                .OrderByDescending(x => x.CreatedOn)
                .ToListAsync();

            if (receiverNotifications.Count + 1 > MaxChatNotificationsPerUser)
            {
                receiverNotifications = receiverNotifications
                    .Skip(MaxChatNotificationsPerUser - 1)
                    .ToList();

                receiverNotifications.ForEach(x => x.IsDeleted = true);
            }

            await this.data.Notifications.AddAsync(notification);
            await this.data.SaveChangesAsync();

            return notification.Id;
        }

        public async Task<string> UpdateMessageNotifications(string senderUsername, string receiverUsername)
        {
            var sender = await this.data.Users.FirstOrDefaultAsync(x => x.UserName == senderUsername);
            var receiver = await this.data.Users.FirstOrDefaultAsync(x => x.UserName == receiverUsername);

            if (sender != null && receiver != null)
            {
                var notifications = this.data.Notifications
                    .Where(x => x.NotificationStatus == NotificationStatus.Unread &&
                    x.SenderId == sender.Id &&
                    x.ReceiverId == receiver.Id)
                    .ToList();

                foreach (var notification in notifications)
                {
                    await this.EditStatus(receiver, NotificationStatus.Read.ToString(), notification.Id);
                }

                return receiver.Id;
            }

            return string.Empty;
        }

        public async Task<bool> EditStatus(ApplicationUser currentUser, string newStatus, string id)
        {
            var notification = await this.data.Notifications
                .FirstOrDefaultAsync(x => x.Id == id && x.Receiver.UserName == currentUser.UserName);

            if (notification != null)
            {
                notification.NotificationStatus = (NotificationStatus)Enum.Parse(typeof(NotificationStatus), newStatus);
                this.data.Notifications.Update(notification);
                await this.data.SaveChangesAsync();

                return true;
            }

            return false;
        }

        public async Task<NotificationViewModel> GetNotificationById(string id)
        {
            var notification = await this.data.Notifications.FirstOrDefaultAsync(x => x.Id == id);

            var receiver = await this.data.Users.FirstOrDefaultAsync(x => x.Id == notification.ReceiverId);
            var sender = await this.data.Users.FirstOrDefaultAsync(x => x.Id == notification.SenderId);

            var item = ParseNotificationViewModel(notification, sender, receiver);

            return item;
        }

        private NotificationViewModel ParseNotificationViewModel(Notification notification, ApplicationUser sender, ApplicationUser receiver)
        {
            var contentWithoutTags =
                Regex.Replace(notification.Text, "<.*?>", string.Empty);

            return new NotificationViewModel
            {
                Id = notification.Id,
                CreatedOn = notification.CreatedOn.ToLocalTime().ToString("dd-MM-yyyy HH:mm"),
                Heading = this.GetNotificationHeading(notificationType, sender, notification.Link),
                Status = notification.NotificationStatus,
                Text = contentWithoutTags.Length < 487 ?
                                contentWithoutTags :
                                $"{contentWithoutTags.Substring(0, 487)}...",
                TargetUsername = receiver.UserName,
                AllStatuses = Enum.GetValues(typeof(NotificationStatus)).Cast<NotificationStatus>().Select(x => x.ToString()).ToList(),
            };
        }

        private string GetNotificationHeading(string notificationTypeName, ApplicationUser user, string link)
        {
            string message = string.Empty;

            switch (notificationTypeName)
            {
                case "Message":
                    message =
                        $"<a href=\"/Profile/{user.UserName}\" style=\"text-decoration: underline\">{user.UserName}</a> ви изпрати ново <a href=\"{link}\" style=\"text-decoration: underline\">съобщение</a>.";
                    break;

                case "RateProfile":
                    message =
                        $"<a href=\"/Profile/{user.UserName}\" style=\"text-decoration: underline\">{user.UserName}</a> оцени вашия <a href=\"{link}\" style=\"text-decoration: underline\">профил</a>.";
                    break;

                case "Banned Profile":
                    message =
                        $"<a href=\"/Profile/{user.UserName}\" style=\"text-decoration: underline\">{user.UserName}</a> банна <a href=\"{link}\" style=\"text-decoration: underline\">вашия профил</a>.";
                    break;

                default:
                    break;
            }

            return message;
        }
    }
}
