﻿namespace Neonatology.ViewModels.Address;

using System.ComponentModel.DataAnnotations;
using Common.Mapping;
using Data.Models;
using static Common.Constants.GlobalConstants;
using static Data.Common.DataConstants.Constants;
    
public class EditAddressFormModel : IMapFrom<Address>
{
    public int Id { get; set; }

    [Required(ErrorMessage = MessageConstants.RequiredFieldErrorMsg)]
    [Display(Name = AccountConstants.AddressName)]
    [StringLength(AddressMaxLength, MinimumLength = DefaultMinLength, ErrorMessage = MessageConstants.LengthErrorMsg)]
    public string StreetName { get; set; }

    [Display(Name = AccountConstants.CityName)]
    public int CityId { get; set; }
}