namespace Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using Data.Common.Models;

    using static Data.Common.DataConstants.Constants;

    public class AppointmentCause : BaseModel<int>
    {
        [Required]
        [MaxLength(DefaultMaxLength)]
        public string Name { get; set; }

        [ForeignKey(nameof(Appointment))]
        public int? AppointmentId { get; set; }

        public Appointment Appointment { get; set; }
    }
}
