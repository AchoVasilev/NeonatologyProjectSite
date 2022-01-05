namespace Data.Models
{
    using System;

    public class StripeCheckoutSession
    {
        public string Id { get; set; }

        public string PaymentId { get; set; }

        public string ToStripeAccountId { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    }
}
