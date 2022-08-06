namespace Data.Models;

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Data.Common.Models;
using Enums;

public class Notification : BaseModel<string>
{
    public Notification()
    {
        this.Id = Guid.NewGuid().ToString();
    }

    [Required]
    public string Link { get; set; }

    [Required]
    public string Text { get; set; }

    public NotificationStatus NotificationStatus { get; set; }

    [ForeignKey(nameof(NotificationType))]
    public int NotificationTypeId { get; set; }

    public NotificationType NotificationType { get; set; }

    [ForeignKey(nameof(Sender))]
    public string SenderId { get; set; }

    public virtual ApplicationUser Sender { get; set; }

    [ForeignKey(nameof(Receiver))]
    public string ReceiverId { get; set; }

    public virtual ApplicationUser Receiver { get; set; }
}