namespace Data.Models
{
    using Data.Common.Models;

    public class Message : BaseModel<int>
    {
        public string SenderId { get; set; }

        public virtual ApplicationUser Sender { get; set; }

        public string ReceiverId { get; set; }

        public virtual ApplicationUser Receiver { get; set; }

        public string Content { get; set; }
    }
}
