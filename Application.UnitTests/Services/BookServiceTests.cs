using Application.DTOs.Book;
using Application.DTOs.Common;
using Application.Services.Implementations;
using AutoMapper;
using Domain.Entities.Book;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Persistence.Context;

public class BookServiceTests
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
            cfg.CreateMap<Book, BookListItemDTO>();
            cfg.CreateMap<Book, EditBookDTO>();

            cfg.CreateMap<AddBookDTO, Book>();
            cfg.CreateMap<EditBookDTO, Book>();

        }, loggerFactory);

        return config.CreateMapper();
    }

    [Theory]
    [AutoDomainData]
    public async Task GetAllBooksAsync_ShouldReturnBooks(Book book)
    {
        // Arrange
        var context = GetDbContext();
        var mapper = GetMapper();
        var service = new BookService(context, mapper);

        book.IsDelete = false;

        await context.Books.AddAsync(book);
        await context.SaveChangesAsync();

        // Act
        var result = await service.GetAllBooksAsync();

        // Assert
        result.Status.Should().Be(GetBooksResult.Success);
        result.Data.Should().NotBeEmpty();
    }

    [Theory]
    [AutoDomainData]
    public async Task GetBookByIdAsync_ShouldReturnBook(Book book)
    {
        // Arrange
        var context = GetDbContext();
        var mapper = GetMapper();
        var service = new BookService(context, mapper);

        book.IsDelete = false;

        await context.Books.AddAsync(book);
        await context.SaveChangesAsync();

        // Act
        var result = await service.GetBookByIdAsync(book.Id);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(book.Id);
    }

    [Theory]
    [AutoDomainData]
    public async Task FilterBookAsync_ShouldReturnFilteredBooks(Book book)
    {
        // Arrange
        var context = GetDbContext();
        var mapper = GetMapper();
        var service = new BookService(context, mapper);

        book.IsDelete = false;
        book.Title = "Clean Code";
        book.ISBN = "12345";

        await context.Books.AddAsync(book);
        await context.SaveChangesAsync();

        var filter = new FilterBookDTO
        {
            Title = "Clean",
            ISBN = "123",
            PageId = 1,
            TakeEntity = 10,
            HowManyShowPageAfterAndBefore = 3,
            OrderBy = FilterDataOrder.CreateDate_DES
        };

        // Act
        var result = await service.FilterBookAsync(filter);

        // Assert
        result.Books.Should().NotBeEmpty();
    }

    [Theory]
    [AutoDomainData]
    public async Task GetBookDetailsForEditAsync_ShouldReturnNotFound(int id)
    {
        // Arrange
        var context = GetDbContext();
        var mapper = GetMapper();
        var service = new BookService(context, mapper);

        // Act
        var result = await service.GetBookDetailsForEditAsync(id);

        // Assert
        result.Status.Should().Be(GetBookDetailsResult.NotFound);
    }
}
