namespace Services.PaymentService
{
    using System.Threading.Tasks;
using ViewModels.Payment;

    public interface IPaymentService
    {
        Task CreateChekoutSession(string sessionId, string paymentId, string toStripeAccountId);

        Task<string> CreatePayment(CreatePaymentModel model);

        Task<bool> PatientHasPaid(string patientId);
    }
}
