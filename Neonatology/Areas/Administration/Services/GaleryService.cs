namespace Neonatology.Areas.Administration.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    using Data;

    using Microsoft.EntityFrameworkCore;

    using Neonatology.Areas.Administration.ViewModels.Galery;

    public class GaleryService : IGaleryService
    {
        private readonly NeonatologyDbContext data;
        private readonly IMapper mapper;

        public GaleryService(NeonatologyDbContext data, IMapper mapper)
        {
            this.data = data;
            this.mapper = mapper;
        }

        public async Task<ICollection<GaleryViewModel>> GetGaleryImages()
            => await this.data.Images
                .Where(x => string.IsNullOrWhiteSpace(x.DoctorId) && x.IsDeleted == false)
                .OrderByDescending(x => x.CreatedOn)
                .ProjectTo<GaleryViewModel>(this.mapper.ConfigurationProvider)
                .ToListAsync();
    }
}
