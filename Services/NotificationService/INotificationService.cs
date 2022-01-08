namespace Services.NotificationService
{
    using System.Threading.Tasks;

    public interface INotificationService
    {
        Task<int> GetUserNotificationsCount(string receiverId);

        Task<string> AddMessageNotification(string message, string receiverId, string senderId);
    }
}
