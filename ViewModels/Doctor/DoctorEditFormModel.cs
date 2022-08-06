namespace ViewModels.Doctor;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static Common.GlobalConstants.MessageConstants;
using static Common.GlobalConstants.AccountConstants;
using static Data.Common.DataConstants.Constants;
using City;
using Microsoft.AspNetCore.Http;
using Address;

public class DoctorEditFormModel
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

    [Display(Name = PictureLinkName)]
    public string UserImageUrl { get; set; }

    [Required(ErrorMessage = RequiredFieldErrorMsg)]
    [Display(Name = AgeName)]
    public int Age { get; set; }

    [Required(ErrorMessage = RequiredFieldErrorMsg)]
    [Phone]
    [Display(Name = Phone)]
    public string PhoneNumber { get; set; }

    [Display(Name = YearsExperience)]
    public int? YearsOfExperience { get; set; }

    [Required(ErrorMessage = RequiredFieldErrorMsg)]
    [EmailAddress]
    [Display(Name = EmailName)]
    public string Email { get; set; }

    [StringLength(DescriptionMaxLength, MinimumLength = DefaultMinLength, ErrorMessage = LengthErrorMsg)]
    [Display(Name = BiographyName)]
    public string Biography { get; set; }

    public IFormFile Picture { get; set; }

    public DateTime ModifiedOn { get; set; } = DateTime.UtcNow;

    public ICollection<EditAddressFormModel> Addresses { get; set; }

    public ICollection<CityFormModel> Cities { get; set; }
}