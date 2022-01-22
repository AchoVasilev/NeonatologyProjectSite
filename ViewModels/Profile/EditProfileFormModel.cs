namespace ViewModels.Profile
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Http;

    using ViewModels.City;

    using static Data.Common.DataConstants.Constants;
    using static Common.GlobalConstants.Messages;
    using static Common.GlobalConstants.AccountConstants;

    public class EditProfileFormModel
    {
        public string Id { get; set; }

        public string UserId { get; set; }

        [Display(Name = Name)]
        [Required(ErrorMessage = RequiredFieldErrorMsg)]
        [StringLength(DefaultMaxLength, MinimumLength = DefaultMinLength, ErrorMessage = LengthErrorMsg)]
        public string FirstName { get; set; }

        [Display(Name = FamilyName)]
        [Required(ErrorMessage = RequiredFieldErrorMsg)]
        [StringLength(DefaultMaxLength, MinimumLength = DefaultMinLength, ErrorMessage = LengthErrorMsg)]
        public string LastName { get; set; }

        [Required(ErrorMessage = RequiredFieldErrorMsg)]
        [Phone]
        [Display(Name = Phone)]
        public string PhoneNumber { get; set; }
        
        [Display(Name = CityName)]
        public int CityId { get; set; }

        public string UserImageUrl { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public IFormFile Picture { get; set; }

        public ICollection<CityFormModel> Cities { get; set; }
    }
}
