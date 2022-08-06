namespace ViewModels.Appointments;

using System.ComponentModel.DataAnnotations;

using static Data.Common.DataConstants.Constants;
using static Common.Constants.GlobalConstants.MessageConstants;
using System.Collections.Generic;

public class CreateAppointmentModel
{
    public string Start { get; set; }

    public string End { get; set; }

    [Required(ErrorMessage = RequiredFieldErrorMsg)]
    [EmailAddress(ErrorMessage = InvalidEmailErrorMsg)]
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

    public int AddressId { get; set; }

    public ICollection<AppointmentCauseViewModel> AppointmentCauses { get; set; }
}