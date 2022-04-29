namespace Test.ServiceUnitTests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

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
    }
}
