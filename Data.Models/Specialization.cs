namespace Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using Data.Common.Models;

    using static Data.Common.DataConstants.Constants;

    public class Specialization : BaseModel<int>
    {
        [Required]
        [MaxLength(DefaultMaxLength)]
        public string Name { get; set; }

        [MaxLength(DescriptionMaxLength)]
        public string Description { get; set; }

        [ForeignKey(nameof(Doctor))]
        public string DoctorId { get; set; }

        public virtual Doctor Doctor { get; set; }
    }
}
