namespace Services.NotificationService;

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

using static global::Common.Constants.GlobalConstants.DateTimeFormats;
using static global::Common.Constants.GlobalConstants.NotificationConstants;

public class NotificationService : INotificationService
{
    private readonly NeonatologyDbContext data;

    private const string url = "/Chat/With/{0}/Group/{1}";
    private const string notificationType = "Message";

    public NotificationService(NeonatologyDbContext data)
    {
        this.data = data;
    }

    public async Task<int> GetUserNotificationsCount(string receiverUsername)
        => await this.data.Notifications
            .Where(x => x.Receiver.UserName == receiverUsername &&
                        x.NotificationStatus == NotificationStatus.Непрочетено &&
                        x.IsDeleted == false)
            .CountAsync();

    public async Task<string> AddMessageNotification(string message, string receiverUsername, string senderUsername, string group)
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
            NotificationStatus = NotificationStatus.Непрочетено,
            Link = string.Format(url, senderUsername, group),
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

            receiverNotifications.ForEach(x =>
            {
                x.IsDeleted = true;
                x.DeletedOn = DateTime.UtcNow;
            });
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
                .Where(x => x.NotificationStatus == NotificationStatus.Прочетено &&
                            x.SenderId == sender.Id &&
                            x.ReceiverId == receiver.Id &&
                            x.IsDeleted == false)
                .ToList();

            foreach (var notification in notifications)
            {
                await this.EditStatus(receiver.Id, NotificationStatus.Прочетено.ToString(), notification.Id);
            }

            return receiver.Id;
        }

        return string.Empty;
    }

    public async Task<bool> EditStatus(string receiverId, string newStatus, string id)
    {
        var notification = await this.data.Notifications
            .FirstOrDefaultAsync(x => x.Id == id && 
                                      x.ReceiverId == receiverId && 
                                      x.IsDeleted == false);

        if (notification != null)
        {
            notification.NotificationStatus = (NotificationStatus)Enum.Parse(typeof(NotificationStatus), newStatus);
            notification.ModifiedOn = DateTime.UtcNow;

            await this.data.SaveChangesAsync();

            return true;
        }

        return false;
    }

    public async Task<bool> DeleteNotification(string receiverId, string id)
    {
        var notification = await this.data.Notifications
            .Where(x => x.Id == id && x.ReceiverId == receiverId && x.IsDeleted == false)
            .FirstOrDefaultAsync();

        if (notification == null)
        {
            return false;
        }

        notification.IsDeleted = true;
        notification.DeletedOn = DateTime.UtcNow;

        await this.data.SaveChangesAsync();

        return true;
    }

    public async Task<NotificationViewModel> GetNotificationById(string id)
    {
        var notification = await this.data.Notifications
            .FirstOrDefaultAsync(x => x.Id == id && x.IsDeleted == false);

        var receiver = await this.data.Users
            .FirstOrDefaultAsync(x => x.Id == notification.ReceiverId);

        var sender = await this.data.Users
            .Where(x => x.Id == notification.SenderId)
            .Include(x => x.Image)
            .FirstOrDefaultAsync();

        var item = this.ParseNotificationViewModel(notification, sender, receiver);

        return item;
    }

    public async Task<NotificationModel> GetUserNotifications(string currentUserUsername, string currentUserId, int notificationCount, int skip)
    {
        var userNotifications = await this.data.Notifications
            .Where(x => x.ReceiverId == currentUserId && x.IsDeleted == false)
            .OrderByDescending(x => x.CreatedOn)
            .Skip(skip)
            .Take(notificationCount)
            .AsNoTracking()
            .ToListAsync();

        var notificationModel = new NotificationModel();

        foreach (var userNotification in userNotifications)
        {
            var sender = await this.data.Users
                .Where(x => x.Id == userNotification.SenderId)
                .Include(x => x.Image)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            var receiver = await this.data.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == userNotification.ReceiverId);

            var notificationViewModel = this.ParseNotificationViewModel(userNotification, sender, receiver);
            notificationModel.Notifications.Add(notificationViewModel);
        }

        var notificationsCount = await this.GetUserNotificationsCount(currentUserUsername);
        var isLessThanDefaultCount = notificationsCount > skip + notificationsCount;
        notificationModel.IsLessThanDefaultCount = isLessThanDefaultCount;

        return notificationModel;
    }

    private NotificationViewModel ParseNotificationViewModel(Notification notification, ApplicationUser sender, ApplicationUser receiver)
    {
        var contentWithoutTags =
            Regex.Replace(notification.Text, "<.*?>", string.Empty);

        return new NotificationViewModel
        {
            Id = notification.Id,
            CreatedOn = notification.CreatedOn.ToLocalTime().ToString(DateTimeFormat),
            Heading = this.GetNotificationHeading(notificationType, sender, notification.Link),
            Status = notification.NotificationStatus,
            Text = contentWithoutTags.Length < 487 ?
                contentWithoutTags :
                $"{contentWithoutTags[..487]}...",
            TargetUsername = receiver.UserName,
            ImageUrl = sender.Image.Url,
            AllStatuses = Enum.GetValues(typeof(NotificationStatus)).Cast<NotificationStatus>().Select(x => x.ToString()).ToList(),
        };
    }

    private string GetNotificationHeading(string notificationTypeName, ApplicationUser user, string link)
    {
        var message = string.Empty;

        switch (notificationTypeName)
        {
            case "Message":
                message =
                    $"<a href=\"/Profile/{user.Id}\" style=\"text-decoration: underline\">{user.UserName}</a> ви изпрати ново <a href=\"{link}\" style=\"text-decoration: underline\">съобщение</a>.";
                break;

            case "RateProfile":
                message =
                    $"<a href=\"/Profile/{user.Id}\" style=\"text-decoration: underline\">{user.UserName}</a> оцени вашия <a href=\"{link}\" style=\"text-decoration: underline\">профил</a>.";
                break;

            case "Banned Profile":
                message =
                    $"<a href=\"/Profile/{user.Id}\" style=\"text-decoration: underline\">{user.UserName}</a> банна <a href=\"{link}\" style=\"text-decoration: underline\">вашия профил</a>.";
                break;

            case "Paid":
                message =
                    $"<a href=\"/Profile/{user.Id}\" style=\"text-decoration: underline\">{user.UserName}</a> заплати <a href=\"{link}\" style=\"text-decoration: underline\">за консултация</a>.";
                break;
            default:
                break;
        }

        return message;
    }
}