namespace ViewModels.Payment
{
    public class CreatePaymentModel
    {
        public string RecepientId { get; set; }

        public string SenderId { get; set; }

        public int OfferedServiceId { get; set; }
    }
}
