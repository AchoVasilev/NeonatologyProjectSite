namespace Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using Data.Common.Models;

    public class Image : BaseModel<string>
    {
        [Key]
        public string Id { get; init; } = Guid.NewGuid().ToString();

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        public DateTime? ModifiedOn { get; set; }

        public DateTime? DeletedOn { get; set; }

        public bool IsDeleted { get; set; } = false;

        public string RemoteImageUrl { get; set; }

        public string Url { get; set; }

        public string Name { get; set; }

        public string Extension { get; set; }

        [ForeignKey(nameof(User))]
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}
