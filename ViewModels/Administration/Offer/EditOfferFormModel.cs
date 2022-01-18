namespace ViewModels.Administration.Offer
{
    using System.ComponentModel.DataAnnotations;

    public class EditOfferFormModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public decimal Price { get; set; }
    }
}
