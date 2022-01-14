namespace Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using Data.Models.Enums;

    public class Payment
    {
        [Key]
        public string Id { get; init; } = Guid.NewGuid().ToString();

        public PaymentStatus PaymentStatus { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        public DateTime? TransactionDate { get; set; }

        [ForeignKey(nameof(Doctor))]
        public string RecepientId { get; set; }

        public Doctor Doctor { get; set; }

        [ForeignKey(nameof(Patient))]
        public string SenderId { get; set; }

        public Patient Patient { get; set; }

        [ForeignKey(nameof(OfferedService))]
        public int OfferedServiceId { get; set; }

        public OfferedService OfferedService { get; set; }
    }
}
