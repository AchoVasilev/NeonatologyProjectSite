namespace Services.CityService
{
    using System.Collections.Generic;

    using ViewModels.City;

    public interface ICityService
    {
        ICollection<CityFormModel> GetAllCities();
    }
}
