namespace ViewModels.Payment;

public class CreatePaymentModel
{
    public string SessionId { get; set; }

    public string CustomerId { get; set; }

    public string CustomerEmail { get; set; }

    public int OfferedServiceId { get; set; }

    public string PaymentStatus { get; set; }

    public string Status { get; set; }

    public long? Charge { get; set; }
}