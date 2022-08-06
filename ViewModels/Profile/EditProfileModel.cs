namespace ViewModels.Profile;

using System.Collections.Generic;
using City;
using Microsoft.AspNetCore.Http;

public class EditProfileModel
{
    public string Id { get; set; }

    public string UserId { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string PhoneNumber { get; set; }
        
    public int CityId { get; set; }

    public string UserImageUrl { get; set; }

    public string Email { get; set; }

    public IFormFile Picture { get; set; }

    public ICollection<CityFormModel> Cities { get; set; }
}