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
        private const string onlineConsultationName = "Онлайн консултация";

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

        public async Task<int> GetOnlineConsultationId()
            => await this.data.OfferedServices
                        .Where(x => x.Name == onlineConsultationName && x.IsDeleted == false)
                        .Select(x => x.Id)
                        .FirstOrDefaultAsync();
    }
}
