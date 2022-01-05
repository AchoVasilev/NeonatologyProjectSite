namespace Services.PaymentService
{
    using System.Threading.Tasks;

    using Data;
    using Data.Models;

    public class PaymentService : IPaymentService
    {
        private readonly NeonatologyDbContext data;

        public PaymentService(NeonatologyDbContext dbContext)
        {
            this.data = dbContext;
        }

        public async Task CreatePaymentSession(string sessionId, string paymentId, string toStripeAccountId)
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
    }
}
