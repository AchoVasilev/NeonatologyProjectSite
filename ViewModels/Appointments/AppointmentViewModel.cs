namespace ViewModels.Appointments
{
    using System;

    using Address;

    public class AppointmentViewModel
    {
        public int Id { get; set; }

        public DateTime DateTime { get; set; }

        public DateTime End { get; set; }

        public string ParentFirstName { get; set; }

        public string ParentLastName { get; set; }

        public string PhoneNumber { get; set; }

        public string ChildFirstName { get; set; }

        public string AppointmentCauseName { get; set; }

        public string DoctorName { get; set; }

        public bool IsRated { get; set; }

        public int? Rating { get; set; }

        public string RatingComment { get; set; }

        public AddressFormModel Address { get; set; }

        public bool IsConfirmed { get; set; }
    }
}
