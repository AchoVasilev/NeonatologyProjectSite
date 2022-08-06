namespace Data.Common.Models;

using System;
using System.ComponentModel.DataAnnotations;

public class BaseModel<TKey>
{
    [Key]
    public TKey Id { get; init; }

    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

    public DateTime? ModifiedOn { get; set; }

    public DateTime? DeletedOn { get; set; }

    public bool IsDeleted { get; set; } = false;
}