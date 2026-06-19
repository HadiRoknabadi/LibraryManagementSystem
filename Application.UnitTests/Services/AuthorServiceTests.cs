using Application.DTOs.Author;
using Application.Services.Implementations;
using Application.Services.Interfaces.Context;
using AutoMapper;
using Domain.Entities.Book;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Persistence.Context;

public class AuthorServiceTests
{
    private IDatabaseContext GetContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new ApplicationDbContext(options);
    }

    private IMapper GetMapper()
    {
        var loggerFactory = LoggerFactory.Create(builder => { });

        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Author, AuthorListItemDTO>();
            cfg.CreateMap<AddAuthorDTO, Author>();
            cfg.CreateMap<EditAuthorDTO, Author>();
        }, loggerFactory);

        return config.CreateMapper();
    }

    [Theory]
    [AutoFixture.Xunit2.AutoData]
    public async Task AddAuthorAsync_Should_Add_Author(AddAuthorDTO dto)
    {
        // Arrange
        var context = GetContext();
        var mapper = GetMapper();
        var service = new AuthorService(context, mapper);

        // Act
        var result = await service.AddAuthorAsync(dto);

        // Assert
        Assert.Equal(AddAuthorResult.Success, result.Status);
        Assert.Equal(1, context.Authors.Count());
    }

    [Theory]
    [AutoFixture.Xunit2.AutoData]
    public async Task GetAuthorByIdAsync_Should_Return_Author(string name, string family)
    {
        // Arrange
        var context = GetContext();
        var mapper = GetMapper();

        var author = new Author
        {
            Name = name,
            Family = family
        };

        context.Authors.Add(author);
        await context.SaveChangesAsync();

        var service = new AuthorService(context, mapper);

        // Act
        var result = await service.GetAuthorByIdAsync(author.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(author.Id, result.Id);
    }

    [Theory]
    [AutoFixture.Xunit2.AutoData]
    public async Task EditAuthorAsync_Should_Edit_Author(string name, string family)
    {
        // Arrange
        var context = GetContext();
        var mapper = GetMapper();

        var author = new Author
        {
            Name = name,
            Family = family
        };

        context.Authors.Add(author);
        await context.SaveChangesAsync();

        var service = new AuthorService(context, mapper);

        var dto = new EditAuthorDTO
        {
            Id = author.Id,
            Name = name + "edit",
            Family = family + "edit"
        };

        // Act
        var result = await service.EditAuthorAsync(dto);

        // Assert
        Assert.Equal(EditAuthorResult.Success, result.Status);
    }

    [Theory]
    [AutoFixture.Xunit2.AutoData]
    public async Task DeleteAuthorAsync_Should_SoftDelete_Author(string name, string family)
    {
        // Arrange
        var context = GetContext();
        var mapper = GetMapper();

        var author = new Author
        {
            Name = name,
            Family = family
        };

        context.Authors.Add(author);
        await context.SaveChangesAsync();

        var service = new AuthorService(context, mapper);

        // Act
        var result = await service.DeleteAuthorAsync(author.Id);

        // Assert
        Assert.Equal(DeleteAuthorResult.Success, result.Status);
        Assert.True(author.IsDelete);
    }

    [Fact]
    public async Task GetAllAuthorsAsync_Should_Return_List()
    {
        // Arrange
        var context = GetContext();
        var mapper = GetMapper();

        context.Authors.Add(new Author
        {
            Name = "test",
            Family = "test"
        });

        await context.SaveChangesAsync();

        var service = new AuthorService(context, mapper);

        // Act
        var result = await service.GetAllAuthorsAsync();

        // Assert
        Assert.NotEmpty(result.Data);
    }

    [Fact]
    public async Task FilterAuthorAsync_Should_Return_Filtered_Result()
    {
        // Arrange
        var context = GetContext();
        var mapper = GetMapper();

        context.Authors.Add(new Author
        {
            Name = "Ali",
            Family = "Ahmadi",
            CreateDate = DateTime.Now
        });

        await context.SaveChangesAsync();

        var service = new AuthorService(context, mapper);

        var filter = new FilterAuthorDTO
        {
            Name = "Ali",
            PageId = 1,
            TakeEntity = 10
        };

        // Act
        var result = await service.FilterAuthorAsync(filter);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result.Authors);
    }
}
