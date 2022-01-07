namespace Services.PaymentService
{
    using System.Threading.Tasks;

    using Data;
    using Data.Models;

    using Microsoft.EntityFrameworkCore;

    using ViewModels.Payment;

    public class PaymentService : IPaymentService
    {
        private readonly NeonatologyDbContext data;

        public PaymentService(NeonatologyDbContext data)
        {
            this.data = data;
        }

        public async Task CreateChekoutSession(string sessionId, string paymentId, string toStripeAccountId)
        {
            var session = new StripeCheckoutSession
            {
                Id = sessionId,
                PaymentId = paymentId,
                ToStripeAccountId = toStripeAccountId
            };

            await this.data.StripeCheckoutSessions.AddAsync(session);
            await this.data.SaveChangesAsync();
        }

        public async Task<string> CreatePayment(CreatePaymentModel model)
        {
            var payment = new Payment();

            payment.OfferedServiceId = model.OfferedServiceId;
            payment.RecepientId = model.RecepientId;
            payment.SenderId = model.SenderId;
            payment.PaymentStatus = Data.Models.Enums.PaymentStatus.Complete;

            await this.data.Payments.AddAsync(payment);
            await this.data.SaveChangesAsync();

            return payment.Id;
        }

        public async Task<bool> PatientHasPaid(string patientId)
            => await this.data.Payments
                         .AnyAsync(x => x.SenderId == patientId);
    }
}
