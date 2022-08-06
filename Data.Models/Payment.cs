namespace Data.Models;

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Payment
{
    [Key]
    public string Id { get; init; } = Guid.NewGuid().ToString();

    public string PaymentStatus { get; set; }

    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

    public DateTime? DeletedOn { get; set; }

    [ForeignKey(nameof(Patient))]
    public string SenderId { get; set; }

    public Patient Patient { get; set; }

    public string SessionId { get; set; }

    public string CustomerId { get; set; }

    public string CustomerEmail { get; set; }

    public string Status { get; set; }

    public long? Charge { get; set; }

    [ForeignKey(nameof(OfferedService))]
    public int OfferedServiceId { get; set; }

    public OfferedService OfferedService { get; set; }
}