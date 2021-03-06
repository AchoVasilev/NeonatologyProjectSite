namespace Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using Data.Common.Models;

    using static Common.DataConstants.Constants;

    public class Appointment : BaseModel<int>
    {
        public DateTime DateTime { get; set; }

        public DateTime End { get; set; }

        [MaxLength(DefaultMaxLength)]
        public string ParentFirstName { get; set; }

        [MaxLength(DefaultMaxLength)]
        public string ParentLastName { get; set; }

        [MaxLength(DefaultMaxLength)]
        public string PhoneNumber { get; set; }

        [MaxLength(DefaultMaxLength)]
        public string ChildFirstName { get; set; }

        [ForeignKey(nameof(Doctor))]
        public string DoctorId { get; set; }

        public virtual Doctor Doctor { get; set; }

        [ForeignKey(nameof(Patient))]
        public string PatientId { get; set; }

        public virtual Patient Patient { get; set; }

        [ForeignKey(nameof(AppointmentCause))]
        public int AppointmentCauseId { get; set; }

        public virtual AppointmentCause AppointmentCause { get; set; }

        public bool IsRated { get; set; } = false;

        [ForeignKey(nameof(Rating))]
        public int? RatingId { get; set; }

        public virtual Rating Rating { get; set; }

        [ForeignKey(nameof(Address))]
        public int AddressId { get; set; }

        public virtual Address Address { get; set; }
    }
}