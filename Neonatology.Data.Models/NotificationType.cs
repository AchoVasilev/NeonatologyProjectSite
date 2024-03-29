﻿namespace Neonatology.Data.Models;

using System.ComponentModel.DataAnnotations;
using static Common.DataConstants.Constants;

public class NotificationType
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(DefaultMaxLength)]
    public string Name { get; set; }
}