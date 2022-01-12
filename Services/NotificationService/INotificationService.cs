namespace Services.NotificationService
{
    using System.Threading.Tasks;

    using ViewModels.Notification;

    public interface INotificationService
    {
        Task<int> GetUserNotificationsCount(string receiverId);

        Task<string> AddMessageNotification(string message, string receiverUsername, string senderUsername);

        Task<string> UpdateMessageNotifications(string senderUsername, string receiverUsername);

        Task<NotificationViewModel> GetNotificationById(string id);
    }
}
