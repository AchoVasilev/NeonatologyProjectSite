namespace ViewModels.Appointments
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using Infrastructure.CustomAttributes;

    using static Data.Common.DataConstants.Constants;

    public class AppointmentViewModel
    {
        [Required]
        [ValidateDateString]
        public string Date { get; set; }

        [Required]
        [ValidateTimeString]
        public string Time { get; set; }

        [Required]
        [MaxLength(DefaultMaxLength)]
        public string ParentFirstName { get; set; }

        [Required]
        [MaxLength(DefaultMaxLength)]
        public string ParentLastName { get; set; }

        [Required]
        [Phone]
        public string PhoneNumber { get; set; }

        [Required]
        [MaxLength(DefaultMaxLength)]
        public string ChildFirstName { get; set; }

        public bool IsRated { get; set; }

        public string DoctorId { get; set; }
    }
}
