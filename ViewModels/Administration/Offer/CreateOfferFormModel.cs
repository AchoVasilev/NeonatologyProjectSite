namespace ViewModels.Administration.Offer
{
    using System.ComponentModel.DataAnnotations;

    public class CreateOfferFormModel
    {
        [Required]
        public string Name { get; set; }

        public decimal Price { get; set; }
    }
}
