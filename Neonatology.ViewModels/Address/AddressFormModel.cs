namespace Neonatology.ViewModels.Address;

using System.ComponentModel.DataAnnotations;
using City;
using Common.Mapping;
using Data.Models;
using static Data.Common.DataConstants.Constants;

public class AddressFormModel : IMapFrom<Address>
{
    public int Id { get; set; }

    [MaxLength(AddressMaxLength)]
    public string StreetName { get; set; }

    public CityFormModel City { get; set; }
}