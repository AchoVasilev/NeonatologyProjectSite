namespace Services.PaymentService
{
    using System;
    using System.Threading.Tasks;

    using Data;
    using Data.Models;

    using Microsoft.EntityFrameworkCore;

    using Services.PatientService;

    using ViewModels.Payment;

    public class PaymentService : IPaymentService
    {
        private readonly NeonatologyDbContext data;
        private readonly IPatientService patientService;

        public PaymentService(NeonatologyDbContext data, IPatientService patientService)
        {
            this.data = data;
            this.patientService = patientService;
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
            var payment = new Payment()
            {
                OfferedServiceId = model.OfferedServiceId,
                SenderId = await this.patientService.GetPatientIdByEmail(model.CustomerEmail),
                PaymentStatus = model.PaymentStatus,
                Status = model.Status,
                Charge = model.Charge,
                CustomerEmail = model.CustomerEmail,
                CustomerId = model.CustomerId,
                SessionId = model.SessionId
            };

            var patient = await this.patientService.GetPatientById(payment.SenderId);
            patient.HasPaid = true;
            patient.ModifiedOn = DateTime.UtcNow;

            await this.data.Payments.AddAsync(payment);
            await this.data.SaveChangesAsync();

            return payment.Id;
        }

        public async Task<bool> PatientHasPaid(string patientId)
            => await this.data.Patients
                         .AnyAsync(x => x.Id == patientId && x.HasPaid);

        public async Task<bool> ChangePaymentStatus(string patientId)
        {
            var patient = await this.patientService.GetPatientById(patientId);

            if (patient is null)
            {
                return false;
            }

            patient.HasPaid = false;
            patient.ModifiedOn = DateTime.UtcNow;

            await this.data.SaveChangesAsync();

            return true;
        }
    }
}
