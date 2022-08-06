namespace ViewModels.Doctor;

using System;
using System.Collections.Generic;
using Address;
using City;
using Microsoft.AspNetCore.Http;

public class DoctorEditModel
{
    public string Id { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string UserImageUrl { get; set; }

    public int Age { get; set; }

    public string PhoneNumber { get; set; }

    public int? YearsOfExperience { get; set; }

    public string Email { get; set; }

    public string Biography { get; set; }

    public IFormFile Picture { get; set; }

    public DateTime ModifiedOn { get; set; }

    public ICollection<EditAddressFormModel> Addresses { get; set; }

    public ICollection<CityFormModel> Cities { get; set; }
}