namespace Neonatology.Data.Models;

using System.ComponentModel.DataAnnotations;
using Common.Models;
using static Common.DataConstants.Constants;

public class Feedback : BaseModel<int>
{
    [Required]
    [MaxLength(DefaultMaxLength)]
    public string FirstName { get; set; }

    [Required]
    [MaxLength(DefaultMaxLength)]
    public string LastName { get; set; }

    [Required]
    [MaxLength(DefaultMaxLength)]
    public string Email { get; set; }

    [Required]
    public string Type { get; set; }

    [Required]
    [MaxLength(DescriptionMaxLength)]
    public string Comment { get; set; }

    public bool? IsSolved { get; set; }
}