namespace Neonatology.ViewModels.Administration.Offer;

using System.ComponentModel.DataAnnotations;
using static Data.Common.DataConstants.Constants;

public class CreateOfferFormModel
{
    [Required]
    [MaxLength(DefaultMaxLength)]
    public string Name { get; set; }

    public decimal Price { get; set; }
}