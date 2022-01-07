namespace Data.Models
{
    using System;

    using Data.Common.Models;

    public class Message : BaseModel<int>
    {
        public int Id { get; init; }

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        public DateTime ModifiedOn { get; set; }

        public DateTime DeletedOn { get; set; }

        public bool IsDeleted { get; set; }

        public string SenderId { get; set; }

        public virtual ApplicationUser Sender { get; set; }

        public string ReceiverId { get; set; }

        public virtual ApplicationUser Receiver { get; set; }

        public string Content { get; set; }
    }
}
