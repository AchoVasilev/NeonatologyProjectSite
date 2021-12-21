namespace ViewModels.Offer
{
    using System.ComponentModel.DataAnnotations;

    public class OfferViewModel
    {
        [Required]
        public string Name { get; set; }

        public decimal Price { get; set; }
    }
}
