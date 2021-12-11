namespace ViewModels.Doctor
{
    using System.Collections.Generic;

    public class DoctorProfileViewModel
    {
        public string FullName { get; set; }

        public string ImageUrl { get; set; }

        public int Age { get; set; }

        public string PhoneNumber { get; set; }

        public int? YearsOfExperience { get; set; }

        public string Email { get; set; }

        public string Address { get; set; }

        public string Biography { get; set; }

        public string CityName { get; set; }

        public ICollection<SpecializationFormModel> Specializations { get; set; }
    }
}
