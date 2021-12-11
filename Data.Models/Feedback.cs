namespace Data.Models
{
using System;
    using System.ComponentModel.DataAnnotations;

    using Data.Common.Models;

    using static Data.Common.DataConstants.Constants;

    public class Feedback : BaseModel<int>
    {
        public int Id { get; init; }

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        public DateTime ModifiedOn { get; set; }

        public DateTime DeletedOn { get; set; }

        public bool IsDeleted { get; set; }

        [Required]
        [MaxLength(DefaultMaxLength)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(DefaultMaxLength)]
        public string LastName { get; set; }

        [Required]
        [MaxLength(DefaultMaxLength)]
        public string Email { get; set; }

        [Required]
        public string Type { get; set; }

        [Required]
        [MaxLength(DescriptionMaxLength)]
        public string Comment { get; set; }

        public bool? IsSolved { get; set; }
    }
}
