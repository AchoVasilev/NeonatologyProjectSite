namespace ViewModels.Administration.Rating;

using System;

public class RatingViewModel
{
    public int Id { get; init; }

    public DateTime CreatedOn { get; set; }

    public bool? IsConfirmed { get; set; } = false;

    public int Number { get; set; }

    public string Comment { get; set; }

    public int AppointmentId { get; set; }
}