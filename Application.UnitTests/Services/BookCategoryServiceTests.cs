using Application.DTOs.BookCategory;
using Application.Services.Implementations;
using AutoMapper;
using Domain.Entities.Book;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Persistence.Context;

public class BookCategoryServiceTests
{
    private static ApplicationDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new ApplicationDbContext(options);
    }

    private static IMapper GetMapper()
    {
        var loggerFactory = LoggerFactory.Create(builder => { });

        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<BookCategory, BookCategoryListItemDTO>();
            cfg.CreateMap<AddBookCategoryDTO, BookCategory>();
            cfg.CreateMap<EditBookCategoryDTO, BookCategory>();
        }, loggerFactory);

        return config.CreateMapper();
    }

    [Theory]
    [AutoDomainData]
    public async Task GetAllBookCategoriesAsync_ShouldReturnCategories(List<BookCategory> categories)
    {
        // Arrange
        var context = GetDbContext();
        var mapper = GetMapper();
        var service = new BookCategoryService(context, mapper);

        await context.BookCategories.AddRangeAsync(categories);
        await context.SaveChangesAsync();

        // Act
        var result = await service.GetAllBookCategoriesAsync();

        // Assert
        result.Status.Should().Be(GetBookCategoriesResult.Success);
        result.Data.Should().NotBeEmpty();
    }

    [Theory]
    [AutoDomainData]
    public async Task FilterBookCategoryAsync_ShouldReturnFilteredData(BookCategory category)
    {
        // Arrange
        var context = GetDbContext();
        var mapper = GetMapper();
        var service = new BookCategoryService(context, mapper);

        category.IsDelete = false;

        await context.BookCategories.AddAsync(category);
        await context.SaveChangesAsync();

        var filter = new FilterBookCategoryDTO
        {
            Title = category.Title,
            PageId = 1,
            TakeEntity = 10
        };

        // Act
        var result = await service.FilterBookCategoryAsync(filter);

        // Assert
        result.BookCategories.Should().NotBeEmpty();
    }


    [Theory]
    [AutoDomainData]
    public async Task GetBookCategoryByIdAsync_ShouldReturnCategory(BookCategory category)
    {
        // Arrange
        var context = GetDbContext();
        var mapper = GetMapper();
        var service = new BookCategoryService(context, mapper);

        category.IsDelete = false;

        await context.BookCategories.AddAsync(category);
        await context.SaveChangesAsync();

        // Act
        var result = await service.GetBookCategoryByIdAsync(category.Id);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(category.Id);
    }


    [Theory]
    [AutoDomainData]
    public async Task AddBookCategoryAsync_ShouldAddCategory(AddBookCategoryDTO dto)
    {
        // Arrange
        var context = GetDbContext();
        var mapper = GetMapper();
        var service = new BookCategoryService(context, mapper);

        // Act
        var result = await service.AddBookCategoryAsync(dto);

        // Assert
        result.Status.Should().Be(AddBookCategoryResult.Success);
        context.BookCategories.Any().Should().BeTrue();
    }

    [Theory]
    [AutoDomainData]
    public async Task EditBookCategoryAsync_ShouldReturnNotFound(EditBookCategoryDTO dto)
    {
        // Arrange
        var context = GetDbContext();
        var mapper = GetMapper();
        var service = new BookCategoryService(context, mapper);

        // Act
        var result = await service.EditBookCategoryAsync(dto);

        // Assert
        result.Status.Should().Be(EditBookCategoryResult.NotFound);
    }

    [Theory]
    [AutoDomainData]
    public async Task EditBookCategoryAsync_ShouldEditCategory(BookCategory category, EditBookCategoryDTO dto)
    {
        // Arrange
        var context = GetDbContext();
        var mapper = GetMapper();
        var service = new BookCategoryService(context, mapper);

        category.Id = 100;
        category.IsDelete = false;

        await context.BookCategories.AddAsync(category);
        await context.SaveChangesAsync();

        dto.Id = category.Id;

        // Act
        var result = await service.EditBookCategoryAsync(dto);

        // Assert
        result.Status.Should().Be(EditBookCategoryResult.Success);
    }



    [Theory]
    [AutoDomainData]
    public async Task DeleteBookCategoryAsync_ShouldReturnNotFound(int id)
    {
        // Arrange
        var context = GetDbContext();
        var mapper = GetMapper();
        var service = new BookCategoryService(context, mapper);

        // Act
        var result = await service.DeleteBookCategoryAsync(id);

        // Assert
        result.Status.Should().Be(DeleteBookCategoryResult.NotFound);
    }

    [Theory]
    [AutoDomainData]
    public async Task DeleteBookCategoryAsync_ShouldSoftDelete(BookCategory category)
    {
        // Arrange
        var context = GetDbContext();
        var mapper = GetMapper();
        var service = new BookCategoryService(context, mapper);

        category.IsDelete = false;

        await context.BookCategories.AddAsync(category);
        await context.SaveChangesAsync();

        // Act
        var result = await service.DeleteBookCategoryAsync(category.Id);

        // Assert
        result.Status.Should().Be(DeleteBookCategoryResult.Success);

        var saved = await context.BookCategories
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(x => x.Id == category.Id);

        saved.Should().NotBeNull();
        saved.IsDelete.Should().BeTrue();
    }


}
