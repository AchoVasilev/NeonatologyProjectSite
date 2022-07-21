namespace ViewModels.Doctor
{
    using System.Collections.Generic;

    using Address;

    public class DoctorProfileViewModel
    {
        public string Id { get; set; }

        public string FullName { get; set; }

        public string UserImageUrl { get; set; }

        public int Age { get; set; }

        public string PhoneNumber { get; set; }

        public int? YearsOfExperience { get; set; }

        public string Email { get; set; }

        public string Biography { get; set; }

        public ICollection<AddressFormModel> Addresses { get; set; }

        public ICollection<SpecializationFormModel> Specializations { get; set; }
    }
}
