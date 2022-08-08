namespace Test.Helpers.Data;

using System.Collections.Generic;
using System.Threading.Tasks;
using global::Data;
using global::Data.Models;

public static class ApplicationUserData
{
    public static async Task TwoUsers(NeonatologyDbContext dataMock)
    {
        var users = new List<ApplicationUser>()
        {
            new ApplicationUser
            {
                Id = "firstUser",
                UserName = "gosho"
            },

            new ApplicationUser
            {
                Id = "secondUser",
                UserName = "evlogi"
            },
        };
        
        await dataMock.Users.AddRangeAsync(users);
        await dataMock.SaveChangesAsync();
    }
}