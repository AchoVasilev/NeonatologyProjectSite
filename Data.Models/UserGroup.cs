namespace Data.Models;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

public class UserGroup
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    [Required]
    [ForeignKey(nameof(Group))]
    public string GroupId { get; set; }

    public Group Group { get; set; }

    [Required]
    [ForeignKey(nameof(ApplicationUser))]
    public string ApplicationUserId { get; set; }

    public ApplicationUser ApplicationUser { get; set; }
}