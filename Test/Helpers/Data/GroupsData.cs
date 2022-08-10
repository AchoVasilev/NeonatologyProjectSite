namespace Test.Helpers.Data;

using System.Collections.Generic;
using System.Threading.Tasks;
using Neonatology.Data;
using Neonatology.Data.Models;

public class GroupsData
{
    public static async Task OneGroup(NeonatologyDbContext dataMock)
    {
        var group = new Group
        {
            Id = "firstGroup",
            Name = "evlogi->gosho"
        };
        
        await dataMock.Groups.AddAsync(group);
        await dataMock.SaveChangesAsync();
    }

    public static async Task TwoGroups(NeonatologyDbContext dataMock)
    {
        var groups = new List<Group>()
        {
            new Group
            {
                Id = "firstGroup",
                Name = "evlogi->gosho"
            },
            new Group
            {
                Id = "secondGroup",
                Name = "mancho->gosho"
            },
        };
        
        await dataMock.Groups.AddRangeAsync(groups);
        await dataMock.SaveChangesAsync();
    }
}