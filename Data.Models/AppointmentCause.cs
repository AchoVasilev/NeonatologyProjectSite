﻿namespace Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using Data.Common.Models;

    using static Data.Common.DataConstants.Constants;

    public class AppointmentCause : BaseModel<int>
    {
        [Key]
        public int Id { get; init ; }

        [Required]
        [MaxLength(DefaultMaxLength)]
        public string Name { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        public DateTime? ModifiedOn { get; set; }

        public DateTime? DeletedOn { get; set; }

        public bool IsDeleted { get; set; } = false;

        [ForeignKey(nameof(Appointment))]
        public int? AppointmentId { get; set; }

        public Appointment Appointment { get; set; }
    }
}
