namespace Neonatology.Data.Models;

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Common.Models;
using static Common.DataConstants.Constants;

public class AppointmentSlot : BaseModel<int>
{
    [MaxLength(DefaultMaxLength)]
    public string Text { get; set; }

    public DateTime Start { get; set; }

    public DateTime End { get; set; }

    public string Status { get; set; } = "Свободен";

    [ForeignKey(nameof(Address))]
    public int AddressId { get; set; }

    public virtual Address Address { get; set; }
}