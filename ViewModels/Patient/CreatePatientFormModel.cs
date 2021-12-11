namespace ViewModels.Patient
{
    using System.ComponentModel.DataAnnotations;

    using static Data.Common.DataConstants.Constants;
    using static Common.GlobalConstants.Messages;
    using static Common.GlobalConstants.PatientConstants;

    public class CreatePatientFormModel
    {
        [Display(Name = Name)]
        [Required(ErrorMessage = RequiredFieldErrorMsg)]
        [StringLength(DefaultMaxLength, MinimumLength = DefaultMinLength, ErrorMessage = LengthErrorMsg)]
        public string FirstName { get; set; }

        [Display(Name = FamilyName)]
        [Required(ErrorMessage = RequiredFieldErrorMsg)]
        [StringLength(DefaultMaxLength, MinimumLength = DefaultMinLength, ErrorMessage = LengthErrorMsg)]
        public string LastName { get; set; }

        [Display(Name = Phone)]
        [Required(ErrorMessage = RequiredFieldErrorMsg)]
        [Phone]
        public string PhoneNumber { get; set; }
    }
}
