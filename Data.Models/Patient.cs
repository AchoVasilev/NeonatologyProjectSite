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

        [MaxLength(DefaultMaxLength)]
        public string MiddleName { get; set; }

        [Required]
        [MaxLength(DefaultMaxLength)]
        public string LastName { get; set; }

        public int Age { get; set; }

        public string Gender { get; set; }

        [MaxLength(DefaultMaxLength)]
        public string Phone { get; set; }

        [ForeignKey(nameof(City))]
        public int CityId { get; set; }

        public virtual City City { get; set; }

        [ForeignKey(nameof(User))]
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual ICollection<Rating> Ratings { get; set; } = new HashSet<Rating>();

        public virtual ICollection<Appointment> Appointments { get; set; } = new HashSet<Appointment>();
    }
}
