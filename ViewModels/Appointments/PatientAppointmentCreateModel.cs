namespace ViewModels.Appointments
{
    using System.ComponentModel.DataAnnotations;

    using Infrastructure.CustomAttributes;

    using static Data.Common.DataConstants.Constants;
    using static Common.GlobalConstants.Messages;

    public class PatientAppointmentCreateModel
    {
        [Required(ErrorMessage = RequiredFieldErrorMsg)]
        [ValidateDateString(ErrorMessage = RequiredFieldErrorMsg)]
        public string Date { get; set; }

        [Required(ErrorMessage = RequiredFieldErrorMsg)]
        [ValidateTimeString(ErrorMessage = RequiredFieldErrorMsg)]
        public string Time { get; set; }

        [Required(ErrorMessage = RequiredFieldErrorMsg)]
        [StringLength(DefaultMaxLength, MinimumLength = DefaultMinLength, ErrorMessage = LengthErrorMsg)]
        public string ChildFirstName { get; set; }

        public string AppointmentCause { get; set; }

        public string DoctorId { get; set; }

        public string PatientId { get; set; }
    }
}
