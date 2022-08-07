namespace Services.PaymentService;

using System.Threading.Tasks;
using Common;
using ViewModels.Payment;

public interface IPaymentService : ITransientService
{
    Task CreateCheckoutSession(string sessionId, string paymentId, string toStripeAccountId);

    Task<string> CreatePayment(CreatePaymentModel model);

    Task<bool> PatientHasPaid(string patientId);

    Task<bool> ChangePaymentStatus(string patientId);
}