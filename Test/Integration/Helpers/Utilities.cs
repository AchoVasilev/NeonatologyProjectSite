namespace Test.Integration.Helpers
{
    using System.Collections.Generic;

    using Data;
    using Data.Models;

    public static class Utilities
    {
        #region snippet1
        public static void InitializeDbForTests(NeonatologyDbContext db)
        {
            db.Messages.AddRange(GetSeedingMessages());
            db.SaveChanges();
        }

        public static void ReinitializeDbForTests(NeonatologyDbContext db)
        {
            db.Messages.RemoveRange(db.Messages);
            InitializeDbForTests(db);
        }

        public static List<Message> GetSeedingMessages()
        {
            return new List<Message>()
            {
                new Message(){ Content = "TEST RECORD: You're standing on my scarf." },
                new Message(){ Content = "TEST RECORD: Would you like a jelly baby?" },
                new Message(){ Content = "TEST RECORD: To the rational mind, " +
                    "nothing is inexplicable; only unexplained." }
            };
        }
        #endregion
    }
}
