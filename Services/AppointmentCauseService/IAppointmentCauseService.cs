namespace Services.AppointmentCauseService;

using System.Collections.Generic;
using System.Threading.Tasks;

using ViewModels.Appointments;

public interface IAppointmentCauseService
{
    Task<ICollection<AppointmentCauseViewModel>> GetAllCauses();

    Task<AppointmentCauseViewModel> GetAppointmentCauseByIdAsync(int id);
}