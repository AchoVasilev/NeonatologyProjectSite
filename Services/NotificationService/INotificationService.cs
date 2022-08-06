namespace Services.NotificationService;

using System.Threading.Tasks;

using ViewModels.Notification;

public interface INotificationService
{
    Task<int> GetUserNotificationsCount(string receiverUsername);

    Task<string> AddMessageNotification(string message, string receiverUsername, string senderUsername, string group);

    Task<string> UpdateMessageNotifications(string senderUsername, string receiverUsername);

    Task<NotificationViewModel> GetNotificationById(string id);

    Task<NotificationModel> GetUserNotifications(string currentUserUsername, string currentUserId, int notificationCount, int skip);

    Task<bool> EditStatus(string receiverId, string newStatus, string id);

    Task<bool> DeleteNotification(string receiverId, string id);
}