namespace ViewModels.Appointments
{
    using System.ComponentModel.DataAnnotations;

    public class AppointmentCauseViewModel
    {
        public int Id { get; init; }

        [Required]
        public string Name { get; set; }
    }
}
