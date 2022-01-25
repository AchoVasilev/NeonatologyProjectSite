namespace Services.CityService
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using ViewModels.City;

    public interface ICityService
    {
        Task<ICollection<CityFormModel>> GetAllCities();

        Task<int> GetCityIdByName(string cityName);

        Task<CityFormModel> GetCityById(int id);
    }
}
