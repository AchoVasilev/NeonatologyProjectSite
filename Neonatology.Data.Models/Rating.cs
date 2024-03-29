﻿namespace Neonatology.Data.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Common.Models;
using static Common.DataConstants.Constants;

public class Rating : BaseModel<int>
{
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