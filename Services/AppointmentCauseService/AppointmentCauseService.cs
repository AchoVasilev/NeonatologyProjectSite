namespace Services.AppointmentCauseService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    using Data;

    using Microsoft.EntityFrameworkCore;

    using ViewModels.Appointments;

    public class AppointmentCauseService : IAppointmentCauseService
    {
        private readonly NeonatologyDbContext data;
        private readonly IMapper mapper;

        public AppointmentCauseService(NeonatologyDbContext data, IMapper mapper)
        {
            this.data = data;
            this.mapper = mapper;
        }

        public async Task<ICollection<AppointmentCauseViewModel>> GetAllCauses()
            => await this.data.AppointmentCauses
                    .Where(x => x.IsDeleted == false)
                    .ProjectTo<AppointmentCauseViewModel>(this.mapper.ConfigurationProvider)
                    .ToListAsync();
    }
}
