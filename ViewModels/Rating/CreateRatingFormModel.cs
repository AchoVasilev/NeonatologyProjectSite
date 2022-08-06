namespace ViewModels.Rating;

using System.ComponentModel.DataAnnotations;

using static Data.Common.DataConstants.Constants;

public class CreateRatingFormModel
{
    public int AppointmentId { get; set; }

    public string DoctorId { get; set; }

    public string PatientId { get; set; }

    [Range(RatingMin, RatingMax)]
    public int Number { get; set; }

    [MaxLength(CommentMaxLength)]
    public string Comment { get; set; }
}