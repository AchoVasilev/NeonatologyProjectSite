namespace Services.CityService
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    using Data;

    using Microsoft.EntityFrameworkCore;

    using ViewModels.City;

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
                         .ProjectTo<CityFormModel>(this.mapper.ConfigurationProvider)
                         .OrderBy(x => x.Name)
                         .ToListAsync();
    }
}
