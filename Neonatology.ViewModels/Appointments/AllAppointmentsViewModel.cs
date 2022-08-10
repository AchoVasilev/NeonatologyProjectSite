namespace Neonatology.ViewModels.Appointments;

using System.Collections.Generic;
using Common.Models;

public class AllAppointmentsViewModel : PagingModel
{
    public ICollection<AppointmentViewModel> Appointments { get; set; }
}