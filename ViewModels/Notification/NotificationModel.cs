namespace ViewModels.Notification;

using System.Collections.Generic;

public class NotificationModel
{
    public bool IsLessThanDefaultCount { get; set; }

    public ICollection<NotificationViewModel> Notifications { get; set; } = new HashSet<NotificationViewModel>();
}