namespace Test.Helpers;

using System.Threading.Tasks;
using Data;
using Data.Models;

public static class AddressData
{
    public static async Task GetOneAddress(NeonatologyDbContext dataMock)
    {
        var address = new Address
        {
            Id = 1,
            StreetName = "Kaspichan 49",
            City = new City
            {
                Name = "Kaspichan",
                ZipCode = 1234
            }
        };

        await dataMock.Addresses.AddAsync(address);
        await dataMock.SaveChangesAsync();
    }
}