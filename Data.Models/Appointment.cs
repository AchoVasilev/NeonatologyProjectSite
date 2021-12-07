namespace Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    using Data.Common.Models;

    public class Appointment : BaseModel<int>
    {
        public DateTime DateTime { get; set; }

        [ForeignKey(nameof(Doctor))]
        public string DoctorId { get; set; }

        public virtual Doctor Doctor { get; set; }

        [ForeignKey(nameof(Patient))]
        public string PatientId { get; set; }

        public virtual Patient Patient { get; set; }

        public bool? Confirmed { get; set; }

        public bool IsRated { get; set; }

        [ForeignKey(nameof(Rating))]
        public int? RatingId { get; set; }

        public virtual Rating Rating { get; set; }
    }
}
