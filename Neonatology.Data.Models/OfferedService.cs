namespace Neonatology.Data.Models;

using System.ComponentModel.DataAnnotations;
using Common.Models;
using Microsoft.EntityFrameworkCore;

public class OfferedService : BaseModel<int>
{
    [Required]
    [MaxLength(200)]
    public string Name { get; set; }

    [Precision(14, 2)]
    public decimal Price { get; set; }
}