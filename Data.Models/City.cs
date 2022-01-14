namespace Data.Models
{
using System;
    using System.ComponentModel.DataAnnotations;

    using Data.Common.Models;

    using static Data.Common.DataConstants.Constants;

    public class City : BaseModel<int>
    {
        [Key]
        public int Id { get; init; }

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        public DateTime? ModifiedOn { get; set; }

        public DateTime? DeletedOn { get; set; }

        public bool IsDeleted { get; set; } = false;

        [Required]
        [MaxLength(DefaultMaxLength)]
        public string Name { get; set; }

        public int? ZipCode { get; set; }
    }
}
