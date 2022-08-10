namespace Neonatology.Data.Models.Dto;

using System.ComponentModel.DataAnnotations;
using static Common.DataConstants.Constants;

public class SpecializationDto
{
    [Required]
    [MaxLength(DefaultMaxLength)]
    public string Name { get; set; }

    [MaxLength(DescriptionMaxLength)]
    public string Description { get; set; }
}