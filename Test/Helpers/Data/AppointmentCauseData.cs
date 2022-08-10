namespace Test.Helpers.Data;

using System.Collections.Generic;
using System.Threading.Tasks;
using Neonatology.Data;
using Neonatology.Data.Models;

public static class AppointmentCauseData
{
    public static async Task GetCauses(NeonatologyDbContext databaseMock, bool isDeleted = false)
    {
        var causes = new List<AppointmentCause>()
        {
            new AppointmentCause { Id = 1, Name = "Start", IsDeleted = isDeleted},
            new AppointmentCause { Id = 2, Name = "End" },
            new AppointmentCause { Id = 3, Name = "Middle" },
        };

        await databaseMock.AppointmentCauses.AddRangeAsync(causes);
        await databaseMock.SaveChangesAsync();
    }
}