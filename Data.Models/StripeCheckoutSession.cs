namespace Data.Models;

using System;
using System.ComponentModel.DataAnnotations;

public class StripeCheckoutSession
{
    [Key]
    public string Id { get; set; }

    public string PaymentId { get; set; }

    public string ToStripeAccountId { get; set; }

    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
}