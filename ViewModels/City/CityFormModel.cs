namespace ViewModels.City;

using System.ComponentModel.DataAnnotations;

using static Data.Common.DataConstants.Constants;

public class CityFormModel
{
    public int Id { get; init; }

    [Required]
    [MaxLength(DefaultMaxLength)]
    public string Name { get; set; }
}