namespace Test.ServiceUnitTests;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mocks;
using Neonatology.Data.Models;
using Neonatology.Services.FeedbackService;
using Neonatology.ViewModels.Feedback;
using Xunit;

public class FeedbackServiceTests
{
    [Fact]
    public async Task CreateFeedbackShouldPopulateDatabaseCorrectly()
    {
        var dataMock = DatabaseMock.Instance;
        var mapperMock = MapperMock.Instance;

        var service = new FeedbackService(dataMock, mapperMock);

        var model = new CreateFeedbackModel()
        {
            Email = "gosho@gosho.bg",
            Comment = "goshogoshogoshogosho",
            FirstName = "Gosho",
            LastName = "Gosho",
            Type = "GoshoGosho"
        };

        await service.CreateFeedback(model);

        Assert.Equal(1, dataMock.Feedbacks.Count());
        var feedback = await dataMock.Feedbacks.FirstOrDefaultAsync(x => x.Email == "gosho@gosho.bg");

        Assert.Equal("Gosho", feedback.FirstName);
    }

    [Fact]
    public async Task GetUserFeedbacksShouldReturnCorrectCount()
    {
        var dataMock = DatabaseMock.Instance;
        var mapperMock = MapperMock.Instance;

        var feedbacks = new List<Feedback>
        {
            new Feedback
            {
                Id = 1,
                FirstName = "Gosho",
                LastName = "Goshev",
                Email = "gosho@gosho.bg",
                Comment = "asdasdasdasd",
                IsSolved = false,
                Type = "Gosho"
            },
            new Feedback
            {
                Id = 2,
                FirstName = "Gosho",
                LastName = "Goshev",
                Email = "gosho@gosho.bg",
                Comment = "asdasdasdasd",
                IsSolved = false,
                Type = "Gosho"
            },
            new Feedback
            {
                Id = 3,
                FirstName = "Gosho",
                LastName = "Goshev",
                Email = "gosho@gosho.bg",
                Comment = "asdasdasdasd",
                IsSolved = false,
                Type = "Gosho"
            },
            new Feedback
            {
                Id = 4,
                FirstName = "Gosho",
                LastName = "Goshev",
                Email = "gosho@gosho.bg",
                Comment = "asdasdasdasd",
                IsSolved = false,
                Type = "Gosho"
            },
            new Feedback
            {
                Id = 5,
                FirstName = "Gosho",
                LastName = "Goshev",
                Email = "evlogi@gosho.bg",
                Comment = "asdasdasdasd",
                IsSolved = false,
                Type = "Gosho"
            },
        };

        await dataMock.Feedbacks.AddRangeAsync(feedbacks);
        await dataMock.SaveChangesAsync();

        var service = new FeedbackService(dataMock, mapperMock);
        var result = await service.GetUserFeedbacks("gosho@gosho.bg");

        Assert.NotNull(result);
        Assert.Equal(4, result.Count);
    }

    [Fact]
    public async Task GetUserFeedbacksShouldReturnCorrectModel()
    {
        var dataMock = DatabaseMock.Instance;
        var mapperMock = MapperMock.Instance;

        var feedbacks = new List<Feedback>
        {
            new Feedback
            {
                Id = 1,
                FirstName = "Gosho",
                LastName = "Goshev",
                Email = "gosho@gosho.bg",
                Comment = "asdasdasdasd",
                IsSolved = false,
                Type = "Gosho"
            },
            new Feedback
            {
                Id = 2,
                FirstName = "Gosho",
                LastName = "Goshev",
                Email = "gosho@gosho.bg",
                Comment = "asdasdasdasd",
                IsSolved = false,
                Type = "Gosho"
            },
            new Feedback
            {
                Id = 3,
                FirstName = "Gosho",
                LastName = "Goshev",
                Email = "gosho@gosho.bg",
                Comment = "asdasdasdasd",
                IsSolved = false,
                Type = "Gosho"
            },
            new Feedback
            {
                Id = 4,
                FirstName = "Gosho",
                LastName = "Goshev",
                Email = "gosho@gosho.bg",
                Comment = "asdasdasdasd",
                IsSolved = false,
                Type = "Gosho"
            },
            new Feedback
            {
                Id = 5,
                FirstName = "Gosho",
                LastName = "Goshev",
                Email = "evlogi@gosho.bg",
                Comment = "asdasdasdasd",
                IsSolved = false,
                Type = "Gosho"
            },
        };

        await dataMock.Feedbacks.AddRangeAsync(feedbacks);
        await dataMock.SaveChangesAsync();

        var service = new FeedbackService(dataMock, mapperMock);
        var result = await service.GetUserFeedbacks("gosho@gosho.bg");

        Assert.NotNull(result);
        Assert.IsAssignableFrom<ICollection<FeedbackViewModel>>(result);
    }

    [Fact]
    public async Task GetUserFeedbacksShouldReturnOnlyModelsThatAreNotDeleted()
    {
        var dataMock = DatabaseMock.Instance;
        var mapperMock = MapperMock.Instance;

        var feedbacks = new List<Feedback>
        {
            new Feedback
            {
                Id = 1,
                FirstName = "Gosho",
                LastName = "Goshev",
                Email = "gosho@gosho.bg",
                Comment = "asdasdasdasd",
                IsSolved = false,
                Type = "Gosho"
            },
            new Feedback
            {
                Id = 2,
                FirstName = "Gosho",
                LastName = "Goshev",
                Email = "gosho@gosho.bg",
                Comment = "asdasdasdasd",
                IsSolved = false,
                Type = "Gosho"
            },
            new Feedback
            {
                Id = 3,
                FirstName = "Gosho",
                LastName = "Goshev",
                Email = "gosho@gosho.bg",
                Comment = "asdasdasdasd",
                IsSolved = false,
                Type = "Gosho",
                IsDeleted = true
            },
            new Feedback
            {
                Id = 4,
                FirstName = "Gosho",
                LastName = "Goshev",
                Email = "gosho@gosho.bg",
                Comment = "asdasdasdasd",
                IsSolved = false,
                Type = "Gosho"
            },
            new Feedback
            {
                Id = 5,
                FirstName = "Gosho",
                LastName = "Goshev",
                Email = "evlogi@gosho.bg",
                Comment = "asdasdasdasd",
                IsSolved = false,
                Type = "Gosho"
            },
        };

        await dataMock.Feedbacks.AddRangeAsync(feedbacks);
        await dataMock.SaveChangesAsync();

        var service = new FeedbackService(dataMock, mapperMock);
        var result = await service.GetUserFeedbacks("gosho@gosho.bg");

        Assert.NotNull(result);
        Assert.Equal(3, result.Count);
    }

    [Fact]
    public async Task GetAllShouldReturnCorrectCount()
    {
        var dataMock = DatabaseMock.Instance;
        var mapperMock = MapperMock.Instance;

        var feedbacks = new List<Feedback>
        {
            new Feedback
            {
                Id = 1,
                FirstName = "Gosho",
                LastName = "Goshev",
                Email = "gosho@gosho.bg",
                Comment = "asdasdasdasd",
                IsSolved = false,
                Type = "Gosho"
            },
            new Feedback
            {
                Id = 2,
                FirstName = "Gosho",
                LastName = "Goshev",
                Email = "gosho@gosho.bg",
                Comment = "asdasdasdasd",
                IsSolved = false,
                Type = "Gosho"
            },
            new Feedback
            {
                Id = 3,
                FirstName = "Gosho",
                LastName = "Goshev",
                Email = "gosho@gosho.bg",
                Comment = "asdasdasdasd",
                IsSolved = false,
                Type = "Gosho"
            },
            new Feedback
            {
                Id = 4,
                FirstName = "Gosho",
                LastName = "Goshev",
                Email = "gosho@gosho.bg",
                Comment = "asdasdasdasd",
                IsSolved = false,
                Type = "Gosho"
            },
            new Feedback
            {
                Id = 5,
                FirstName = "Gosho",
                LastName = "Goshev",
                Email = "evlogi@gosho.bg",
                Comment = "asdasdasdasd",
                IsSolved = false,
                Type = "Gosho"
            },
        };

        await dataMock.Feedbacks.AddRangeAsync(feedbacks);
        await dataMock.SaveChangesAsync();

        var service = new FeedbackService(dataMock, mapperMock);
        var result = await service.GetAll();

        Assert.NotNull(result);
        Assert.Equal(5, result.Count);
    }

    [Fact]
    public async Task GetAllShouldReturnOnlyModelsThatAreNotDeleted()
    {
        var dataMock = DatabaseMock.Instance;
        var mapperMock = MapperMock.Instance;

        var feedbacks = new List<Feedback>
        {
            new Feedback
            {
                Id = 1,
                FirstName = "Gosho",
                LastName = "Goshev",
                Email = "gosho@gosho.bg",
                Comment = "asdasdasdasd",
                IsSolved = false,
                Type = "Gosho"
            },
            new Feedback
            {
                Id = 2,
                FirstName = "Gosho",
                LastName = "Goshev",
                Email = "gosho@gosho.bg",
                Comment = "asdasdasdasd",
                IsSolved = false,
                Type = "Gosho"
            },
            new Feedback
            {
                Id = 3,
                FirstName = "Gosho",
                LastName = "Goshev",
                Email = "gosho@gosho.bg",
                Comment = "asdasdasdasd",
                IsSolved = false,
                Type = "Gosho"
            },
            new Feedback
            {
                Id = 4,
                FirstName = "Gosho",
                LastName = "Goshev",
                Email = "gosho@gosho.bg",
                Comment = "asdasdasdasd",
                IsSolved = false,
                Type = "Gosho"
            },
            new Feedback
            {
                Id = 5,
                FirstName = "Gosho",
                LastName = "Goshev",
                Email = "evlogi@gosho.bg",
                Comment = "asdasdasdasd",
                IsSolved = false,
                Type = "Gosho",
                IsDeleted = true
            },
        };

        await dataMock.Feedbacks.AddRangeAsync(feedbacks);
        await dataMock.SaveChangesAsync();

        var service = new FeedbackService(dataMock, mapperMock);
        var result = await service.GetAll();

        Assert.NotNull(result);
        Assert.Equal(4, result.Count);
    }

    [Fact]
    public async Task GetAllFeedbacksShouldReturnCorrectModel()
    {
        var dataMock = DatabaseMock.Instance;
        var mapperMock = MapperMock.Instance;

        var feedbacks = new List<Feedback>
        {
            new Feedback
            {
                Id = 1,
                FirstName = "Gosho",
                LastName = "Goshev",
                Email = "gosho@gosho.bg",
                Comment = "asdasdasdasd",
                IsSolved = false,
                Type = "Gosho"
            },
            new Feedback
            {
                Id = 2,
                FirstName = "Gosho",
                LastName = "Goshev",
                Email = "gosho@gosho.bg",
                Comment = "asdasdasdasd",
                IsSolved = false,
                Type = "Gosho"
            },
            new Feedback
            {
                Id = 3,
                FirstName = "Gosho",
                LastName = "Goshev",
                Email = "gosho@gosho.bg",
                Comment = "asdasdasdasd",
                IsSolved = false,
                Type = "Gosho"
            },
            new Feedback
            {
                Id = 4,
                FirstName = "Gosho",
                LastName = "Goshev",
                Email = "gosho@gosho.bg",
                Comment = "asdasdasdasd",
                IsSolved = false,
                Type = "Gosho"
            },
            new Feedback
            {
                Id = 5,
                FirstName = "Gosho",
                LastName = "Goshev",
                Email = "evlogi@gosho.bg",
                Comment = "asdasdasdasd",
                IsSolved = false,
                Type = "Gosho"
            },
        };

        await dataMock.Feedbacks.AddRangeAsync(feedbacks);
        await dataMock.SaveChangesAsync();

        var service = new FeedbackService(dataMock, mapperMock);
        var result = await service.GetAll();

        Assert.NotNull(result);
        Assert.IsAssignableFrom<ICollection<FeedbackViewModel>>(result);
    }

    [Fact]
    public async Task DeleteShouldReturnTrueIfSuccessful()
    {
        var dataMock = DatabaseMock.Instance;
        var mapperMock = MapperMock.Instance;

        var feedbacks = new List<Feedback>
        {
            new Feedback
            {
                Id = 1,
                FirstName = "Gosho",
                LastName = "Goshev",
                Email = "gosho@gosho.bg",
                Comment = "asdasdasdasd",
                IsSolved = false,
                Type = "Gosho"
            },
            new Feedback
            {
                Id = 2,
                FirstName = "Gosho",
                LastName = "Goshev",
                Email = "gosho@gosho.bg",
                Comment = "asdasdasdasd",
                IsSolved = false,
                Type = "Gosho"
            },
            new Feedback
            {
                Id = 3,
                FirstName = "Gosho",
                LastName = "Goshev",
                Email = "gosho@gosho.bg",
                Comment = "asdasdasdasd",
                IsSolved = false,
                Type = "Gosho"
            },
            new Feedback
            {
                Id = 4,
                FirstName = "Gosho",
                LastName = "Goshev",
                Email = "gosho@gosho.bg",
                Comment = "asdasdasdasd",
                IsSolved = false,
                Type = "Gosho"
            },
            new Feedback
            {
                Id = 5,
                FirstName = "Gosho",
                LastName = "Goshev",
                Email = "evlogi@gosho.bg",
                Comment = "asdasdasdasd",
                IsSolved = false,
                Type = "Gosho"
            },
        };

        await dataMock.Feedbacks.AddRangeAsync(feedbacks);
        await dataMock.SaveChangesAsync();

        var service = new FeedbackService(dataMock, mapperMock);
        var result = await service.Delete(5);

        Assert.True(result.Succeeded);
    }

    [Fact]
    public async Task DeleteShouldReturnCorrectCountWhenFeedbacksAreFilteredWithIsDeletedSetToTrue()
    {
        var dataMock = DatabaseMock.Instance;
        var mapperMock = MapperMock.Instance;

        var feedbacks = new List<Feedback>
        {
            new Feedback
            {
                Id = 1,
                FirstName = "Gosho",
                LastName = "Goshev",
                Email = "gosho@gosho.bg",
                Comment = "asdasdasdasd",
                IsSolved = false,
                Type = "Gosho"
            },
            new Feedback
            {
                Id = 2,
                FirstName = "Gosho",
                LastName = "Goshev",
                Email = "gosho@gosho.bg",
                Comment = "asdasdasdasd",
                IsSolved = false,
                Type = "Gosho"
            },
            new Feedback
            {
                Id = 3,
                FirstName = "Gosho",
                LastName = "Goshev",
                Email = "gosho@gosho.bg",
                Comment = "asdasdasdasd",
                IsSolved = false,
                Type = "Gosho"
            },
            new Feedback
            {
                Id = 4,
                FirstName = "Gosho",
                LastName = "Goshev",
                Email = "gosho@gosho.bg",
                Comment = "asdasdasdasd",
                IsSolved = false,
                Type = "Gosho"
            },
            new Feedback
            {
                Id = 5,
                FirstName = "Gosho",
                LastName = "Goshev",
                Email = "evlogi@gosho.bg",
                Comment = "asdasdasdasd",
                IsSolved = false,
                Type = "Gosho"
            },
        };

        await dataMock.Feedbacks.AddRangeAsync(feedbacks);
        await dataMock.SaveChangesAsync();

        var service = new FeedbackService(dataMock, mapperMock);
        var result = await service.Delete(5);

        var list = await dataMock.Feedbacks
            .Where(x => x.IsDeleted == false)
            .ToListAsync();

        Assert.Equal(4, list.Count);
    }

    [Fact]
    public async Task DeleteFeedbackShouldReturnFalseIfFeedbackDoesNotExist()
    {
        var dataMock = DatabaseMock.Instance;
        var mapperMock = MapperMock.Instance;

        var feedbacks = new List<Feedback>
        {
            new Feedback
            {
                Id = 1,
                FirstName = "Gosho",
                LastName = "Goshev",
                Email = "gosho@gosho.bg",
                Comment = "asdasdasdasd",
                IsSolved = false,
                Type = "Gosho"
            },
            new Feedback
            {
                Id = 2,
                FirstName = "Gosho",
                LastName = "Goshev",
                Email = "gosho@gosho.bg",
                Comment = "asdasdasdasd",
                IsSolved = false,
                Type = "Gosho"
            },
            new Feedback
            {
                Id = 3,
                FirstName = "Gosho",
                LastName = "Goshev",
                Email = "gosho@gosho.bg",
                Comment = "asdasdasdasd",
                IsSolved = false,
                Type = "Gosho"
            },
            new Feedback
            {
                Id = 4,
                FirstName = "Gosho",
                LastName = "Goshev",
                Email = "gosho@gosho.bg",
                Comment = "asdasdasdasd",
                IsSolved = false,
                Type = "Gosho"
            },
            new Feedback
            {
                Id = 5,
                FirstName = "Gosho",
                LastName = "Goshev",
                Email = "evlogi@gosho.bg",
                Comment = "asdasdasdasd",
                IsSolved = false,
                Type = "Gosho"
            },
        };

        await dataMock.Feedbacks.AddRangeAsync(feedbacks);
        await dataMock.SaveChangesAsync();

        var service = new FeedbackService(dataMock, mapperMock);
        var result = await service.Delete(6);

        Assert.True(result.Failed);
    }

    [Fact]
    public async Task DeleteShouldReturnFalseIfModelIsAlreadyDeleted()
    {
        var dataMock = DatabaseMock.Instance;
        var mapperMock = MapperMock.Instance;

        var feedbacks = new List<Feedback>
        {
            new Feedback
            {
                Id = 1,
                FirstName = "Gosho",
                LastName = "Goshev",
                Email = "gosho@gosho.bg",
                Comment = "asdasdasdasd",
                IsSolved = false,
                Type = "Gosho"
            },
            new Feedback
            {
                Id = 2,
                FirstName = "Gosho",
                LastName = "Goshev",
                Email = "gosho@gosho.bg",
                Comment = "asdasdasdasd",
                IsSolved = false,
                Type = "Gosho"
            },
            new Feedback
            {
                Id = 3,
                FirstName = "Gosho",
                LastName = "Goshev",
                Email = "gosho@gosho.bg",
                Comment = "asdasdasdasd",
                IsSolved = false,
                Type = "Gosho"
            },
            new Feedback
            {
                Id = 4,
                FirstName = "Gosho",
                LastName = "Goshev",
                Email = "gosho@gosho.bg",
                Comment = "asdasdasdasd",
                IsSolved = false,
                Type = "Gosho"
            },
            new Feedback
            {
                Id = 5,
                FirstName = "Gosho",
                LastName = "Goshev",
                Email = "evlogi@gosho.bg",
                Comment = "asdasdasdasd",
                IsSolved = false,
                Type = "Gosho",
                IsDeleted = true
            },
        };

        await dataMock.Feedbacks.AddRangeAsync(feedbacks);
        await dataMock.SaveChangesAsync();

        var service = new FeedbackService(dataMock, mapperMock);
        var result = await service.Delete(5);

        Assert.True(result.Failed);
    }

    [Fact]
    public async Task GetByIdShouldReturnCorrectEntity()
    {
        var dataMock = DatabaseMock.Instance;
        var mapperMock = MapperMock.Instance;

        var feedbacks = new List<Feedback>
        {
            new Feedback
            {
                Id = 1,
                FirstName = "Gosho",
                LastName = "Goshev",
                Email = "gosho@gosho.bg",
                Comment = "asdasdasdasd",
                IsSolved = false,
                Type = "Gosho"
            },
            new Feedback
            {
                Id = 2,
                FirstName = "Gosho",
                LastName = "Goshev",
                Email = "gosho@gosho.bg",
                Comment = "asdasdasdasd",
                IsSolved = false,
                Type = "Gosho"
            },
            new Feedback
            {
                Id = 3,
                FirstName = "Gosho",
                LastName = "Goshev",
                Email = "gosho@gosho.bg",
                Comment = "asdasdasdasd",
                IsSolved = false,
                Type = "Gosho"
            },
            new Feedback
            {
                Id = 4,
                FirstName = "Gosho",
                LastName = "Goshev",
                Email = "gosho@gosho.bg",
                Comment = "asdasdasdasd",
                IsSolved = false,
                Type = "Gosho"
            },
            new Feedback
            {
                Id = 5,
                FirstName = "Gosho",
                LastName = "Goshev",
                Email = "evlogi@gosho.bg",
                Comment = "asdasdasdasd",
                IsSolved = false,
                Type = "Gosho"
            },
        };

        await dataMock.Feedbacks.AddRangeAsync(feedbacks);
        await dataMock.SaveChangesAsync();

        var service = new FeedbackService(dataMock, mapperMock);
        var result = await service.GetById(5);

        Assert.NotNull(result);
        Assert.Equal("Gosho", result.FirstName);
    }

    [Fact]
    public async Task GetByIdShouldReturnCorrectModelType()
    {
        var dataMock = DatabaseMock.Instance;
        var mapperMock = MapperMock.Instance;

        var feedbacks = new List<Feedback>
        {
            new Feedback
            {
                Id = 1,
                FirstName = "Gosho",
                LastName = "Goshev",
                Email = "gosho@gosho.bg",
                Comment = "asdasdasdasd",
                IsSolved = false,
                Type = "Gosho"
            },
            new Feedback
            {
                Id = 2,
                FirstName = "Gosho",
                LastName = "Goshev",
                Email = "gosho@gosho.bg",
                Comment = "asdasdasdasd",
                IsSolved = false,
                Type = "Gosho"
            },
            new Feedback
            {
                Id = 3,
                FirstName = "Gosho",
                LastName = "Goshev",
                Email = "gosho@gosho.bg",
                Comment = "asdasdasdasd",
                IsSolved = false,
                Type = "Gosho"
            },
            new Feedback
            {
                Id = 4,
                FirstName = "Gosho",
                LastName = "Goshev",
                Email = "gosho@gosho.bg",
                Comment = "asdasdasdasd",
                IsSolved = false,
                Type = "Gosho"
            },
            new Feedback
            {
                Id = 5,
                FirstName = "Gosho",
                LastName = "Goshev",
                Email = "evlogi@gosho.bg",
                Comment = "asdasdasdasd",
                IsSolved = false,
                Type = "Gosho"
            },
        };

        await dataMock.Feedbacks.AddRangeAsync(feedbacks);
        await dataMock.SaveChangesAsync();

        var service = new FeedbackService(dataMock, mapperMock);
        var result = await service.GetById(5);

        Assert.NotNull(result);
        Assert.IsType<FeedbackViewModel>(result);
    }

    [Fact]
    public async Task GetByIdShouldReturnNullIfIdIsNotFound()
    {
        var dataMock = DatabaseMock.Instance;
        var mapperMock = MapperMock.Instance;

        var feedbacks = new List<Feedback>
        {
            new Feedback
            {
                Id = 1,
                FirstName = "Gosho",
                LastName = "Goshev",
                Email = "gosho@gosho.bg",
                Comment = "asdasdasdasd",
                IsSolved = false,
                Type = "Gosho"
            },
            new Feedback
            {
                Id = 2,
                FirstName = "Gosho",
                LastName = "Goshev",
                Email = "gosho@gosho.bg",
                Comment = "asdasdasdasd",
                IsSolved = false,
                Type = "Gosho"
            },
            new Feedback
            {
                Id = 3,
                FirstName = "Gosho",
                LastName = "Goshev",
                Email = "gosho@gosho.bg",
                Comment = "asdasdasdasd",
                IsSolved = false,
                Type = "Gosho"
            },
            new Feedback
            {
                Id = 4,
                FirstName = "Gosho",
                LastName = "Goshev",
                Email = "gosho@gosho.bg",
                Comment = "asdasdasdasd",
                IsSolved = false,
                Type = "Gosho"
            },
            new Feedback
            {
                Id = 5,
                FirstName = "Gosho",
                LastName = "Goshev",
                Email = "evlogi@gosho.bg",
                Comment = "asdasdasdasd",
                IsSolved = false,
                Type = "Gosho"
            },
        };

        await dataMock.Feedbacks.AddRangeAsync(feedbacks);
        await dataMock.SaveChangesAsync();

        var service = new FeedbackService(dataMock, mapperMock);
        var result = await service.GetById(6);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetByIdShouldReturnNullIfEntityIsDeleted()
    {
        var dataMock = DatabaseMock.Instance;
        var mapperMock = MapperMock.Instance;

        var feedbacks = new List<Feedback>
        {
            new Feedback
            {
                Id = 1,
                FirstName = "Gosho",
                LastName = "Goshev",
                Email = "gosho@gosho.bg",
                Comment = "asdasdasdasd",
                IsSolved = false,
                Type = "Gosho"
            },
            new Feedback
            {
                Id = 2,
                FirstName = "Gosho",
                LastName = "Goshev",
                Email = "gosho@gosho.bg",
                Comment = "asdasdasdasd",
                IsSolved = false,
                Type = "Gosho"
            },
            new Feedback
            {
                Id = 3,
                FirstName = "Gosho",
                LastName = "Goshev",
                Email = "gosho@gosho.bg",
                Comment = "asdasdasdasd",
                IsSolved = false,
                Type = "Gosho"
            },
            new Feedback
            {
                Id = 4,
                FirstName = "Gosho",
                LastName = "Goshev",
                Email = "gosho@gosho.bg",
                Comment = "asdasdasdasd",
                IsSolved = false,
                Type = "Gosho"
            },
            new Feedback
            {
                Id = 5,
                FirstName = "Gosho",
                LastName = "Goshev",
                Email = "evlogi@gosho.bg",
                Comment = "asdasdasdasd",
                IsSolved = false,
                Type = "Gosho",
                IsDeleted = true
            },
        };

        await dataMock.Feedbacks.AddRangeAsync(feedbacks);
        await dataMock.SaveChangesAsync();

        var service = new FeedbackService(dataMock, mapperMock);
        var result = await service.GetById(5);

        Assert.Null(result);
    }

    [Fact]
    public async Task SolveFeedbackShouldWorkCorrectly()
    {
        var data = DatabaseMock.Instance;
        var mapper = MapperMock.Instance;

        var feedback = new Feedback()
        {
            Id = 1,
            FirstName = "Gosho",
            LastName = "Peshev",
            Email = "gosho@abv.bg",
            Type = "gosho",
            Comment = "goshogosho"
        };

        await data.Feedbacks.AddAsync(feedback);
        await data.SaveChangesAsync();

        var service = new FeedbackService(data, mapper);

        await service.SolveFeedback(1);

        var res = await data.Feedbacks.FirstOrDefaultAsync(x => x.Id == 1);
            
        Assert.True(res.IsSolved);
    }
}