namespace ViewModels.Administration.Offer
{
    using System.ComponentModel.DataAnnotations;

    using static Data.Common.DataConstants.Constants;

    public class EditOfferFormModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(DefaultMaxLength)]
        public string Name { get; set; }

        public decimal Price { get; set; }
    }
}
