namespace Services.PaymentService
{
    using System.Threading.Tasks;

    public interface IPaymentService
    {
        Task CreatePaymentSession(string sessionId, string paymentId, string toStripeAccountId);
    }
}
