namespace Neonatology.Data.Models;

using System.ComponentModel.DataAnnotations;
using Common.Models;
using static Common.DataConstants.Constants;

public class City : BaseModel<int>
{
    [Required]
    [MaxLength(DefaultMaxLength)]
    public string Name { get; set; }

    public int? ZipCode { get; set; }
}