namespace Neonatology.Services.AppointmentCauseService;

using System.Collections.Generic;
using System.Threading.Tasks;
using Common;
using Neonatology.Common.Models;
using ViewModels.Appointments;

public interface IAppointmentCauseService : ITransientService
{
    Task<ICollection<AppointmentCauseViewModel>> GetAllCauses();

    Task<AppointmentCauseViewModel> GetAppointmentCauseByIdAsync(int id);

    Task<OperationResult> AppointmentCauseExists(int id);
}