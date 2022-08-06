namespace ViewModels.Offer;

using System.ComponentModel.DataAnnotations;

public class OfferViewModel
{
    public int Id { get; init; }

    [Required]
    public string Name { get; set; }

    public decimal Price { get; set; }
}