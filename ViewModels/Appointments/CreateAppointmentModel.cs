namespace ViewModels.Appointments
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using static Data.Common.DataConstants.Constants;
    using static Common.GlobalConstants.MessageConstants;
    using System.Collections.Generic;

    public class CreateAppointmentModel
    {
        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        [Required(ErrorMessage = RequiredFieldErrorMsg)]
        [EmailAddress(ErrorMessage = IvalidEmailErrorMsg)]
        public string Email { get; set; }

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

        public int AppointmentCauseId { get; set; }

        public ICollection<AppointmentCauseViewModel> AppointmentCauses { get; set; }
    }
}
