namespace Neonatology.ViewModels.Feedback;

using System.ComponentModel.DataAnnotations;
using static Common.Constants.GlobalConstants.MessageConstants;
using static Data.Common.DataConstants.Constants;
public class FeedbackInputModel
{
    [Required(ErrorMessage = RequiredFieldErrorMsg)]
    [MaxLength(DefaultMaxLength)]
    public string FirstName { get; set; }

    [Required(ErrorMessage = RequiredFieldErrorMsg)]
    [MaxLength(DefaultMaxLength)]
    public string LastName { get; set; }

    [Required(ErrorMessage = RequiredFieldErrorMsg)]
    [EmailAddress]
    [MaxLength(DefaultMaxLength)]
    public string Email { get; set; }

    [Required(ErrorMessage = RequiredFieldErrorMsg)]
    [StringLength(maximumLength: DefaultMaxLength, MinimumLength = DefaultMinLength, ErrorMessage = LengthErrorMsg)]
    public string Type { get; set; }

    [Required(ErrorMessage = RequiredFieldErrorMsg)]
    [MaxLength(DescriptionMaxLength)]
    public string Comment { get; set; }
}