namespace Test.ServiceUnitTests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Data.Models;
    using Data.Models.Enums;

    using global::Services.NotificationService;

    using Microsoft.EntityFrameworkCore;

    using Test.Mocks;

    using Xunit;

    public class NotificationServiceTests
    {
        [Fact]
        public async Task GetUserNotificationsCountShouldReturnCorrectCount()
        {
            var dataMock = DatabaseMock.Instance;
            var sender = new ApplicationUser
            {
                Id = "sender",
                UserName = "gosho@gosho.com"
            };

            var receiver = new ApplicationUser
            {
                Id = "receiver",
                UserName = "pesho@pesho.com"
            };

            var notificationType = new NotificationType()
            {
                Id = 1,
                Name = "message"
            };

            var notifications = new List<Notification>
            {
                new Notification
                {
                    Id = "gosho",
                    Link = "goshogosho.com",
                    Text = "goshogoshogosho",
                    NotificationTypeId = 1,
                    SenderId = "sender",
                    ReceiverId = "receiver",
                    NotificationStatus = NotificationStatus.Непрочетено
                },
                new Notification
                {
                    Id = "mancho",
                    Link = "goshogosho.com",
                    Text = "goshogoshogosho",
                    NotificationTypeId = 1,
                    SenderId = "sender",
                    ReceiverId = "receiver",
                    NotificationStatus = NotificationStatus.Непрочетено
                },
                new Notification
                {
                    Id = "evlogi",
                    Link = "goshogosho.com",
                    Text = "goshogoshogosho",
                    NotificationTypeId = 1,
                    SenderId = "sender",
                    ReceiverId = "receiver",
                    NotificationStatus = NotificationStatus.Непрочетено
                },
            };

            await dataMock.Users.AddRangeAsync(sender);
            await dataMock.Users.AddRangeAsync(receiver);
            await dataMock.NotificationTypes.AddAsync(notificationType);
            await dataMock.Notifications.AddRangeAsync(notifications);

            await dataMock.SaveChangesAsync();

            var service = new NotificationService(dataMock);
            var count = await service.GetUserNotificationsCount("pesho@pesho.com");

            Assert.Equal(3, count);
        }

        [Fact]
        public async Task GetUserNotificationsCountShouldReturnCorrectCountIfOneIsDeleted()
        {
            var dataMock = DatabaseMock.Instance;
            var sender = new ApplicationUser
            {
                Id = "sender",
                UserName = "gosho@gosho.com"
            };

            var receiver = new ApplicationUser
            {
                Id = "receiver",
                UserName = "pesho@pesho.com"
            };

            var notificationType = new NotificationType()
            {
                Id = 1,
                Name = "message"
            };

            var notifications = new List<Notification>
            {
                new Notification
                {
                    Id = "gosho",
                    Link = "goshogosho.com",
                    Text = "goshogoshogosho",
                    NotificationTypeId = 1,
                    SenderId = "sender",
                    ReceiverId = "receiver",
                    NotificationStatus = NotificationStatus.Непрочетено
                },
                new Notification
                {
                    Id = "mancho",
                    Link = "goshogosho.com",
                    Text = "goshogoshogosho",
                    NotificationTypeId = 1,
                    SenderId = "sender",
                    ReceiverId = "receiver",
                    NotificationStatus = NotificationStatus.Непрочетено
                },
                new Notification
                {
                    Id = "evlogi",
                    Link = "goshogosho.com",
                    Text = "goshogoshogosho",
                    NotificationTypeId = 1,
                    SenderId = "sender",
                    ReceiverId = "receiver",
                    NotificationStatus = NotificationStatus.Непрочетено,
                    IsDeleted = true
                },
            };

            await dataMock.Users.AddRangeAsync(sender);
            await dataMock.Users.AddRangeAsync(receiver);
            await dataMock.NotificationTypes.AddAsync(notificationType);
            await dataMock.Notifications.AddRangeAsync(notifications);

            await dataMock.SaveChangesAsync();

            var service = new NotificationService(dataMock);
            var count = await service.GetUserNotificationsCount("pesho@pesho.com");

            Assert.Equal(2, count);
        }

        [Fact]
        public async Task GetUserNotificationsCountShouldReturnCorrectCountIfStatusIsDifferent()
        {
            var dataMock = DatabaseMock.Instance;
            var sender = new ApplicationUser
            {
                Id = "sender",
                UserName = "gosho@gosho.com"
            };

            var receiver = new ApplicationUser
            {
                Id = "receiver",
                UserName = "pesho@pesho.com"
            };

            var notificationType = new NotificationType()
            {
                Id = 1,
                Name = "message"
            };

            var notifications = new List<Notification>
            {
                new Notification
                {
                    Id = "gosho",
                    Link = "goshogosho.com",
                    Text = "goshogoshogosho",
                    NotificationTypeId = 1,
                    SenderId = "sender",
                    ReceiverId = "receiver",
                    NotificationStatus = NotificationStatus.Непрочетено
                },
                new Notification
                {
                    Id = "mancho",
                    Link = "goshogosho.com",
                    Text = "goshogoshogosho",
                    NotificationTypeId = 1,
                    SenderId = "sender",
                    ReceiverId = "receiver",
                    NotificationStatus = NotificationStatus.Непрочетено
                },
                new Notification
                {
                    Id = "evlogi",
                    Link = "goshogosho.com",
                    Text = "goshogoshogosho",
                    NotificationTypeId = 1,
                    SenderId = "sender",
                    ReceiverId = "receiver",
                    NotificationStatus = NotificationStatus.Прочетено
                },
            };

            await dataMock.Users.AddRangeAsync(sender);
            await dataMock.Users.AddRangeAsync(receiver);
            await dataMock.NotificationTypes.AddAsync(notificationType);
            await dataMock.Notifications.AddRangeAsync(notifications);

            await dataMock.SaveChangesAsync();

            var service = new NotificationService(dataMock);
            var count = await service.GetUserNotificationsCount("pesho@pesho.com");

            Assert.Equal(2, count);
        }

        [Fact]
        public async Task AddMessageNotificationShouldReturnAddNotificationToDatabaseWhenSuccessful()
        {
            var dataMock = DatabaseMock.Instance;
            var sender = new ApplicationUser
            {
                Id = "sender",
                UserName = "gosho@gosho.com"
            };

            var receiver = new ApplicationUser
            {
                Id = "receiver",
                UserName = "pesho@pesho.com"
            };

            var notificationType = new NotificationType()
            {
                Id = 1,
                Name = "Message"
            };

            await dataMock.Users.AddRangeAsync(sender);
            await dataMock.Users.AddRangeAsync(receiver);
            await dataMock.NotificationTypes.AddAsync(notificationType);

            await dataMock.SaveChangesAsync();

            var service = new NotificationService(dataMock);
            var result = await service.AddMessageNotification("gosho", receiver.UserName, sender.UserName, "gosho->pesho");

            Assert.Equal(1, dataMock.Notifications.Count());
        }

        [Fact]
        public async Task UpdateMessageNotificationsShouldReturnReceiverIdWhenSuccessful()
        {
            var dataMock = DatabaseMock.Instance;
            var sender = new ApplicationUser
            {
                Id = "sender",
                UserName = "gosho@gosho.com"
            };

            var receiver = new ApplicationUser
            {
                Id = "receiver",
                UserName = "pesho@pesho.com"
            };

            var notificationType = new NotificationType()
            {
                Id = 1,
                Name = "message"
            };

            var notifications = new List<Notification>
            {
                new Notification
                {
                    Id = "gosho",
                    Link = "goshogosho.com",
                    Text = "goshogoshogosho",
                    NotificationTypeId = 1,
                    SenderId = "sender",
                    ReceiverId = "receiver",
                    NotificationStatus = NotificationStatus.Непрочетено
                },
                new Notification
                {
                    Id = "mancho",
                    Link = "goshogosho.com",
                    Text = "goshogoshogosho",
                    NotificationTypeId = 1,
                    SenderId = "sender",
                    ReceiverId = "receiver",
                    NotificationStatus = NotificationStatus.Непрочетено
                },
                new Notification
                {
                    Id = "evlogi",
                    Link = "goshogosho.com",
                    Text = "goshogoshogosho",
                    NotificationTypeId = 1,
                    SenderId = "sender",
                    ReceiverId = "receiver",
                    NotificationStatus = NotificationStatus.Прочетено
                },
            };

            await dataMock.Users.AddRangeAsync(sender);
            await dataMock.Users.AddRangeAsync(receiver);
            await dataMock.NotificationTypes.AddAsync(notificationType);
            await dataMock.Notifications.AddRangeAsync(notifications);

            await dataMock.SaveChangesAsync();

            var service = new NotificationService(dataMock);
            var result = await service.UpdateMessageNotifications(sender.UserName, receiver.UserName);

            Assert.Equal("receiver", result);
        }

        [Fact]
        public async Task UpdateMessageNotificationsShouldReturnEmptyStringIfSenderIsNull()
        {
            var dataMock = DatabaseMock.Instance;
            var sender = new ApplicationUser
            {
                Id = "sender",
                UserName = "gosho@gosho.com"
            };

            var receiver = new ApplicationUser
            {
                Id = "receiver",
                UserName = "pesho@pesho.com"
            };

            var notificationType = new NotificationType()
            {
                Id = 1,
                Name = "message"
            };

            var notifications = new List<Notification>
            {
                new Notification
                {
                    Id = "gosho",
                    Link = "goshogosho.com",
                    Text = "goshogoshogosho",
                    NotificationTypeId = 1,
                    SenderId = "sender",
                    ReceiverId = "receiver",
                    NotificationStatus = NotificationStatus.Непрочетено
                },
                new Notification
                {
                    Id = "mancho",
                    Link = "goshogosho.com",
                    Text = "goshogoshogosho",
                    NotificationTypeId = 1,
                    SenderId = "sender",
                    ReceiverId = "receiver",
                    NotificationStatus = NotificationStatus.Непрочетено
                },
                new Notification
                {
                    Id = "evlogi",
                    Link = "goshogosho.com",
                    Text = "goshogoshogosho",
                    NotificationTypeId = 1,
                    SenderId = "sender",
                    ReceiverId = "receiver",
                    NotificationStatus = NotificationStatus.Прочетено
                },
            };

            await dataMock.Users.AddRangeAsync(sender);
            await dataMock.Users.AddRangeAsync(receiver);
            await dataMock.NotificationTypes.AddAsync(notificationType);
            await dataMock.Notifications.AddRangeAsync(notifications);

            await dataMock.SaveChangesAsync();

            var service = new NotificationService(dataMock);
            var result = await service.UpdateMessageNotifications("fifi", receiver.UserName);

            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public async Task UpdateMessageNotificationsShouldReturnEmptyStringIfReceiverIsNull()
        {
            var dataMock = DatabaseMock.Instance;
            var sender = new ApplicationUser
            {
                Id = "sender",
                UserName = "gosho@gosho.com"
            };

            var receiver = new ApplicationUser
            {
                Id = "receiver",
                UserName = "pesho@pesho.com"
            };

            var notificationType = new NotificationType()
            {
                Id = 1,
                Name = "message"
            };

            var notifications = new List<Notification>
            {
                new Notification
                {
                    Id = "gosho",
                    Link = "goshogosho.com",
                    Text = "goshogoshogosho",
                    NotificationTypeId = 1,
                    SenderId = "sender",
                    ReceiverId = "receiver",
                    NotificationStatus = NotificationStatus.Непрочетено
                },
                new Notification
                {
                    Id = "mancho",
                    Link = "goshogosho.com",
                    Text = "goshogoshogosho",
                    NotificationTypeId = 1,
                    SenderId = "sender",
                    ReceiverId = "receiver",
                    NotificationStatus = NotificationStatus.Непрочетено
                },
                new Notification
                {
                    Id = "evlogi",
                    Link = "goshogosho.com",
                    Text = "goshogoshogosho",
                    NotificationTypeId = 1,
                    SenderId = "sender",
                    ReceiverId = "receiver",
                    NotificationStatus = NotificationStatus.Прочетено
                },
            };

            await dataMock.Users.AddRangeAsync(sender);
            await dataMock.Users.AddRangeAsync(receiver);
            await dataMock.NotificationTypes.AddAsync(notificationType);
            await dataMock.Notifications.AddRangeAsync(notifications);

            await dataMock.SaveChangesAsync();

            var service = new NotificationService(dataMock);
            var result = await service.UpdateMessageNotifications(sender.UserName, "fifi");

            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public async Task EditStatusShouldReturnTrueIfEverythingIsSuccessful()
        {
            var dataMock = DatabaseMock.Instance;
            var sender = new ApplicationUser
            {
                Id = "sender",
                UserName = "gosho@gosho.com"
            };

            var receiver = new ApplicationUser
            {
                Id = "receiver",
                UserName = "pesho@pesho.com"
            };

            var notificationType = new NotificationType()
            {
                Id = 1,
                Name = "message"
            };

            var notifications = new List<Notification>
            {
                new Notification
                {
                    Id = "gosho",
                    Link = "goshogosho.com",
                    Text = "goshogoshogosho",
                    NotificationTypeId = 1,
                    SenderId = "sender",
                    ReceiverId = "receiver",
                    NotificationStatus = NotificationStatus.Непрочетено
                },
                new Notification
                {
                    Id = "mancho",
                    Link = "goshogosho.com",
                    Text = "goshogoshogosho",
                    NotificationTypeId = 1,
                    SenderId = "sender",
                    ReceiverId = "receiver",
                    NotificationStatus = NotificationStatus.Непрочетено
                },
                new Notification
                {
                    Id = "evlogi",
                    Link = "goshogosho.com",
                    Text = "goshogoshogosho",
                    NotificationTypeId = 1,
                    SenderId = "sender",
                    ReceiverId = "receiver",
                    NotificationStatus = NotificationStatus.Прочетено
                },
            };

            await dataMock.Users.AddRangeAsync(sender);
            await dataMock.Users.AddRangeAsync(receiver);
            await dataMock.NotificationTypes.AddAsync(notificationType);
            await dataMock.Notifications.AddRangeAsync(notifications);

            await dataMock.SaveChangesAsync();

            var service = new NotificationService(dataMock);
            var result = await service.EditStatus(receiver.Id, NotificationStatus.Прочетено.ToString(), "mancho");

            Assert.True(result);
        }

        [Fact]
        public async Task EditStatusShouldEditTheStatusOfTheNotification()
        {
            var dataMock = DatabaseMock.Instance;
            var sender = new ApplicationUser
            {
                Id = "sender",
                UserName = "gosho@gosho.com"
            };

            var receiver = new ApplicationUser
            {
                Id = "receiver",
                UserName = "pesho@pesho.com"
            };

            var notificationType = new NotificationType()
            {
                Id = 1,
                Name = "message"
            };

            var notifications = new List<Notification>
            {
                new Notification
                {
                    Id = "gosho",
                    Link = "goshogosho.com",
                    Text = "goshogoshogosho",
                    NotificationTypeId = 1,
                    SenderId = "sender",
                    ReceiverId = "receiver",
                    NotificationStatus = NotificationStatus.Непрочетено
                },
                new Notification
                {
                    Id = "mancho",
                    Link = "goshogosho.com",
                    Text = "goshogoshogosho",
                    NotificationTypeId = 1,
                    SenderId = "sender",
                    ReceiverId = "receiver",
                    NotificationStatus = NotificationStatus.Непрочетено
                },
                new Notification
                {
                    Id = "evlogi",
                    Link = "goshogosho.com",
                    Text = "goshogoshogosho",
                    NotificationTypeId = 1,
                    SenderId = "sender",
                    ReceiverId = "receiver",
                    NotificationStatus = NotificationStatus.Прочетено
                },
            };

            await dataMock.Users.AddRangeAsync(sender);
            await dataMock.Users.AddRangeAsync(receiver);
            await dataMock.NotificationTypes.AddAsync(notificationType);
            await dataMock.Notifications.AddRangeAsync(notifications);

            await dataMock.SaveChangesAsync();

            var service = new NotificationService(dataMock);
            var result = await service.EditStatus(receiver.Id, NotificationStatus.Прочетено.ToString(), "mancho");

            var notification = await dataMock.Notifications.FirstOrDefaultAsync(x => x.Id == "mancho");

            Assert.Equal(NotificationStatus.Прочетено.ToString(), notification.NotificationStatus.ToString());
        }

        [Fact]
        public async Task EditStatusShouldReturnFalseIfNotificationIsNotFound()
        {
            var dataMock = DatabaseMock.Instance;
            var sender = new ApplicationUser
            {
                Id = "sender",
                UserName = "gosho@gosho.com"
            };

            var receiver = new ApplicationUser
            {
                Id = "receiver",
                UserName = "pesho@pesho.com"
            };

            var notificationType = new NotificationType()
            {
                Id = 1,
                Name = "message"
            };

            var notifications = new List<Notification>
            {
                new Notification
                {
                    Id = "gosho",
                    Link = "goshogosho.com",
                    Text = "goshogoshogosho",
                    NotificationTypeId = 1,
                    SenderId = "sender",
                    ReceiverId = "receiver",
                    NotificationStatus = NotificationStatus.Непрочетено
                },
                new Notification
                {
                    Id = "mancho",
                    Link = "goshogosho.com",
                    Text = "goshogoshogosho",
                    NotificationTypeId = 1,
                    SenderId = "sender",
                    ReceiverId = "receiver",
                    NotificationStatus = NotificationStatus.Непрочетено
                },
                new Notification
                {
                    Id = "evlogi",
                    Link = "goshogosho.com",
                    Text = "goshogoshogosho",
                    NotificationTypeId = 1,
                    SenderId = "sender",
                    ReceiverId = "receiver",
                    NotificationStatus = NotificationStatus.Прочетено
                },
            };

            await dataMock.Users.AddRangeAsync(sender);
            await dataMock.Users.AddRangeAsync(receiver);
            await dataMock.NotificationTypes.AddAsync(notificationType);
            await dataMock.Notifications.AddRangeAsync(notifications);

            await dataMock.SaveChangesAsync();

            var service = new NotificationService(dataMock);
            var result = await service.EditStatus(receiver.Id, NotificationStatus.Прочетено.ToString(), "tuti");

            Assert.False(result);
        }

        [Fact]
        public async Task DeleteNotificationShouldReturnTrueWhenSuccessful()
        {
            var dataMock = DatabaseMock.Instance;
            var sender = new ApplicationUser
            {
                Id = "sender",
                UserName = "gosho@gosho.com"
            };

            var receiver = new ApplicationUser
            {
                Id = "receiver",
                UserName = "pesho@pesho.com"
            };

            var notificationType = new NotificationType()
            {
                Id = 1,
                Name = "message"
            };

            var notifications = new List<Notification>
            {
                new Notification
                {
                    Id = "gosho",
                    Link = "goshogosho.com",
                    Text = "goshogoshogosho",
                    NotificationTypeId = 1,
                    SenderId = "sender",
                    ReceiverId = "receiver",
                    NotificationStatus = NotificationStatus.Непрочетено
                },
                new Notification
                {
                    Id = "mancho",
                    Link = "goshogosho.com",
                    Text = "goshogoshogosho",
                    NotificationTypeId = 1,
                    SenderId = "sender",
                    ReceiverId = "receiver",
                    NotificationStatus = NotificationStatus.Непрочетено
                },
                new Notification
                {
                    Id = "evlogi",
                    Link = "goshogosho.com",
                    Text = "goshogoshogosho",
                    NotificationTypeId = 1,
                    SenderId = "sender",
                    ReceiverId = "receiver",
                    NotificationStatus = NotificationStatus.Прочетено
                },
            };

            await dataMock.Users.AddRangeAsync(sender);
            await dataMock.Users.AddRangeAsync(receiver);
            await dataMock.NotificationTypes.AddAsync(notificationType);
            await dataMock.Notifications.AddRangeAsync(notifications);

            await dataMock.SaveChangesAsync();

            var service = new NotificationService(dataMock);
            var result = await service.DeleteNotification(receiver.Id, "evlogi");

            Assert.True(result);
        }

        [Fact]
        public async Task DeleteNotificationShouldSetIsDeletedToTrue()
        {
            var dataMock = DatabaseMock.Instance;
            var sender = new ApplicationUser
            {
                Id = "sender",
                UserName = "gosho@gosho.com"
            };

            var receiver = new ApplicationUser
            {
                Id = "receiver",
                UserName = "pesho@pesho.com"
            };

            var notificationType = new NotificationType()
            {
                Id = 1,
                Name = "message"
            };

            var notifications = new List<Notification>
            {
                new Notification
                {
                    Id = "gosho",
                    Link = "goshogosho.com",
                    Text = "goshogoshogosho",
                    NotificationTypeId = 1,
                    SenderId = "sender",
                    ReceiverId = "receiver",
                    NotificationStatus = NotificationStatus.Непрочетено
                },
                new Notification
                {
                    Id = "mancho",
                    Link = "goshogosho.com",
                    Text = "goshogoshogosho",
                    NotificationTypeId = 1,
                    SenderId = "sender",
                    ReceiverId = "receiver",
                    NotificationStatus = NotificationStatus.Непрочетено
                },
                new Notification
                {
                    Id = "evlogi",
                    Link = "goshogosho.com",
                    Text = "goshogoshogosho",
                    NotificationTypeId = 1,
                    SenderId = "sender",
                    ReceiverId = "receiver",
                    NotificationStatus = NotificationStatus.Прочетено
                },
            };

            await dataMock.Users.AddRangeAsync(sender);
            await dataMock.Users.AddRangeAsync(receiver);
            await dataMock.NotificationTypes.AddAsync(notificationType);
            await dataMock.Notifications.AddRangeAsync(notifications);

            await dataMock.SaveChangesAsync();

            var service = new NotificationService(dataMock);
            var result = await service.DeleteNotification(receiver.Id, "evlogi");

            var notification = await dataMock.Notifications.FirstOrDefaultAsync(x => x.Id == "evlogi");

            Assert.True(notification.IsDeleted);
        }

        [Fact]
        public async Task DeleteNotificationShouldReturnFalseIfNotificationIsNotFound()
        {
            var dataMock = DatabaseMock.Instance;
            var sender = new ApplicationUser
            {
                Id = "sender",
                UserName = "gosho@gosho.com"
            };

            var receiver = new ApplicationUser
            {
                Id = "receiver",
                UserName = "pesho@pesho.com"
            };

            var notificationType = new NotificationType()
            {
                Id = 1,
                Name = "message"
            };

            var notifications = new List<Notification>
            {
                new Notification
                {
                    Id = "gosho",
                    Link = "goshogosho.com",
                    Text = "goshogoshogosho",
                    NotificationTypeId = 1,
                    SenderId = "sender",
                    ReceiverId = "receiver",
                    NotificationStatus = NotificationStatus.Непрочетено
                },
                new Notification
                {
                    Id = "mancho",
                    Link = "goshogosho.com",
                    Text = "goshogoshogosho",
                    NotificationTypeId = 1,
                    SenderId = "sender",
                    ReceiverId = "receiver",
                    NotificationStatus = NotificationStatus.Непрочетено
                },
                new Notification
                {
                    Id = "evlogi",
                    Link = "goshogosho.com",
                    Text = "goshogoshogosho",
                    NotificationTypeId = 1,
                    SenderId = "sender",
                    ReceiverId = "receiver",
                    NotificationStatus = NotificationStatus.Прочетено
                },
            };

            await dataMock.Users.AddRangeAsync(sender);
            await dataMock.Users.AddRangeAsync(receiver);
            await dataMock.NotificationTypes.AddAsync(notificationType);
            await dataMock.Notifications.AddRangeAsync(notifications);

            await dataMock.SaveChangesAsync();

            var service = new NotificationService(dataMock);
            var result = await service.DeleteNotification(receiver.Id, "asdasd");

            Assert.False(result);
        }

        [Fact]
        public async Task DeleteNotificationShouldReturnFalseIfReceiverIdIsDifferent()
        {
            var dataMock = DatabaseMock.Instance;
            var sender = new ApplicationUser
            {
                Id = "sender",
                UserName = "gosho@gosho.com"
            };

            var receiver = new ApplicationUser
            {
                Id = "receiver",
                UserName = "pesho@pesho.com"
            };

            var notificationType = new NotificationType()
            {
                Id = 1,
                Name = "message"
            };

            var notifications = new List<Notification>
            {
                new Notification
                {
                    Id = "gosho",
                    Link = "goshogosho.com",
                    Text = "goshogoshogosho",
                    NotificationTypeId = 1,
                    SenderId = "sender",
                    ReceiverId = "receiver",
                    NotificationStatus = NotificationStatus.Непрочетено
                },
                new Notification
                {
                    Id = "mancho",
                    Link = "goshogosho.com",
                    Text = "goshogoshogosho",
                    NotificationTypeId = 1,
                    SenderId = "sender",
                    ReceiverId = "receiver",
                    NotificationStatus = NotificationStatus.Непрочетено
                },
                new Notification
                {
                    Id = "evlogi",
                    Link = "goshogosho.com",
                    Text = "goshogoshogosho",
                    NotificationTypeId = 1,
                    SenderId = "sender",
                    ReceiverId = "receiver",
                    NotificationStatus = NotificationStatus.Прочетено
                },
            };

            await dataMock.Users.AddRangeAsync(sender);
            await dataMock.Users.AddRangeAsync(receiver);
            await dataMock.NotificationTypes.AddAsync(notificationType);
            await dataMock.Notifications.AddRangeAsync(notifications);

            await dataMock.SaveChangesAsync();

            var service = new NotificationService(dataMock);
            var result = await service.DeleteNotification("pipi", "evlogi");

            Assert.False(result);
        }

        [Fact]
        public async Task DeleteNotificationShouldReturnFalseIfItIsAlreadyDeleted()
        {
            var dataMock = DatabaseMock.Instance;
            var sender = new ApplicationUser
            {
                Id = "sender",
                UserName = "gosho@gosho.com"
            };

            var receiver = new ApplicationUser
            {
                Id = "receiver",
                UserName = "pesho@pesho.com"
            };

            var notificationType = new NotificationType()
            {
                Id = 1,
                Name = "message"
            };

            var notifications = new List<Notification>
            {
                new Notification
                {
                    Id = "gosho",
                    Link = "goshogosho.com",
                    Text = "goshogoshogosho",
                    NotificationTypeId = 1,
                    SenderId = "sender",
                    ReceiverId = "receiver",
                    NotificationStatus = NotificationStatus.Непрочетено
                },
                new Notification
                {
                    Id = "mancho",
                    Link = "goshogosho.com",
                    Text = "goshogoshogosho",
                    NotificationTypeId = 1,
                    SenderId = "sender",
                    ReceiverId = "receiver",
                    NotificationStatus = NotificationStatus.Непрочетено
                },
                new Notification
                {
                    Id = "evlogi",
                    Link = "goshogosho.com",
                    Text = "goshogoshogosho",
                    NotificationTypeId = 1,
                    SenderId = "sender",
                    ReceiverId = "receiver",
                    NotificationStatus = NotificationStatus.Прочетено,
                    IsDeleted = true
                },
            };

            await dataMock.Users.AddRangeAsync(sender);
            await dataMock.Users.AddRangeAsync(receiver);
            await dataMock.NotificationTypes.AddAsync(notificationType);
            await dataMock.Notifications.AddRangeAsync(notifications);

            await dataMock.SaveChangesAsync();

            var service = new NotificationService(dataMock);
            var result = await service.DeleteNotification(receiver.Id, "evlogi");

            Assert.False(result);
        }
    }
}
