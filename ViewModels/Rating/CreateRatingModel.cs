namespace ViewModels.Rating;

public class CreateRatingModel
{
    public int AppointmentId { get; set; }

    public string DoctorId { get; set; }

    public string PatientId { get; set; }

    public int Number { get; set; }

    public string Comment { get; set; }
}