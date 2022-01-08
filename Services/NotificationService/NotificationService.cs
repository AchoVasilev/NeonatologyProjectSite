namespace Services.NotificationService
{
    using System.Linq;
    using System.Security.Cryptography.X509Certificates;
    using System.Threading.Tasks;

    using Data;
    using Data.Models;
    using Data.Models.Enums;

    using Microsoft.EntityFrameworkCore;

    public class NotificationService : INotificationService
    {
        private readonly NeonatologyDbContext data;

        public NotificationService(NeonatologyDbContext data)
        {
            this.data = data;
        }

        public async Task<int> GetUserNotificationsCount(string username)
            => await this.data.Notifications
                         .Where(x => x.Receiver.Email == username && x.NotificationStatus == NotificationStatus.Unread)
                         .CountAsync();

        public async Task<string> AddMessageNotification(string message, string receiverId, string senderId)
        {
            var notification = new Notification
            {
                SenderId = senderId,
                ReceiverId = receiverId,
                NotificationStatus = NotificationStatus.Unread,

            };

            return notification.Id;
        }
    }
}
