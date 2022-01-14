namespace Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using Data.Common.Models;

    public class OfferedService : BaseModel<int>
    {
        [Key]
        public int Id { get; init; }

        [Required]
        public string Name { get; set; }

        public decimal Price { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        public DateTime? ModifiedOn { get; set; }

        public DateTime? DeletedOn { get; set; }

        public bool IsDeleted { get; set; } = false;
    }
}
