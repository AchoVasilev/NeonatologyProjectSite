namespace ViewModels.Doctor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using static Common.GlobalConstants.Messages;
    using static Common.GlobalConstants.AccountConstants;
    using static Data.Common.DataConstants.Constants;
    using ViewModels.City;
    using Microsoft.AspNetCore.Http;

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
        public string ImageUrl { get; set; }

        [Required(ErrorMessage = RequiredFieldErrorMsg)]
        [Display(Name = AgeName)]
        public int Age { get; set; }

        [Required(ErrorMessage = RequiredFieldErrorMsg)]
        [Phone]
        [Display(Name = Phone)]
        public string PhoneNumber { get; set; }

        [Display(Name = YearsExpierence)]
        public int? YearsOfExperience { get; set; }

        [Required(ErrorMessage = RequiredFieldErrorMsg)]
        [EmailAddress]
        [Display(Name = EmailName)]
        public string Email { get; set; }

        [Required(ErrorMessage = RequiredFieldErrorMsg)]
        [Display(Name = AddressName)]
        [StringLength(AddressMaxLength, MinimumLength = DefaultMinLength, ErrorMessage = LengthErrorMsg)]
        public string Address { get; set; }

        [StringLength(DescriptionMaxLength, MinimumLength = DefaultMinLength, ErrorMessage = LengthErrorMsg)]
        [Display(Name = BiographyName)]
        public string Biography { get; set; }

        [Display(Name = CityName)]
        public int CityId { get; set; }

        public IFormFile Picture { get; set; }

        public DateTime ModifiedOn { get; set; } = DateTime.UtcNow;

        public ICollection<CityFormModel> Cities { get; set; }
    }
}
