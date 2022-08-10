namespace Neonatology.Data.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Common.Models;
using static Common.DataConstants.Constants;

public class AppointmentCause : BaseModel<int>
{
    [Required]
    [MaxLength(DefaultMaxLength)]
    public string Name { get; set; }

    [ForeignKey(nameof(Appointment))]
    public int? AppointmentId { get; set; }

    public Appointment Appointment { get; set; }
}