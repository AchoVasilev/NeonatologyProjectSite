namespace Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using Data.Common.Models;

    public class Group : BaseModel<string>
    {
        public string Id { get; init; } = Guid.NewGuid().ToString();

        [MaxLength(60)]
        public string Name { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        public DateTime ModifiedOn { get; set; }

        public DateTime DeletedOn { get; set; }

        public bool IsDeleted { get; set; }

        public ICollection<UserGroup> UserGroups { get; set; } = new HashSet<UserGroup>();

        public ICollection<Message> Messages { get; set; } = new HashSet<Message>();

        public ICollection<ChatImage> ChatImages { get; set; } = new HashSet<ChatImage>();
    }
}
