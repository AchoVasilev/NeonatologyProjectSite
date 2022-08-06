namespace ViewModels.Appointments;

using System.Collections.Generic;

using Common;
using Common.Models;

public class AllAppointmentsViewModel : PagingModel
{
    public ICollection<AppointmentViewModel> Appointments { get; set; }
}