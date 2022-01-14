namespace Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using Data.Common.Models;

    using static Data.Common.DataConstants.Constants;

    public class Doctor : BaseModel<string>
    {
        [Key]
        public string Id { get; init; } = Guid.NewGuid().ToString();

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        public DateTime? ModifiedOn { get; set; }

        public DateTime? DeletedOn { get; set; }

        public bool IsDeleted { get; set; } = false;

        [Required]
        [MaxLength(DefaultMaxLength)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(DefaultMaxLength)]
        public string LastName { get; set; }

        public int Age { get; set; }

        [MaxLength(DefaultMaxLength)]
        public string PhoneNumber { get; set; }

        public int? YearsOfExperience { get; set; }

        public string Email { get; set; }

        [Required]
        [MaxLength(AddressMaxLength)]
        public string Address { get; set; }

        [MaxLength(DescriptionMaxLength)]
        public string Biography { get; set; }

        [ForeignKey(nameof(City))]
        public int CityId { get; set; }
        public virtual City City { get; set; }

        public virtual ICollection<Specialization> Specializations { get; set; } = new HashSet<Specialization>();

        [ForeignKey(nameof(User))]
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        [ForeignKey(nameof(Image))]
        public string ImageId { get; set; }

        public virtual Image Image { get; set; }

        public virtual ICollection<Rating> Ratings { get; set; } = new HashSet<Rating>();

        public virtual ICollection<Appointment> Appointments { get; set; } = new HashSet<Appointment>();
    }
}
