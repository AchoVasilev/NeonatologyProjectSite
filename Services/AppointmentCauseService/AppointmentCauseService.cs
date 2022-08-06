namespace Services.AppointmentCauseService;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common.Models;
using Data;

using Microsoft.EntityFrameworkCore;

using ViewModels.Appointments;
using static Common.Constants.GlobalConstants.MessageConstants;
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
            .AsNoTracking()
            .ProjectTo<AppointmentCauseViewModel>(this.mapper.ConfigurationProvider)
            .ToListAsync();

    public async Task<AppointmentCauseViewModel> GetAppointmentCauseByIdAsync(int id)
        => await this.data.AppointmentCauses
            .Where(x => x.Id == id && x.IsDeleted == false)
            .AsNoTracking()
            .ProjectTo<AppointmentCauseViewModel>(this.mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();

    public async Task<OperationResult> AppointmentCauseExists(int id)
    {
        var exists = await this.data.AppointmentCauses
            .AnyAsync(x => x.Id == id && x.IsDeleted == false);

        if (exists)
        {
            return AppointmentCauseWrongId;
        }

        return true;
    }
}