namespace Services.OfferService
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    using Data;

    using Microsoft.EntityFrameworkCore;

    using ViewModels.Offer;

    public class OfferService : IOfferService
    {
        private readonly NeonatologyDbContext data;
        private readonly IMapper mapper;
        public OfferService(NeonatologyDbContext data, IMapper mapper)
        {
            this.data = data;
            this.mapper = mapper;
        }

        public async Task<ICollection<OfferViewModel>> GetAllAsync()
            => await data.OfferedServices
                .Where(x => x.IsDeleted == false)
                .ProjectTo<OfferViewModel>(mapper.ConfigurationProvider)
                .ToListAsync();
    }
}
