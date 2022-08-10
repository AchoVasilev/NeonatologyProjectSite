namespace Neonatology.ViewModels.Notification;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Data.Models.Enums;
using static Data.Common.DataConstants.Constants;

public class NotificationViewModel
{
    public string Id { get; set; }

    [Required]
    public string Heading { get; set; }

    public NotificationStatus Status { get; set; }

    public ICollection<string> AllStatuses { get; set; } = new HashSet<string>();

    public string TargetFirstName { get; set; }

    public string TargetLastName { get; set; }

    [Required]
    [MaxLength(DefaultMaxLength)]
    public string TargetUsername { get; set; }

    [Required]
    public string CreatedOn { get; set; }

    [Required]
    [MaxLength(DescriptionMaxLength)]
    public string Text { get; set; }

    public string ImageUrl { get; set; }
}