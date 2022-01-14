namespace Data.Models
{
using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using Data.Common.Models;

    using static Data.Common.DataConstants.Constants;

    public class Rating : BaseModel<int>
    {
        [Key]
        public int Id { get; init; }

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        public DateTime? ModifiedOn { get; set; }

        public DateTime? DeletedOn { get; set; }

        public bool IsDeleted { get; set; } = false;

        public bool? IsConfirmed { get; set; } = false;

        public int Number { get; set; }

        [MaxLength(CommentMaxLength)]
        public string Comment { get; set; }

        [ForeignKey(nameof(Appointment))]
        public int AppointmentId { get; set; }

        public virtual Appointment Appointment { get; set; }

        [ForeignKey(nameof(Doctor))]
        public string DoctorId { get; set; }

        public virtual Doctor Doctor { get; set; }

        [ForeignKey(nameof(Patient))]
        public string PatientId { get; set; }

        public virtual Patient Patient { get; set; }
    }
}
