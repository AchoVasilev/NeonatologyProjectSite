namespace ViewModels.Appointments
{
    using System.ComponentModel.DataAnnotations;

    using Infrastructure.CustomAttributes;

    using static Data.Common.DataConstants.Constants;
    using static Common.GlobalConstants.Messages;

    public class AppointmentViewModel
    {
        [Required(ErrorMessage = RequiredFieldErrorMsg)]
        [ValidateDateString(ErrorMessage = RequiredFieldErrorMsg)]
        public string Date { get; set; }

        [Required(ErrorMessage = RequiredFieldErrorMsg)]
        [ValidateTimeString(ErrorMessage = RequiredFieldErrorMsg)]
        public string Time { get; set; }

        [Required(ErrorMessage = RequiredFieldErrorMsg)]
        [StringLength(DefaultMaxLength, MinimumLength = DefaultMinLength, ErrorMessage = LengthErrorMsg)]
        public string ParentFirstName { get; set; }

        [Required(ErrorMessage = RequiredFieldErrorMsg)]
        [StringLength(DefaultMaxLength, MinimumLength = DefaultMinLength, ErrorMessage = LengthErrorMsg)]
        public string ParentLastName { get; set; }

        [Required(ErrorMessage = RequiredFieldErrorMsg)]
        [Phone]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = RequiredFieldErrorMsg)]
        [StringLength(DefaultMaxLength, MinimumLength = DefaultMinLength, ErrorMessage = LengthErrorMsg)]
        public string ChildFirstName { get; set; }

        public string DoctorId { get; set; }
    }
}
