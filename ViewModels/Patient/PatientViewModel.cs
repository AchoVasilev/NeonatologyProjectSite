namespace ViewModels.Patient
{
    using System.ComponentModel.DataAnnotations;

    using static Data.Common.DataConstants.Constants;
    using static Common.GlobalConstants.MessageConstants;
    using static Common.GlobalConstants.AccountConstants;

    public class PatientViewModel
    {
        public string Id { get; set; }

        [Display(Name = Name)]
        [Required(ErrorMessage = RequiredFieldErrorMsg)]
        [StringLength(DefaultMaxLength, MinimumLength = DefaultMinLength, ErrorMessage = LengthErrorMsg)]
        public string FirstName { get; set; }

        [Display(Name = FamilyName)]
        [Required(ErrorMessage = RequiredFieldErrorMsg)]
        [StringLength(DefaultMaxLength, MinimumLength = DefaultMinLength, ErrorMessage = LengthErrorMsg)]
        public string LastName { get; set; }

        [Display(Name = Common.GlobalConstants.AccountConstants.Phone)]
        [Required(ErrorMessage = RequiredFieldErrorMsg)]
        [Phone]
        public string Phone { get; set; }
    }
}
