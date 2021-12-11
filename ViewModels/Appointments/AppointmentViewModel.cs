namespace ViewModels.Appointments
{
    using System;

    public class AppointmentViewModel
    {
        public int Id { get; set; }

        public DateTime DateTime { get; set; }

        public string ParentFirstName { get; set; }

        public string ParentLastName { get; set; }

        public string PhoneNumber { get; set; }

        public string ChildFirstName { get; set; }

        public string AppointmentCause { get; set; }

        public string DoctorName { get; set; }

        public bool IsRated { get; set; }

        public string Address { get; set; }
    }
}
