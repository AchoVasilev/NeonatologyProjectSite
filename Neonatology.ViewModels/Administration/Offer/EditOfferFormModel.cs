namespace Neonatology.ViewModels.Administration.Offer;

using System.ComponentModel.DataAnnotations;
using Common.Mapping;
using Data.Models;
using static Data.Common.DataConstants.Constants;

public class EditOfferFormModel : IMapFrom<OfferedService>
{
    public int Id { get; set; }

    [Required]
    [MaxLength(DefaultMaxLength)]
    public string Name { get; set; }

    public decimal Price { get; set; }
}