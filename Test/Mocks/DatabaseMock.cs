namespace Test.Mocks;

using System;

using Data;

using Microsoft.EntityFrameworkCore;

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