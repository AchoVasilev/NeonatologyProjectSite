namespace Neonatology.ViewModels.Profile;

using Common.Mapping;
using Data.Models;

public class ProfileViewModel : IMapFrom<Patient>
{
    public string Id { get; init; }

    public string UserId { get; init; }

    public string FirstName { get; init; }

    public string LastName { get; init; }

    public string UserImageUrl { get; init; }

    public string Phone { get; init; }

    public string CityName { get; init; }

    public string UserEmail { get; init; }
}