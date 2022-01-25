namespace Test.ServiceUnitTests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Data.Models;

    using global::Services.ChatService;

    using Microsoft.EntityFrameworkCore;

    using Test.Mocks;

    using ViewModels.Chat;

    using Xunit;

    public class ChatServiceTests
    {
        [Fact]
        public async Task SendMessageToUserShouldReturnCorrectReceiverId()
        {
            var dataMock = DatabaseMock.Instance;

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

            var group = new Group
            {
                Id = "firstGroup",
                Name = "evlogi->gosho"
            };

            await dataMock.Groups.AddAsync(group);
            await dataMock.Users.AddRangeAsync(users);
            await dataMock.SaveChangesAsync();

            var service = new ChatService(dataMock, null, null, null);
            var result = await service.SendMessageToUser("gosho", "evlogi", "mancho", "evlogi->gosho");

            Assert.NotNull(result);
            Assert.Equal("secondUser", result);
        }

        [Fact]
        public async Task SendMessageToUserShouldReturnNullIfSenderIsNotFound()
        {
            var dataMock = DatabaseMock.Instance;

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

            var group = new Group
            {
                Id = "firstGroup",
                Name = "evlogi->gosho"
            };

            await dataMock.Groups.AddAsync(group);
            await dataMock.Users.AddRangeAsync(users);
            await dataMock.SaveChangesAsync();

            var service = new ChatService(dataMock, null, null, null);
            var result = await service.SendMessageToUser("firstUser", "evlogi", "mancho", "evlogi->gosho");

            Assert.Null(result);
        }

        [Fact]
        public async Task SendMessageToUserShouldReturnNullIfReceiverIsNotFound()
        {
            var dataMock = DatabaseMock.Instance;

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

            var group = new Group
            {
                Id = "firstGroup",
                Name = "evlogi->gosho"
            };

            await dataMock.Groups.AddAsync(group);
            await dataMock.Users.AddRangeAsync(users);
            await dataMock.SaveChangesAsync();

            var service = new ChatService(dataMock, null, null, null);
            var result = await service.SendMessageToUser("gosho", "firstUser", "mancho", "evlogi->gosho");

            Assert.Null(result);
        }

        [Fact]
        public async Task AddUserToGroupShouldWorkCorrectly()
        {
            var dataMock = DatabaseMock.Instance;

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

            var service = new ChatService(dataMock, null, null, null);
            await service.AddUserToGroup("gosho->evlogi", "gosho", "evlogi");

            var group = dataMock.Groups.First();

            Assert.NotNull(group);
            Assert.Equal("gosho->evlogi", group.Name);
            Assert.Equal(1, dataMock.Groups.Count());
        }

        [Fact]
        public async Task ExtractAllMessagesShouldWorkCorrectly()
        {
            var dataMock = DatabaseMock.Instance;

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

            var group = new Group
            {
                Id = "firstGroup",
                Name = "evlogi->gosho"
            };

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
            };

            await dataMock.Groups.AddAsync(group);
            await dataMock.Users.AddRangeAsync(users);
            await dataMock.Messages.AddRangeAsync(messages);
            await dataMock.SaveChangesAsync();

            var service = new ChatService(dataMock, null, null, null);
            var result = await service.ExtractAllMessages("evlogi->gosho");

            Assert.Equal(3, result.Count);
        }

        [Fact]
        public async Task ExtractAllMessagesShouldReturnCorrectModel()
        {
            var dataMock = DatabaseMock.Instance;

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

            var group = new Group
            {
                Id = "firstGroup",
                Name = "evlogi->gosho"
            };

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
            };

            await dataMock.Groups.AddAsync(group);
            await dataMock.Users.AddRangeAsync(users);
            await dataMock.Messages.AddRangeAsync(messages);
            await dataMock.SaveChangesAsync();

            var service = new ChatService(dataMock, null, null, null);
            var result = await service.ExtractAllMessages("evlogi->gosho");

            Assert.IsAssignableFrom<ICollection<Message>>(result);
        }

        [Fact]
        public async Task ExtractAllMessagesShouldReturnCorrectCountIfThereAreMoreThanOneGroups()
        {
            var dataMock = DatabaseMock.Instance;

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

            await dataMock.Groups.AddRangeAsync(groups);
            await dataMock.Users.AddRangeAsync(users);
            await dataMock.Messages.AddRangeAsync(messages);
            await dataMock.SaveChangesAsync();

            var service = new ChatService(dataMock, null, null, null);
            var result = await service.ExtractAllMessages("mancho->gosho");

            Assert.Equal(1, result.Count);
        }

        [Fact]
        public async Task ExtractAllMessagesShouldReturnEmptyListOfMessageIfGroupIsNull()
        {
            var dataMock = DatabaseMock.Instance;

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

            await dataMock.Groups.AddRangeAsync(groups);
            await dataMock.Users.AddRangeAsync(users);
            await dataMock.Messages.AddRangeAsync(messages);
            await dataMock.SaveChangesAsync();

            var service = new ChatService(dataMock, null, null, null);
            var result = await service.ExtractAllMessages("mancho->pancho");

            Assert.Equal(0, result.Count);
            Assert.IsAssignableFrom<ICollection<Message>>(result);
        }

        [Fact]
        public async Task ExtractAllMessagesShouldReturnExactlyTenMessagesFromGroup()
        {
            var dataMock = DatabaseMock.Instance;

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

            await dataMock.Groups.AddRangeAsync(groups);
            await dataMock.Users.AddRangeAsync(users);
            await dataMock.Messages.AddRangeAsync(messages);
            await dataMock.SaveChangesAsync();

            var service = new ChatService(dataMock, null, null, null);
            var result = await service.ExtractAllMessages("evlogi->gosho");

            Assert.Equal(10, result.Count);
        }

        [Fact]
        public async Task ExtractAllMessagesShouldReturnOnlyMessagesThatAreNotDeleted()
        {
            var dataMock = DatabaseMock.Instance;

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

            var group = new Group
            {
                Id = "firstGroup",
                Name = "evlogi->gosho"
            };

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
                    IsDeleted = true
                },
                new Message
                {
                    GroupId = "firstGroup",
                    SenderId = "firstUser",
                    ReceiverId = "secondUser",
                    Content = "fasdasdasf"
                },
            };

            await dataMock.Groups.AddAsync(group);
            await dataMock.Users.AddRangeAsync(users);
            await dataMock.Messages.AddRangeAsync(messages);
            await dataMock.SaveChangesAsync();

            var service = new ChatService(dataMock, null, null, null);
            var result = await service.ExtractAllMessages("evlogi->gosho");

            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task LoadMoreMessagesShouldReturnCorrectMessageCount()
        {
            var dataMock = DatabaseMock.Instance;

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

            await dataMock.Groups.AddRangeAsync(groups);
            await dataMock.Users.AddRangeAsync(users);
            await dataMock.Messages.AddRangeAsync(messages);
            await dataMock.SaveChangesAsync();

            var firstUser = await dataMock.Users.FirstOrDefaultAsync(x => x.Id == "firstUser");
            var service = new ChatService(dataMock, null, null, null);

            var result = await service.LoadMoreMessages("evlogi->gosho", 3, firstUser, "Gosho Goshev", "Pesho Peshev");

            Assert.NotNull(result);
            Assert.Equal(10, result.Count);
        }

        [Fact]
        public async Task LoadMoreMessagesShouldReturnMessagesThatAreNotDeleted()
        {
            var dataMock = DatabaseMock.Instance;

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
                    IsDeleted = true
                },
            };

            await dataMock.Groups.AddRangeAsync(groups);
            await dataMock.Users.AddRangeAsync(users);
            await dataMock.Messages.AddRangeAsync(messages);
            await dataMock.SaveChangesAsync();

            var firstUser = await dataMock.Users.FirstOrDefaultAsync(x => x.Id == "firstUser");
            var service = new ChatService(dataMock, null, null, null);

            var result = await service.LoadMoreMessages("evlogi->gosho", 3, firstUser, "Gosho Goshev", "Pesho Peshev");

            Assert.NotNull(result);
            Assert.Equal(9, result.Count);
        }

        [Fact]
        public async Task LoadMoreMessagesShouldReturnCorrectModel()
        {
            var dataMock = DatabaseMock.Instance;

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

            await dataMock.Groups.AddRangeAsync(groups);
            await dataMock.Users.AddRangeAsync(users);
            await dataMock.Messages.AddRangeAsync(messages);
            await dataMock.SaveChangesAsync();

            var firstUser = await dataMock.Users.FirstOrDefaultAsync(x => x.Id == "firstUser");
            var service = new ChatService(dataMock, null, null, null);

            var result = await service.LoadMoreMessages("evlogi->gosho", 3, firstUser, "Gosho Goshev", "Pesho Peshev");

            Assert.NotNull(result);
            Assert.IsAssignableFrom<ICollection<LoadMoreMessagesViewModel>>(result);
        }

        [Fact]
        public async Task LoadMoreMessagesShouldReturnEmptyCollectionIfGroupIsNull()
        {
            var dataMock = DatabaseMock.Instance;

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

            await dataMock.Groups.AddRangeAsync(groups);
            await dataMock.Users.AddRangeAsync(users);
            await dataMock.Messages.AddRangeAsync(messages);
            await dataMock.SaveChangesAsync();

            var firstUser = await dataMock.Users.FirstOrDefaultAsync(x => x.Id == "firstUser");
            var service = new ChatService(dataMock, null, null, null);

            var result = await service.LoadMoreMessages("penio->gosho", 3, firstUser, "Gosho Goshev", "Pesho Peshev");

            Assert.NotNull(result);
            Assert.Equal(0, result.Count);
            Assert.IsAssignableFrom<ICollection<LoadMoreMessagesViewModel>>(result);
        }
    }
}
