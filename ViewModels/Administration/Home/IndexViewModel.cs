namespace ViewModels.Administration.Home
{
    using System.Collections.Generic;

    using ViewModels.Appointments;

    public class IndexViewModel
    {
        public int TotalPatientsCount { get; set; }

        public int TotalAppointmentsCount { get; set; }

        public int LatestPatientsRegisterCount { get; set; }

        public int TotalRatingsCount { get; set; }

        public ICollection<AppointmentViewModel> AllAppointments { get; set; }
    }
}
