namespace ViewModels.Address
{
    using System.ComponentModel.DataAnnotations;

    using City;

    using static Data.Common.DataConstants.Constants;

    public class AddressFormModel
    {
        public int Id { get; set; }

        [MaxLength(AddressMaxLength)]
        public string StreetName { get; set; }

        public CityFormModel City { get; set; }
    }
}
