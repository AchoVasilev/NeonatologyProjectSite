namespace Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class ChatImage
    {
        public string Id { get; init; } = Guid.NewGuid().ToString();

        [MaxLength(60)]
        public string Name { get; set; }

        public string Url { get; set; }

        [ForeignKey(nameof(Group))]
        public string GroupId { get; set; }

        public Group Group { get; set; }

        [ForeignKey(nameof(Message))]
        public int MessageId { get; set; }

        public Message Message { get; set; }
    }
}
