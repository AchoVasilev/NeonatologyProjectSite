namespace Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class ChatImage
    {
        [Key]
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

        public bool IsDeleted { get; set; } = false;

        public DateTime? DeletedOn { get; set; }
    }
}
