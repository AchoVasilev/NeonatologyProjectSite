namespace Test.Mocks;

using System;
using Microsoft.EntityFrameworkCore;
using Neonatology.Data;

public static class DatabaseMock
{
    public static NeonatologyDbContext Instance
    {
        get
        {
            var dbContextOptions = new DbContextOptionsBuilder<NeonatologyDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new NeonatologyDbContext(dbContextOptions);
        }
    }
}