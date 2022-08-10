namespace Test.ServiceUnitTests;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Helpers.Data;
using Microsoft.EntityFrameworkCore;
using Mocks;
using Neonatology.Data.Models;
using Neonatology.Services.ChatService;
using Neonatology.ViewModels.Chat;
using Xunit;

public class ChatServiceTests
{
    [Fact]
    public async Task SendMessageToUserShouldReturnCorrectReceiverId()
    {
        var dataMock = DatabaseMock.Instance;

        await ApplicationUserData.TwoUsers(dataMock);
        await GroupsData.OneGroup(dataMock);

        var service = new ChatService(dataMock, null, null, null);
        var result = await service.SendMessageToUser("gosho", "evlogi", "mancho", "evlogi->gosho");

        Assert.NotNull(result);
        Assert.Equal("secondUser", result);
    }

    [Fact]
    public async Task SendMessageToUserShouldReturnNullIfSenderIsNotFound()
    {
        var dataMock = DatabaseMock.Instance;

        await ApplicationUserData.TwoUsers(dataMock);
        await GroupsData.OneGroup(dataMock);

        var service = new ChatService(dataMock, null, null, null);
        var result = await service.SendMessageToUser("firstUser", "evlogi", "mancho", "evlogi->gosho");

        Assert.Null(result);
    }

    [Fact]
    public async Task SendMessageToUserShouldReturnNullIfReceiverIsNotFound()
    {
        var dataMock = DatabaseMock.Instance;

        await ApplicationUserData.TwoUsers(dataMock);
        await GroupsData.OneGroup(dataMock);

        var service = new ChatService(dataMock, null, null, null);
        var result = await service.SendMessageToUser("gosho", "firstUser", "mancho", "evlogi->gosho");

        Assert.Null(result);
    }

    [Fact]
    public async Task AddUserToGroupShouldWorkCorrectly()
    {
        var dataMock = DatabaseMock.Instance;

        await ApplicationUserData.TwoUsers(dataMock);

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

        await ApplicationUserData.TwoUsers(dataMock);
        await GroupsData.OneGroup(dataMock);
        await MessagesData.ThreeMessages(dataMock);

        var service = new ChatService(dataMock, null, null, null);
        var result = await service.ExtractAllMessages("evlogi->gosho");

        Assert.Equal(3, result.Count);
    }

    [Fact]
    public async Task ExtractAllMessagesShouldReturnCorrectModel()
    {
        var dataMock = DatabaseMock.Instance;

        await ApplicationUserData.TwoUsers(dataMock);
        await GroupsData.OneGroup(dataMock);
        await MessagesData.ThreeMessages(dataMock);

        var service = new ChatService(dataMock, null, null, null);
        var result = await service.ExtractAllMessages("evlogi->gosho");

        Assert.IsAssignableFrom<ICollection<Message>>(result);
    }

    [Fact]
    public async Task ExtractAllMessagesShouldReturnCorrectCountIfThereAreMoreThanOneGroups()
    {
        var dataMock = DatabaseMock.Instance;

        await ApplicationUserData.TwoUsers(dataMock);
        await GroupsData.TwoGroups(dataMock);
        await MessagesData.ThreeMessagesWithTwoGroups(dataMock);

        var service = new ChatService(dataMock, null, null, null);
        var result = await service.ExtractAllMessages("mancho->gosho");

        Assert.Equal(1, result.Count);
    }

    [Fact]
    public async Task ExtractAllMessagesShouldReturnEmptyListOfMessageIfGroupIsNull()
    {
        var dataMock = DatabaseMock.Instance;

        await ApplicationUserData.TwoUsers(dataMock);
        await GroupsData.TwoGroups(dataMock);
        await MessagesData.ThreeMessagesWithTwoGroups(dataMock);

        var service = new ChatService(dataMock, null, null, null);
        var result = await service.ExtractAllMessages("mancho->pancho");

        Assert.Equal(0, result.Count);
        Assert.IsAssignableFrom<ICollection<Message>>(result);
    }

    [Fact]
    public async Task ExtractAllMessagesShouldReturnExactlyTenMessagesFromGroup()
    {
        var dataMock = DatabaseMock.Instance;

        await ApplicationUserData.TwoUsers(dataMock);
        await GroupsData.TwoGroups(dataMock);
        await MessagesData.TenMessages(dataMock);

        var service = new ChatService(dataMock, null, null, null);
        var result = await service.ExtractAllMessages("evlogi->gosho");

        Assert.Equal(10, result.Count);
    }

    [Fact]
    public async Task ExtractAllMessagesShouldReturnOnlyMessagesThatAreNotDeleted()
    {
        var dataMock = DatabaseMock.Instance;

        await ApplicationUserData.TwoUsers(dataMock);
        await GroupsData.TwoGroups(dataMock);
        await MessagesData.ThreeMessages(dataMock, true);

        var service = new ChatService(dataMock, null, null, null);
        var result = await service.ExtractAllMessages("evlogi->gosho");

        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task LoadMoreMessagesShouldReturnCorrectMessageCount()
    {
        var dataMock = DatabaseMock.Instance;

        await ApplicationUserData.TwoUsers(dataMock);
        await GroupsData.TwoGroups(dataMock);
        await MessagesData.ThirteenMessages(dataMock);

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

        await ApplicationUserData.TwoUsers(dataMock);
        await GroupsData.TwoGroups(dataMock);
        await MessagesData.TenMessages(dataMock, true);

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

        await ApplicationUserData.TwoUsers(dataMock);
        await GroupsData.TwoGroups(dataMock);
        await MessagesData.ThirteenMessages(dataMock);

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

        await ApplicationUserData.TwoUsers(dataMock);
        await GroupsData.TwoGroups(dataMock);
        await MessagesData.ThirteenMessages(dataMock);

        var firstUser = await dataMock.Users.FirstOrDefaultAsync(x => x.Id == "firstUser");
        var service = new ChatService(dataMock, null, null, null);

        var result = await service.LoadMoreMessages("penio->gosho", 3, firstUser, "Gosho Goshev", "Pesho Peshev");

        Assert.NotNull(result);
        Assert.Equal(0, result.Count);
        Assert.IsAssignableFrom<ICollection<LoadMoreMessagesViewModel>>(result);
    }
}