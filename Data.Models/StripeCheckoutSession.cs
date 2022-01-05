﻿namespace Data.Models
{
    using System;

    public class StripeCheckoutSession
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string PaymentId { get; set; }

        public string ToStripeAccountId { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    }
}
