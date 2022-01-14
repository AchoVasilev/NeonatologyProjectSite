namespace ViewModels.Chat
{
    using System.Collections.Generic;

    using Data.Models;

    public class PrivateChatViewModel
    {
        public ApplicationUser CurrentUser { get; set; }

        public ApplicationUser ToUser { get; set; }

        public string ReceiverFullName { get; set; }

        public string SenderFullName { get; set; }

        public string ReceiverEmail { get; set; }

        public string SenderEmail { get; set; }

        public string GroupName { get; set; }

        public ICollection<Message> ChatMessages { get; set; } = new HashSet<Message>();
    }
}
