namespace Test.ServiceUnitTests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Data.Models;
    using Data.Models;
    using Data.Models.Enums;
    using Services.SlotService;

    using Test.Mocks;

    using Xunit;

    public class SlotServiceTests
    {
        [Fact]
        public async Task GenerateSlotsShouldReturnTrueWhenSlotsAreGeneratedSuccessfully()
        {
            var dataMock = DatabaseMock.Instance;
            var mapperMock = MapperMock.Instance;

            var service = new SlotService(dataMock, mapperMock);
            var startDate = new DateTime(2022, 01, 25, 10, 0, 0);
            var endDate = new DateTime(2022, 01, 25, 11, 0, 0);

            var result = await service.GenerateSlots(startDate, endDate, 10, 5);

            Assert.True(result);
        }

        [Fact]
        public async Task GenerateSlotsSuccessfullyAddsToTheDatabase()
        {
            var dataMock = DatabaseMock.Instance;
            var mapperMock = MapperMock.Instance;

            var service = new SlotService(dataMock, mapperMock);
            var startDate = new DateTime(2022, 01, 25, 10, 0, 0);
            var endDate = new DateTime(2022, 01, 25, 11, 0, 0);

            var result = await service.GenerateSlots(startDate, endDate, 10, 5);

            Assert.Equal(6, dataMock.AppointmentSlots.Count());
        }

        [Fact]
        public async Task GetGabrovoSlotsShouldReturnCorrectCount()
        {
            var dataMock = DatabaseMock.Instance;
            var mapperMock = MapperMock.Instance;

            var address = new Address()
            {
                Id = 5,
                StreetName = "Kaspichan",
                CityId = 1,
                City = new City()
                {
                    Name = "Габрово"
                }
            };

            await dataMock.Addresses.AddAsync(address);
            await dataMock.SaveChangesAsync();

            var service = new SlotService(dataMock, mapperMock);
            var startDate = new DateTime(2022, 01, 25, 10, 0, 0);
            var endDate = new DateTime(2022, 01, 25, 11, 0, 0);

            var generatedSlots = await service.GenerateSlots(startDate, endDate, 30, 5);

            var result = await service.GetGabrovoSlots();

            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task GetPlevenSlotsShouldReturnCorrectCount()
        {
            var dataMock = DatabaseMock.Instance;
            var mapperMock = MapperMock.Instance;

            var address = new Address()
            {
                Id = 5,
                StreetName = "Kaspichan",
                CityId = 1,
                City = new City()
                {
                    Name = "Плевен"
                }
            };

            await dataMock.Addresses.AddAsync(address);
            await dataMock.SaveChangesAsync();

            var service = new SlotService(dataMock, mapperMock);
            var startDate = new DateTime(2022, 01, 25, 10, 0, 0);
            var endDate = new DateTime(2022, 01, 25, 11, 0, 0);

            var generatedSlots = await service.GenerateSlots(startDate, endDate, 30, 5);

            var result = await service.GetPlevenSlots();

            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task GetFreeGabrovoSlotsShouldReturnCorrectCount()
        {
            var dataMock = DatabaseMock.Instance;
            var mapperMock = MapperMock.Instance;

            var address = new Address()
            {
                Id = 5,
                StreetName = "Kaspichan",
                CityId = 1,
                City = new City()
                {
                    Name = "Габрово"
                }
            };

            await dataMock.Addresses.AddAsync(address);
            await dataMock.SaveChangesAsync();

            var service = new SlotService(dataMock, mapperMock);
            var startDate = DateTime.UtcNow;
            var endDate = DateTime.UtcNow.AddHours(1);

            await service.GenerateSlots(startDate, endDate, 20, 5);

            var allSlots = dataMock.AppointmentSlots.ToList();
            var generatedSlot = dataMock.AppointmentSlots.First();
            generatedSlot.Status = SlotStatus.Зает.ToString();
            await dataMock.SaveChangesAsync();

            var result = await service.GetFreeGabrovoSlots();

            Assert.Equal(3, result.Count);
        }

        [Fact]
        public async Task GetFreePlevenSlotsShouldReturnCorrectCount()
        {
            var dataMock = DatabaseMock.Instance;
            var mapperMock = MapperMock.Instance;

            var address = new Address()
            {
                Id = 5,
                StreetName = "Kaspichan",
                CityId = 1,
                City = new City()
                {
                    Name = "Плевен"
                }
            };

            await dataMock.Addresses.AddAsync(address);
            await dataMock.SaveChangesAsync();

            var service = new SlotService(dataMock, mapperMock);
            var startDate = DateTime.UtcNow;
            var endDate = DateTime.UtcNow.AddHours(1);

            await service.GenerateSlots(startDate, endDate, 20, 5);

            var allSlots = dataMock.AppointmentSlots.ToList();
            var generatedSlot = dataMock.AppointmentSlots.First();
            generatedSlot.Status = SlotStatus.Зает.ToString();
            await dataMock.SaveChangesAsync();

            var result = await service.GetFreePlevenSlots();

            Assert.Equal(3, result.Count);
        }
        
        [Fact]
        public async Task DeleteSlotByIdShouldReturnDeletedSlotIdIfSuccessful()
        {
            var dataMock = DatabaseMock.Instance;
            var mapperMock = MapperMock.Instance;

            var address = new Address()
            {
                Id = 5,
                StreetName = "Kaspichan",
                CityId = 1,
                City = new City()
                {
                    Name = "Плевен"
                }
            };

            await dataMock.Addresses.AddAsync(address);
            await dataMock.SaveChangesAsync();

            var service = new SlotService(dataMock, mapperMock);
            var startDate = DateTime.UtcNow;
            var endDate = DateTime.UtcNow.AddHours(1);

            await service.GenerateSlots(startDate, endDate, 20, 5);

            var result = await service.DeleteSlotById(1);

            Assert.Equal(1, result);
        }
        
        [Fact]
        public async Task DeleteSlotByIdShouldReturnZeroIfSlotIsNull()
        {
            var dataMock = DatabaseMock.Instance;
            var mapperMock = MapperMock.Instance;

            var address = new Address()
            {
                Id = 5,
                StreetName = "Kaspichan",
                CityId = 1,
                City = new City()
                {
                    Name = "Плевен"
                }
            };

            await dataMock.Addresses.AddAsync(address);
            await dataMock.SaveChangesAsync();

            var service = new SlotService(dataMock, mapperMock);
            var startDate = DateTime.UtcNow;
            var endDate = DateTime.UtcNow.AddHours(1);

            await service.GenerateSlots(startDate, endDate, 20, 5);

            var result = await service.DeleteSlotById(15);

            Assert.Equal(0, result);
        }
        
        [Fact]
        public async Task GetTodaysTakenSlotsShouldReturnCorrectCount()
        {
            var dataMock = DatabaseMock.Instance;
            var mapperMock = MapperMock.Instance;

            var address = new Address()
            {
                Id = 5,
                StreetName = "Kaspichan",
                CityId = 1,
                City = new City()
                {
                    Name = "Плевен"
                }
            };

            await dataMock.Addresses.AddAsync(address);
            await dataMock.SaveChangesAsync();

            var service = new SlotService(dataMock, mapperMock);
            var startDate = DateTime.UtcNow;
            var endDate = DateTime.UtcNow.AddHours(1);

            await service.GenerateSlots(startDate, endDate, 20, 5);

            var allSlots = dataMock.AppointmentSlots.ToList();
            var generatedSlot = dataMock.AppointmentSlots.First();
            generatedSlot.Status = SlotStatus.Зает.ToString();
            await dataMock.SaveChangesAsync();

            var result = await service.GetTodaysTakenSlots();

            Assert.Equal(1, result.Count);
        }
        
        [Fact]
        public async Task EditSlotShouldWorkCorrectly()
        {
            var dataMock = DatabaseMock.Instance;
            var mapperMock = MapperMock.Instance;

            var address = new Address()
            {
                Id = 5,
                StreetName = "Kaspichan",
                CityId = 1,
                City = new City()
                {
                    Name = "Плевен"
                }
            };

            await dataMock.Addresses.AddAsync(address);
            await dataMock.SaveChangesAsync();

            var service = new SlotService(dataMock, mapperMock);
            var startDate = DateTime.UtcNow;
            var endDate = DateTime.UtcNow.AddHours(1);

            await service.GenerateSlots(startDate, endDate, 20, 5);

            var result = await service.EditSlot(1, "Зает", "asdasdasd");

            Assert.True(result);
        }
        
        [Fact]
        public async Task EditSlotShouldReturnFalseIfSlotIsNull()
        {
            var dataMock = DatabaseMock.Instance;
            var mapperMock = MapperMock.Instance;

            var address = new Address()
            {
                Id = 5,
                StreetName = "Kaspichan",
                CityId = 1,
                City = new City()
                {
                    Name = "Плевен"
                }
            };

            await dataMock.Addresses.AddAsync(address);
            await dataMock.SaveChangesAsync();

            var service = new SlotService(dataMock, mapperMock);
            var startDate = DateTime.UtcNow;
            var endDate = DateTime.UtcNow.AddHours(1);

            await service.GenerateSlots(startDate, endDate, 20, 5);

            var result = await service.EditSlot(15, "Зает", "asdasdasd");

            Assert.False(result);
        }
    }
}
