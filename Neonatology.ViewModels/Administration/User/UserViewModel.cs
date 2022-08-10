namespace Neonatology.ViewModels.Administration.User;

using System;
using Common.Mapping;
using Patient;

public class UserViewModel : IMapFrom<PatientServiceModel>
{
    public string UserId { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string ImageUrl { get; set; }

    public string CityName { get; set; }

    public string Phone { get; set; }

    public string UserUserName { get; set; }

    public DateTime CreatedOn { get; set; }
}