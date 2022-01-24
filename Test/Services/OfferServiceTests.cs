namespace Test.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Data.Models;

    using global::Services.OfferService;

    using Microsoft.EntityFrameworkCore;

    using Test.Mocks;

    using ViewModels.Administration.Offer;
    using ViewModels.Offer;

    using Xunit;

    public class OfferServiceTests
    {
        [Fact]
        public async Task GetAllShouldReturnCorrectCount()
        {
            var dataMock = DatabaseMock.Instance;
            var mapperMock = MapperMock.Instance;

            var offers = new List<OfferedService>
            {
                new OfferedService
                {
                    Id = 1,
                    Name = "Injection",
                    Price = 30
                },
                new OfferedService
                {
                    Id = 2,
                    Name = "Wound clean",
                    Price = 30
                },
                new OfferedService
                {
                    Id = 3,
                    Name = "Reciping",
                    Price = 30
                },
                new OfferedService
                {
                    Id = 4,
                    Name = "Examination",
                    Price = 30
                },
                new OfferedService
                {
                    Id = 5,
                    Name = "Vaccine",
                    Price = 30
                },
            };

            await dataMock.OfferedServices.AddRangeAsync(offers);
            await dataMock.SaveChangesAsync();

            var service = new OfferService(dataMock, mapperMock);
            var result = await service.GetAllAsync();

            Assert.NotNull(result);
            Assert.Equal(5, result.Count);
        }

        [Fact]
        public async Task GetAllAsyncShouldReturnCorrectModel()
        {
            var dataMock = DatabaseMock.Instance;
            var mapperMock = MapperMock.Instance;

            var offers = new List<OfferedService>
            {
                new OfferedService
                {
                    Id = 1,
                    Name = "Injection",
                    Price = 30
                },
                new OfferedService
                {
                    Id = 2,
                    Name = "Wound clean",
                    Price = 30
                },
                new OfferedService
                {
                    Id = 3,
                    Name = "Reciping",
                    Price = 30
                },
                new OfferedService
                {
                    Id = 4,
                    Name = "Examination",
                    Price = 30
                },
                new OfferedService
                {
                    Id = 5,
                    Name = "Vaccine",
                    Price = 30
                },
            };

            await dataMock.OfferedServices.AddRangeAsync(offers);
            await dataMock.SaveChangesAsync();

            var service = new OfferService(dataMock, mapperMock);
            var result = await service.GetAllAsync();

            Assert.IsAssignableFrom<ICollection<OfferViewModel>>(result);
        }

        [Fact]
        public async Task GetAllShouldReturnCorrectCountIfOneIsDeleted()
        {
            var dataMock = DatabaseMock.Instance;
            var mapperMock = MapperMock.Instance;

            var offers = new List<OfferedService>
            {
                new OfferedService
                {
                    Id = 1,
                    Name = "Injection",
                    Price = 30
                },
                new OfferedService
                {
                    Id = 2,
                    Name = "Wound clean",
                    Price = 30
                },
                new OfferedService
                {
                    Id = 3,
                    Name = "Reciping",
                    Price = 30
                },
                new OfferedService
                {
                    Id = 4,
                    Name = "Examination",
                    Price = 30
                },
                new OfferedService
                {
                    Id = 5,
                    Name = "Vaccine",
                    Price = 30,
                    IsDeleted = true
                },
            };

            await dataMock.OfferedServices.AddRangeAsync(offers);
            await dataMock.SaveChangesAsync();

            var service = new OfferService(dataMock, mapperMock);
            var result = await service.GetAllAsync();

            Assert.NotNull(result);
            Assert.Equal(4, result.Count);
        }

        [Fact]
        public async Task GetOnlineConsultationIdShouldReturnCorrectId()
        {
            var dataMock = DatabaseMock.Instance;
            var mapperMock = MapperMock.Instance;

            var offers = new List<OfferedService>
            {
                new OfferedService
                {
                    Id = 1,
                    Name = "Онлайн консултация",
                    Price = 30
                },
                new OfferedService
                {
                    Id = 2,
                    Name = "Wound clean",
                    Price = 30
                },
                new OfferedService
                {
                    Id = 3,
                    Name = "Reciping",
                    Price = 30
                },
                new OfferedService
                {
                    Id = 4,
                    Name = "Examination",
                    Price = 30
                },
                new OfferedService
                {
                    Id = 5,
                    Name = "Vaccine",
                    Price = 30
                },
            };

            await dataMock.OfferedServices.AddRangeAsync(offers);
            await dataMock.SaveChangesAsync();

            var service = new OfferService(dataMock, mapperMock);
            var result = await service.GetOnlineConsultationId();

            Assert.Equal(1, result);
        }

        [Fact]
        public async Task GetOnlineConsultationModelShouldReturnCorrectResult()
        {
            var dataMock = DatabaseMock.Instance;
            var mapperMock = MapperMock.Instance;

            var offers = new List<OfferedService>
            {
                new OfferedService
                {
                    Id = 1,
                    Name = "Онлайн консултация",
                    Price = 30
                },
                new OfferedService
                {
                    Id = 2,
                    Name = "Wound clean",
                    Price = 30
                },
                new OfferedService
                {
                    Id = 3,
                    Name = "Reciping",
                    Price = 30
                },
                new OfferedService
                {
                    Id = 4,
                    Name = "Examination",
                    Price = 30
                },
                new OfferedService
                {
                    Id = 5,
                    Name = "Vaccine",
                    Price = 30
                },
            };

            await dataMock.OfferedServices.AddRangeAsync(offers);
            await dataMock.SaveChangesAsync();

            var service = new OfferService(dataMock, mapperMock);
            var result = await service.GetOnlineConsultationModelAsync();

            Assert.Equal(1, result.Id);
            Assert.Equal(30, result.Price);
        }

        [Fact]
        public async Task DeleteOfferShouldReturnTrueIfSuccessful()
        {
            var dataMock = DatabaseMock.Instance;
            var mapperMock = MapperMock.Instance;

            var offers = new List<OfferedService>
            {
                new OfferedService
                {
                    Id = 1,
                    Name = "Онлайн консултация",
                    Price = 30
                },
                new OfferedService
                {
                    Id = 2,
                    Name = "Wound clean",
                    Price = 30
                },
                new OfferedService
                {
                    Id = 3,
                    Name = "Reciping",
                    Price = 30
                },
                new OfferedService
                {
                    Id = 4,
                    Name = "Examination",
                    Price = 30
                },
                new OfferedService
                {
                    Id = 5,
                    Name = "Vaccine",
                    Price = 30
                },
            };

            await dataMock.OfferedServices.AddRangeAsync(offers);
            await dataMock.SaveChangesAsync();

            var service = new OfferService(dataMock, mapperMock);
            var result = await service.DeleteOffer(1);

            Assert.True(result);
        }

        [Fact]
        public async Task DeleteOfferShouldSetIsDeletedToTrue()
        {
            var dataMock = DatabaseMock.Instance;
            var mapperMock = MapperMock.Instance;

            var offers = new List<OfferedService>
            {
                new OfferedService
                {
                    Id = 1,
                    Name = "Онлайн консултация",
                    Price = 30
                },
                new OfferedService
                {
                    Id = 2,
                    Name = "Wound clean",
                    Price = 30
                },
                new OfferedService
                {
                    Id = 3,
                    Name = "Reciping",
                    Price = 30
                },
                new OfferedService
                {
                    Id = 4,
                    Name = "Examination",
                    Price = 30
                },
                new OfferedService
                {
                    Id = 5,
                    Name = "Vaccine",
                    Price = 30
                },
            };

            await dataMock.OfferedServices.AddRangeAsync(offers);
            await dataMock.SaveChangesAsync();

            var service = new OfferService(dataMock, mapperMock);
            var result = await service.DeleteOffer(1);

            var model = await dataMock.OfferedServices.FirstOrDefaultAsync(x => x.Id == 1);
            Assert.True(model.IsDeleted);
        }

        [Fact]
        public async Task DeleteOfferShouldReturnFalseIfOfferIsNotFound()
        {
            var dataMock = DatabaseMock.Instance;
            var mapperMock = MapperMock.Instance;

            var offers = new List<OfferedService>
            {
                new OfferedService
                {
                    Id = 1,
                    Name = "Онлайн консултация",
                    Price = 30
                },
                new OfferedService
                {
                    Id = 2,
                    Name = "Wound clean",
                    Price = 30
                },
                new OfferedService
                {
                    Id = 3,
                    Name = "Reciping",
                    Price = 30
                },
                new OfferedService
                {
                    Id = 4,
                    Name = "Examination",
                    Price = 30
                },
                new OfferedService
                {
                    Id = 5,
                    Name = "Vaccine",
                    Price = 30
                },
            };

            await dataMock.OfferedServices.AddRangeAsync(offers);
            await dataMock.SaveChangesAsync();

            var service = new OfferService(dataMock, mapperMock);
            var result = await service.DeleteOffer(7);

            Assert.False(result);
        }

        [Fact]
        public async Task DeleteOfferShouldReturnFalseIfModelIsAlreadyDeleted()
        {
            var dataMock = DatabaseMock.Instance;
            var mapperMock = MapperMock.Instance;

            var offers = new List<OfferedService>
            {
                new OfferedService
                {
                    Id = 1,
                    Name = "Онлайн консултация",
                    Price = 30
                },
                new OfferedService
                {
                    Id = 2,
                    Name = "Wound clean",
                    Price = 30
                },
                new OfferedService
                {
                    Id = 3,
                    Name = "Reciping",
                    Price = 30
                },
                new OfferedService
                {
                    Id = 4,
                    Name = "Examination",
                    Price = 30
                },
                new OfferedService
                {
                    Id = 5,
                    Name = "Vaccine",
                    Price = 30,
                    IsDeleted = true
                },
            };

            await dataMock.OfferedServices.AddRangeAsync(offers);
            await dataMock.SaveChangesAsync();

            var service = new OfferService(dataMock, mapperMock);
            var result = await service.DeleteOffer(5);

            Assert.False(result);
        }

        [Fact]
        public async Task CreateOfferShouldAddModelToDatabase()
        {
            var dataMock = DatabaseMock.Instance;
            var mapperMock = MapperMock.Instance;

            var createModel = new CreateOfferFormModel
            {
                Name = "Consultation",
                Price = 30
            };

            var service = new OfferService(dataMock, mapperMock);
            await service.AddOffer(createModel);

            Assert.Equal(1, dataMock.OfferedServices.Count());
        }

        [Fact]
        public async Task GetOfferShouldReturnCorrectModel()
        {
            var dataMock = DatabaseMock.Instance;
            var mapperMock = MapperMock.Instance;

            var offers = new List<OfferedService>
            {
                new OfferedService
                {
                    Id = 1,
                    Name = "Онлайн консултация",
                    Price = 30
                },
                new OfferedService
                {
                    Id = 2,
                    Name = "Wound clean",
                    Price = 30
                },
                new OfferedService
                {
                    Id = 3,
                    Name = "Reciping",
                    Price = 30
                },
                new OfferedService
                {
                    Id = 4,
                    Name = "Examination",
                    Price = 30
                },
                new OfferedService
                {
                    Id = 5,
                    Name = "Vaccine",
                    Price = 30
                },
            };

            await dataMock.OfferedServices.AddRangeAsync(offers);
            await dataMock.SaveChangesAsync();

            var service = new OfferService(dataMock, mapperMock);
            var result = await service.GetOffer(1);

            Assert.NotNull(result);
            Assert.IsType<EditOfferFormModel>(result);
        }

        [Fact]
        public async Task GetOfferShouldReturnNullIfOfferIsNotFound()
        {
            var dataMock = DatabaseMock.Instance;
            var mapperMock = MapperMock.Instance;

            var offers = new List<OfferedService>
            {
                new OfferedService
                {
                    Id = 1,
                    Name = "Онлайн консултация",
                    Price = 30
                },
                new OfferedService
                {
                    Id = 2,
                    Name = "Wound clean",
                    Price = 30
                },
                new OfferedService
                {
                    Id = 3,
                    Name = "Reciping",
                    Price = 30
                },
                new OfferedService
                {
                    Id = 4,
                    Name = "Examination",
                    Price = 30
                },
                new OfferedService
                {
                    Id = 5,
                    Name = "Vaccine",
                    Price = 30
                },
            };

            await dataMock.OfferedServices.AddRangeAsync(offers);
            await dataMock.SaveChangesAsync();

            var service = new OfferService(dataMock, mapperMock);
            var result = await service.GetOffer(9);

            Assert.Null(result);
        }

        [Fact]
        public async Task GetOfferShouldReturnNullIfOfferIsDeleted()
        {
            var dataMock = DatabaseMock.Instance;
            var mapperMock = MapperMock.Instance;

            var offers = new List<OfferedService>
            {
                new OfferedService
                {
                    Id = 1,
                    Name = "Онлайн консултация",
                    Price = 30
                },
                new OfferedService
                {
                    Id = 2,
                    Name = "Wound clean",
                    Price = 30
                },
                new OfferedService
                {
                    Id = 3,
                    Name = "Reciping",
                    Price = 30
                },
                new OfferedService
                {
                    Id = 4,
                    Name = "Examination",
                    Price = 30
                },
                new OfferedService
                {
                    Id = 5,
                    Name = "Vaccine",
                    Price = 30,
                    IsDeleted = true
                },
            };

            await dataMock.OfferedServices.AddRangeAsync(offers);
            await dataMock.SaveChangesAsync();

            var service = new OfferService(dataMock, mapperMock);
            var result = await service.GetOffer(5);

            Assert.Null(result);
        }

        [Fact]
        public async Task EditOfferShouldReturnTrueIfSuccesful()
        {
            var dataMock = DatabaseMock.Instance;
            var mapperMock = MapperMock.Instance;

            var offers = new List<OfferedService>
            {
                new OfferedService
                {
                    Id = 1,
                    Name = "Онлайн консултация",
                    Price = 30
                },
                new OfferedService
                {
                    Id = 2,
                    Name = "Wound clean",
                    Price = 30
                },
                new OfferedService
                {
                    Id = 3,
                    Name = "Reciping",
                    Price = 30
                },
                new OfferedService
                {
                    Id = 4,
                    Name = "Examination",
                    Price = 30
                },
                new OfferedService
                {
                    Id = 5,
                    Name = "Vaccine",
                    Price = 30
                },
            };

            await dataMock.OfferedServices.AddRangeAsync(offers);
            await dataMock.SaveChangesAsync();
            var model = new EditOfferFormModel()
            {
                Id = 1,
                Name = "Consult",
                Price = 10
            };

            var service = new OfferService(dataMock, mapperMock);
            var result = await service.EditOffer(model);

            Assert.True(result);
        }

        [Fact]
        public async Task EditOfferShouldEditProperties()
        {
            var dataMock = DatabaseMock.Instance;
            var mapperMock = MapperMock.Instance;

            var offers = new List<OfferedService>
            {
                new OfferedService
                {
                    Id = 1,
                    Name = "Онлайн консултация",
                    Price = 30
                },
                new OfferedService
                {
                    Id = 2,
                    Name = "Wound clean",
                    Price = 30
                },
                new OfferedService
                {
                    Id = 3,
                    Name = "Reciping",
                    Price = 30
                },
                new OfferedService
                {
                    Id = 4,
                    Name = "Examination",
                    Price = 30
                },
                new OfferedService
                {
                    Id = 5,
                    Name = "Vaccine",
                    Price = 30
                },
            };

            await dataMock.OfferedServices.AddRangeAsync(offers);
            await dataMock.SaveChangesAsync();

            var model = new EditOfferFormModel()
            {
                Id = 1,
                Name = "Consult",
                Price = 10
            };

            var service = new OfferService(dataMock, mapperMock);
            var result = await service.EditOffer(model);

            var edited = await dataMock.OfferedServices.FirstOrDefaultAsync(x => x.Id == 1);

            Assert.Equal("Consult", edited.Name);
            Assert.Equal(10, edited.Price);
        }

        [Fact]
        public async Task EditOfferShouldReturnFalseIfModelIsNotFound()
        {
            var dataMock = DatabaseMock.Instance;
            var mapperMock = MapperMock.Instance;

            var offers = new List<OfferedService>
            {
                new OfferedService
                {
                    Id = 1,
                    Name = "Онлайн консултация",
                    Price = 30
                },
                new OfferedService
                {
                    Id = 2,
                    Name = "Wound clean",
                    Price = 30
                },
                new OfferedService
                {
                    Id = 3,
                    Name = "Reciping",
                    Price = 30
                },
                new OfferedService
                {
                    Id = 4,
                    Name = "Examination",
                    Price = 30
                },
                new OfferedService
                {
                    Id = 5,
                    Name = "Vaccine",
                    Price = 30
                },
            };

            await dataMock.OfferedServices.AddRangeAsync(offers);
            await dataMock.SaveChangesAsync();

            var model = new EditOfferFormModel()
            {
                Id = 15,
                Name = "Consult",
                Price = 10
            };

            var service = new OfferService(dataMock, mapperMock);
            var result = await service.EditOffer(model);

            Assert.False(result);
        }

        [Fact]
        public async Task EditOfferShouldReturnFalseIfOfferIsDeleted()
        {
            var dataMock = DatabaseMock.Instance;
            var mapperMock = MapperMock.Instance;

            var offers = new List<OfferedService>
            {
                new OfferedService
                {
                    Id = 1,
                    Name = "Онлайн консултация",
                    Price = 30
                },
                new OfferedService
                {
                    Id = 2,
                    Name = "Wound clean",
                    Price = 30
                },
                new OfferedService
                {
                    Id = 3,
                    Name = "Reciping",
                    Price = 30
                },
                new OfferedService
                {
                    Id = 4,
                    Name = "Examination",
                    Price = 30
                },
                new OfferedService
                {
                    Id = 5,
                    Name = "Vaccine",
                    Price = 30,
                    IsDeleted = true
                },
            };

            await dataMock.OfferedServices.AddRangeAsync(offers);
            await dataMock.SaveChangesAsync();

            var model = new EditOfferFormModel()
            {
                Id = 5,
                Name = "Consult",
                Price = 10
            };

            var service = new OfferService(dataMock, mapperMock);
            var result = await service.EditOffer(model);

            Assert.False(result);
        }
    }
}