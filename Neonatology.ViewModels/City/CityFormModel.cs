namespace Neonatology.ViewModels.City;

using System.ComponentModel.DataAnnotations;
using Common.Mapping;
using Data.Models;
using static Data.Common.DataConstants.Constants;

public class CityFormModel : IMapFrom<City>
{
    public int Id { get; init; }

    [Required]
    [MaxLength(DefaultMaxLength)]
    public string Name { get; set; }
}