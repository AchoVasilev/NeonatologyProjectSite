namespace Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using Data.Common.Models;

    using static Common.DataConstants.Constants;

    public class Doctor : BaseModel<string>
    {
        public Doctor()
        {
            this.Id = Guid.NewGuid().ToString();
        }

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

        [MaxLength(DescriptionMaxLength)]
        public string Biography { get; set; }

        public virtual ICollection<Specialization> Specializations { get; set; } = new HashSet<Specialization>();

        public virtual ICollection<Address> Addresses { get; set; } = new HashSet<Address>();

        [ForeignKey(nameof(User))]
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual ICollection<Rating> Ratings { get; set; } = new HashSet<Rating>();

        public virtual ICollection<Appointment> Appointments { get; set; } = new HashSet<Appointment>();
    }
}
