namespace Test.Helpers.Data;

using System.Collections.Generic;
using System.Threading.Tasks;
using global::Data;
using global::Data.Models;

public static class MessagesData
{
    public static async Task ThreeMessages(NeonatologyDbContext dataMock, bool isDeleted = false)
    {
        var messages = new List<Message>()
        {
            new Message
            {
                GroupId = "firstGroup",
                SenderId = "firstUser",
                ReceiverId = "secondUser",
                Content = "fasdasdasf"
            },
            new Message
            {
                GroupId = "firstGroup",
                SenderId = "firstUser",
                ReceiverId = "secondUser",
                Content = "fasdasdasf",
                IsDeleted = isDeleted
            },
            new Message
            {
                GroupId = "firstGroup",
                SenderId = "firstUser",
                ReceiverId = "secondUser",
                Content = "fasdasdasf"
            },
        };

        await dataMock.Messages.AddRangeAsync(messages);
        await dataMock.SaveChangesAsync();
    }

    public static async Task ThreeMessagesWithTwoGroups(NeonatologyDbContext dataMock)
    {
        var messages = new List<Message>()
        {
            new Message
            {
                GroupId = "secondGroup",
                SenderId = "firstUser",
                ReceiverId = "secondUser",
                Content = "fasdasdasf"
            },
            new Message
            {
                GroupId = "firstGroup",
                SenderId = "firstUser",
                ReceiverId = "secondUser",
                Content = "fasdasdasf"
            },
            new Message
            {
                GroupId = "firstGroup",
                SenderId = "firstUser",
                ReceiverId = "secondUser",
                Content = "fasdasdasf"
            },
        };

        await dataMock.Messages.AddRangeAsync(messages);
        await dataMock.SaveChangesAsync();
    }

    public static async Task TenMessages(NeonatologyDbContext dataMock, bool isDeleted = false)
    {
         var messages = new List<Message>()
        {
            new Message
            {
                GroupId = "firstGroup",
                SenderId = "firstUser",
                ReceiverId = "secondUser",
                Content = "fasdasdasf"
            },
            new Message
            {
                GroupId = "firstGroup",
                SenderId = "firstUser",
                ReceiverId = "secondUser",
                Content = "fasdasdasf"
            },
            new Message
            {
                GroupId = "firstGroup",
                SenderId = "firstUser",
                ReceiverId = "secondUser",
                Content = "fasdasdasf"
            },
            new Message
            {
                GroupId = "firstGroup",
                SenderId = "firstUser",
                ReceiverId = "secondUser",
                Content = "fasdasdasf"
            },
            new Message
            {
                GroupId = "firstGroup",
                SenderId = "firstUser",
                ReceiverId = "secondUser",
                Content = "fasdasdasf"
            },
            new Message
            {
                GroupId = "firstGroup",
                SenderId = "firstUser",
                ReceiverId = "secondUser",
                Content = "fasdasdasf"
            },
            new Message
            {
                GroupId = "firstGroup",
                SenderId = "firstUser",
                ReceiverId = "secondUser",
                Content = "fasdasdasf"
            },
            new Message
            {
                GroupId = "firstGroup",
                SenderId = "firstUser",
                ReceiverId = "secondUser",
                Content = "fasdasdasf"
            },
            new Message
            {
                GroupId = "firstGroup",
                SenderId = "firstUser",
                ReceiverId = "secondUser",
                Content = "fasdasdasf"
            },
            new Message
            {
                GroupId = "firstGroup",
                SenderId = "firstUser",
                ReceiverId = "secondUser",
                Content = "fasdasdasf"
            },
            new Message
            {
                GroupId = "firstGroup",
                SenderId = "firstUser",
                ReceiverId = "secondUser",
                Content = "fasdasdasf"
            },
            new Message
            {
                GroupId = "firstGroup",
                SenderId = "firstUser",
                ReceiverId = "secondUser",
                Content = "fasdasdasf"
            },
            new Message
            {
                GroupId = "firstGroup",
                SenderId = "firstUser",
                ReceiverId = "secondUser",
                Content = "fasdasdasf",
                IsDeleted = isDeleted
            },
        };
         
         await dataMock.Messages.AddRangeAsync(messages);
         await dataMock.SaveChangesAsync();
    }

    public static async Task ThirteenMessages(NeonatologyDbContext dataMock)
    {
        var messages = new List<Message>()
        {
            new Message
            {
                GroupId = "firstGroup",
                SenderId = "firstUser",
                ReceiverId = "secondUser",
                Content = "fasdasdasf"
            },
            new Message
            {
                GroupId = "firstGroup",
                SenderId = "firstUser",
                ReceiverId = "secondUser",
                Content = "fasdasdasf"
            },
            new Message
            {
                GroupId = "firstGroup",
                SenderId = "firstUser",
                ReceiverId = "secondUser",
                Content = "fasdasdasf"
            },
            new Message
            {
                GroupId = "firstGroup",
                SenderId = "firstUser",
                ReceiverId = "secondUser",
                Content = "fasdasdasf"
            },
            new Message
            {
                GroupId = "firstGroup",
                SenderId = "firstUser",
                ReceiverId = "secondUser",
                Content = "fasdasdasf"
            },
            new Message
            {
                GroupId = "firstGroup",
                SenderId = "firstUser",
                ReceiverId = "secondUser",
                Content = "fasdasdasf"
            },
            new Message
            {
                GroupId = "firstGroup",
                SenderId = "firstUser",
                ReceiverId = "secondUser",
                Content = "fasdasdasf"
            },
            new Message
            {
                GroupId = "firstGroup",
                SenderId = "firstUser",
                ReceiverId = "secondUser",
                Content = "fasdasdasf"
            },
            new Message
            {
                GroupId = "firstGroup",
                SenderId = "firstUser",
                ReceiverId = "secondUser",
                Content = "fasdasdasf"
            },
            new Message
            {
                GroupId = "firstGroup",
                SenderId = "firstUser",
                ReceiverId = "secondUser",
                Content = "fasdasdasf"
            },
            new Message
            {
                GroupId = "firstGroup",
                SenderId = "firstUser",
                ReceiverId = "secondUser",
                Content = "fasdasdasf"
            },
            new Message
            {
                GroupId = "firstGroup",
                SenderId = "firstUser",
                ReceiverId = "secondUser",
                Content = "fasdasdasf"
            },
            new Message
            {
                GroupId = "firstGroup",
                SenderId = "firstUser",
                ReceiverId = "secondUser",
                Content = "fasdasdasf"
            },
        };
        
        await dataMock.Messages.AddRangeAsync(messages);
        await dataMock.SaveChangesAsync();
    }
}