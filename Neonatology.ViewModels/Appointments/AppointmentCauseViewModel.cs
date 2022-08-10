namespace Neonatology.ViewModels.Appointments;

using System.ComponentModel.DataAnnotations;
using Common.Mapping;
using Data.Models;

public class AppointmentCauseViewModel : IMapFrom<AppointmentCause>
{
    public int Id { get; init; }

    [Required]
    public string Name { get; set; }
}