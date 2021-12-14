namespace ViewModels.Appointments
{
    using System;

    public class AppointmentViewModel
    {
        public int Id { get; set; }

        public DateTime DateTime { get; set; }

        public DateTime End { get; set; }

        public string PatientFirstName { get; set; }

        public string PatientLastName { get; set; }

        public string PatientPhone { get; set; }

        public string ChildFirstName { get; set; }

        public string AppointmentCause { get; set; }

        public string DoctorName { get; set; }

        public bool IsRated { get; set; }

        public int? Rating { get; set; }

        public string RatingComment { get; set; }

        public string Address { get; set; }

        public bool IsConfirmed { get; set; }
    }
}
