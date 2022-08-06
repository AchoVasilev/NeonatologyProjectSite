namespace Services.CityService;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common;
using Data;

using Microsoft.EntityFrameworkCore;

using ViewModels.Address;
using ViewModels.City;

using static Common.GlobalConstants.MessageConstants;
public class CityService : ICityService
{
    private readonly NeonatologyDbContext data;
    private readonly IMapper mapper;

    public CityService(NeonatologyDbContext data, IMapper mapper)
    {
        this.data = data;
        this.mapper = mapper;
    }

    public async Task<ICollection<CityFormModel>> GetAllCities()
        => await this.data.Cities
            .Where(x => x.IsDeleted == false)
            .ProjectTo<CityFormModel>(this.mapper.ConfigurationProvider)
            .OrderBy(x => x.Name)
            .AsNoTracking()
            .ToListAsync();

    public async Task<int> GetCityIdByName(string cityName)
        => await this.data.Cities
            .Where(x => x.Name == cityName)
            .Select(x => x.Id)
            .FirstOrDefaultAsync();

    public async Task<CityFormModel> GetCityById(int id)
        => await this.data.Cities
            .Where(x => x.Id == id && x.IsDeleted == false)
            .ProjectTo<CityFormModel>(this.mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();

    public async Task<OperationResult> CityExists(int id)
    {
        var exists = await this.data.Cities.AnyAsync(x => x.Id == id && x.IsDeleted == false);
        if (!exists)
        {
            return CityDoesNotExistErrorMsg;
        }

        return true;
    }

    public async Task<ICollection<AddressFormModel>> GetDoctorAddressesByDoctorId(string doctorId)
        => await this.data.Addresses
            .Where(x => x.DoctorId == doctorId)
            .AsNoTracking()
            .ProjectTo<AddressFormModel>(this.mapper.ConfigurationProvider)
            .ToListAsync();
}