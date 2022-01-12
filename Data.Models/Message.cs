namespace Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    using Data.Common.Models;

    public class Message : BaseModel<int>
    {
        public int Id { get; init; }

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        public DateTime? ModifiedOn { get; set; }

        public DateTime? DeletedOn { get; set; }

        public bool IsDeleted { get; set; } = false;

        [ForeignKey(nameof(Sender))]
        public string SenderId { get; set; }

        public virtual ApplicationUser Sender { get; set; }

        [ForeignKey(nameof(Receiver))]
        public string ReceiverId { get; set; }

        public virtual ApplicationUser Receiver { get; set; }

        public string Content { get; set; }

        [ForeignKey(nameof(Group))]
        public string GroupId { get; set; }

        public Group Group { get; set; }

        public ICollection<ChatImage> ChatImages { get; set; } = new HashSet<ChatImage>();
    }
}
