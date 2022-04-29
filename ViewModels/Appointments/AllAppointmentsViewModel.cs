namespace ViewModels.Appointments
{
    using System.Collections.Generic;

    using Common;

    public class AllAppointmentsViewModel : PagingModel
    {
        public ICollection<AppointmentViewModel> Appointments { get; set; }
    }
}
