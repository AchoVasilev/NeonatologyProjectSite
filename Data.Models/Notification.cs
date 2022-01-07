namespace Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using Data.Common.Models;
    using Data.Models.Enums;

    using static Data.Common.DataConstants.Constants;

    public class Notification : BaseModel<string>
    {
        public string Id { get; init; } = Guid.NewGuid().ToString();

        [Required]
        [MaxLength(DefaultMaxLength)]
        public string TargetUsername { get; set; }

        [Required]
        public string Link { get; set; }

        [Required]
        public string Text { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        public DateTime ModifiedOn { get; set; }

        public DateTime DeletedOn { get; set; }

        public bool IsDeleted { get; set; }

        public NotificationStatus NotificationStatus { get; set; }

        [ForeignKey(nameof(NotificationType))]
        public int NotificationTypeId { get; set; }

        public NotificationType NotificationType { get; set; }

        [ForeignKey(nameof(ApplicationUser))]
        public string UserId { get; set; }

        public ApplicationUser ApplicationUser { get; set; }
    }
}
