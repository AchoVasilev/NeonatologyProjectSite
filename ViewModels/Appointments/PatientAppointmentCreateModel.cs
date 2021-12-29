namespace ViewModels.Appointments
{
    using System.ComponentModel.DataAnnotations;
    using System;

    using static Data.Common.DataConstants.Constants;
    using static Common.GlobalConstants.Messages;
using System.Collections.Generic;

    public class PatientAppointmentCreateModel
    {
        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        [Required(ErrorMessage = RequiredFieldErrorMsg)]
        [StringLength(DefaultMaxLength, MinimumLength = DefaultMinLength, ErrorMessage = LengthErrorMsg)]
        public string ChildFirstName { get; set; }

        public string DoctorId { get; set; }

        public string PatientId { get; set; }

        public int AppointmentCauseId { get; set; }

        public ICollection<AppointmentCauseViewModel> AppointmentCauses { get; set; }
    }
}
