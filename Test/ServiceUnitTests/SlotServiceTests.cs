namespace Test.ServiceUnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Services.SlotService;

    using Test.Mocks;

    using ViewModels.Slot;

    using Xunit;

    public class SlotServiceTests
    {
        [Fact]
        public async Task GenerateSlotsShouldReturnCorrectCount()
        {
            var dataMock = DatabaseMock.Instance;
            var mapperMock = MapperMock.Instance;

            var service = new SlotService(dataMock, mapperMock);
            var startDate = new DateTime(2022, 01, 25, 10, 0, 0);
            var endDate = new DateTime(2022, 01, 25, 11, 0, 0);

            var result = await service.GenerateSlots(startDate, endDate, 10);

            Assert.NotNull(result);
            Assert.Equal(6, result.Count);
        }

        [Fact]
        public async Task GenerateSlotsShouldReturnCorrectModel()
        {
            var dataMock = DatabaseMock.Instance;
            var mapperMock = MapperMock.Instance;

            var service = new SlotService(dataMock, mapperMock);
            var startDate = new DateTime(2022, 01, 25, 10, 0, 0);
            var endDate = new DateTime(2022, 01, 25, 11, 0, 0);

            var result = await service.GenerateSlots(startDate, endDate, 10);

            Assert.NotNull(result);
            Assert.IsAssignableFrom<ICollection<SlotViewModel>>(result);
        }

        [Fact]
        public async Task GenerateSlotsSuccessfullyAddsToTheDatabase()
        {
            var dataMock = DatabaseMock.Instance;
            var mapperMock = MapperMock.Instance;

            var service = new SlotService(dataMock, mapperMock);
            var startDate = new DateTime(2022, 01, 25, 10, 0, 0);
            var endDate = new DateTime(2022, 01, 25, 11, 0, 0);

            var result = await service.GenerateSlots(startDate, endDate, 10);

            Assert.Equal(6, dataMock.AppointmentSlots.Count());
        }
    }
}
