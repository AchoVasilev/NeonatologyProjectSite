namespace Services.CityService
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    using Data;

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

        public ICollection<CityFormModel> GetAllCities() 
            => this.data.Cities
                                   .ProjectTo<CityFormModel>(this.mapper.ConfigurationProvider)
                                   .ToList();
    }
}
