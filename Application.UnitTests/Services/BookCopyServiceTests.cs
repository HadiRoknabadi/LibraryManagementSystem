using Application.DTOs.BookCopy;
using Application.DTOs.Common;
using Application.Services.Implementations;
using AutoMapper;
using Domain.Entities.Book;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Persistence.Context;

public class BookCopyServiceTests
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
            cfg.CreateMap<BookCopy, BookCopyListItemDTO>()
                .ForMember(d => d.BookName, o => o.MapFrom(s => s.Book.Title));

            cfg.CreateMap<AddBookCopyDTO, BookCopy>();
            cfg.CreateMap<EditBookCopyDTO, BookCopy>();

        }, loggerFactory);

        return config.CreateMapper();
    }

    [Theory]
    [AutoDomainData]
    public async Task GetAllBookCopiesAsync_ShouldReturnCopies(BookCopy bookCopy, Book book)
    {
        // Arrange
        var context = GetDbContext();
        var mapper = GetMapper();
        var service = new BookCopyService(context, mapper);

        book.IsDelete = false;
        book.Title = "Test Book";

        await context.Books.AddAsync(book);
        await context.SaveChangesAsync();

        bookCopy.IsDelete = false;
        bookCopy.BookId = book.Id;
        bookCopy.Book = book;
        bookCopy.InventoryCode = "INV-100";

        await context.BookCopies.AddAsync(bookCopy);
        await context.SaveChangesAsync();

        // Act
        var result = await service.GetAllBookCopiesAsync();

        // Assert
        result.Status.Should().Be(GetAllBookCopiesResult.Success);
        result.Data.Should().NotBeEmpty();
    }


    [Theory]
    [AutoDomainData]
    public async Task GetBookCopyByIdAsync_ShouldReturnBookCopy(BookCopy bookCopy)
    {
        // Arrange
        var context = GetDbContext();
        var mapper = GetMapper();
        var service = new BookCopyService(context, mapper);

        await context.BookCopies.AddAsync(bookCopy);
        await context.SaveChangesAsync();

        // Act
        var result = await service.GetBookCopyByIdAsync(bookCopy.Id);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(bookCopy.Id);
    }

    [Theory]
    [AutoDomainData]
    public async Task FilterBookCopyAsync_ShouldReturnFilteredData(BookCopy bookCopy, Book book)
    {
        // Arrange
        var context = GetDbContext();
        var mapper = GetMapper();
        var service = new BookCopyService(context, mapper);

        book.IsDelete = false;
        book.Title = "Clean Code";

        await context.Books.AddAsync(book);
        await context.SaveChangesAsync();

        bookCopy.IsDelete = false;
        bookCopy.BookId = book.Id;
        bookCopy.Book = book;
        bookCopy.InventoryCode = "INV-100";
        bookCopy.Status = BookCopyStatus.Available;

        await context.BookCopies.AddAsync(bookCopy);
        await context.SaveChangesAsync();

        var filter = new FilterBookCopyDTO
        {
            Status = bookCopy.Status,
            BookName = "Clean",
            InventoryCode = "INV",
            PageId = 1,
            TakeEntity = 10,
            HowManyShowPageAfterAndBefore = 3,
            OrderBy = FilterDataOrder.CreateDate_DES
        };

        // Act
        var result = await service.FilterBookCopyAsync(filter);

        // Assert
        result.BookCopies.Should().NotBeEmpty();
    }


    [Theory]
    [AutoDomainData]
    public async Task AddBookCopyAsync_ShouldAddBookCopy(AddBookCopyDTO dto)
    {
        // Arrange
        var context = GetDbContext();
        var mapper = GetMapper();
        var service = new BookCopyService(context, mapper);

        // Act
        var result = await service.AddBookCopyAsync(dto);

        // Assert
        result.Status.Should().Be(AddBookCopyResult.Success);
        context.BookCopies.Any().Should().BeTrue();
    }

    [Theory]
    [AutoDomainData]
    public async Task EditBookCopyAsync_ShouldReturnNotFound(EditBookCopyDTO dto)
    {
        // Arrange
        var context = GetDbContext();
        var mapper = GetMapper();
        var service = new BookCopyService(context, mapper);

        // Act
        var result = await service.EditBookCopyAsync(dto);

        // Assert
        result.Status.Should().Be(EditBookCopyResult.NotFound);
    }

    [Theory]
    [AutoDomainData]
    public async Task EditBookCopyAsync_ShouldEditBookCopy(BookCopy bookCopy, EditBookCopyDTO dto)
    {
        // Arrange
        var context = GetDbContext();
        var mapper = GetMapper();
        var service = new BookCopyService(context, mapper);

        await context.BookCopies.AddAsync(bookCopy);
        await context.SaveChangesAsync();

        dto.Id = bookCopy.Id;

        // Act
        var result = await service.EditBookCopyAsync(dto);

        // Assert
        result.Status.Should().Be(EditBookCopyResult.Success);
    }

    [Theory]
    [AutoDomainData]
    public async Task DeleteBookCopyAsync_ShouldReturnNotFound(int id)
    {
        // Arrange
        var context = GetDbContext();
        var mapper = GetMapper();
        var service = new BookCopyService(context, mapper);

        // Act
        var result = await service.DeleteBookCopyAsync(id);

        // Assert
        result.Status.Should().Be(DeleteBookCopyResult.NotFound);
    }

    [Theory]
    [AutoDomainData]
    public async Task DeleteBookCopyAsync_ShouldSoftDelete(BookCopy bookCopy)
    {
        // Arrange
        var context = GetDbContext();
        var mapper = GetMapper();
        var service = new BookCopyService(context, mapper);

        bookCopy.IsDelete = false;

        await context.BookCopies.AddAsync(bookCopy);
        await context.SaveChangesAsync();

        // Act
        var result = await service.DeleteBookCopyAsync(bookCopy.Id);

        // Assert
        result.Status.Should().Be(DeleteBookCopyResult.Success);

        var saved = await context.BookCopies
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(x => x.Id == bookCopy.Id);

        saved.IsDelete.Should().BeTrue();
    }
}
