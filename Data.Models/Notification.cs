namespace Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using Data.Common.Models;
    using Data.Models.Enums;

    public class Notification : BaseModel<string>
    {
        [Key]
        public string Id { get; init; } = Guid.NewGuid().ToString();

        [Required]
        public string Link { get; set; }

        [Required]
        public string Text { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        public DateTime? ModifiedOn { get; set; }

        public DateTime? DeletedOn { get; set; }

        public bool IsDeleted { get; set; } = false;

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
}
