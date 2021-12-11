namespace ViewModels.Appointments
{
    using System.Collections.Generic;

    public class AllAppointmentsViewModel
    {
        public ICollection<AppointmentViewModel> Upcoming { get; set; }

        public ICollection<AppointmentViewModel> Past { get; set; }
    }
}
