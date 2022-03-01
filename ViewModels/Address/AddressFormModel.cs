namespace ViewModels.Address
{
    using System.ComponentModel.DataAnnotations;

    using ViewModels.City;

    using static Data.Common.DataConstants.Constants;

    public class AddressFormModel
    {
        [MaxLength(AddressMaxLength)]
        public string StreetName { get; set; }

        public CityFormModel City { get; set; }
    }
}
