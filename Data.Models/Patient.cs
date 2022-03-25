namespace Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using Data.Common.Models;

    using static Data.Common.DataConstants.Constants;

    public class Patient : BaseModel<string>
    {
        public Patient()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        [Required]
        [MaxLength(DefaultMaxLength)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(DefaultMaxLength)]
        public string LastName { get; set; }

        [Required]
        [MaxLength(DefaultMaxLength)]
        public string Phone { get; set; }

        [ForeignKey(nameof(User))]
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        [ForeignKey(nameof(City))]
        public int? CityId { get; set; }

        public City City { get; set; }

        public bool HasPaid { get; set; } = false;

        public virtual ICollection<Rating> Ratings { get; set; } = new HashSet<Rating>();

        public virtual ICollection<Payment> Payments { get; set; } = new HashSet<Payment>();

        public virtual ICollection<Appointment> Appointments { get; set; } = new HashSet<Appointment>();
    }
}
