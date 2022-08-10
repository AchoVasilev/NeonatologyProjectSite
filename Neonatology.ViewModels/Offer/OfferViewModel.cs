namespace Neonatology.ViewModels.Offer;

using System.ComponentModel.DataAnnotations;
using Common.Mapping;
using Data.Models;

public class OfferViewModel : IMapFrom<OfferedService>
{
    public int Id { get; init; }

    [Required]
    public string Name { get; set; }

    public decimal Price { get; set; }
}