using Application.DTOs.Borrowing;
using Application.Services.Implementations;
using AutoMapper;
using Domain.Entities.Account;
using Domain.Entities.Book;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Persistence.Context;

public class BorrowingServiceTests
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
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<SubmitBorrowDTO, Borrowing>();
            cfg.CreateMap<EditBorrowDTO, Borrowing>();

            cfg.CreateMap<Borrowing, BorrowingListItemDTO>()
                .ForMember(d => d.BookName,
                    o => o.MapFrom(s => s.BookCopy.Book.Title));

            cfg.CreateMap<Borrowing, BorrowDetailsDTO>();

            cfg.CreateMap<Borrowing, BorrowingReportDTO>()
                .ForMember(d => d.BookName,
                    o => o.MapFrom(s => s.BookCopy.Book.Title));
        }, LoggerFactory.Create(b => { }));

        return config.CreateMapper();
    }

    [Theory]
    [AutoDomainData]
    public async Task GetBorrowByIdAsync_ShouldReturnBorrow_WhenBorrowExists(Borrowing borrow)
    {
        // Arrange
        var context = GetDbContext();
        var mapper = GetMapper();
        var service = new BorrowingService(context, mapper);

        borrow.IsDelete = false;

        await context.Borrowings.AddAsync(borrow);
        await context.SaveChangesAsync();

        // Act
        var result = await service.GetBorrowByIdAsync(borrow.Id);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(borrow.Id);
    }

    [Theory]
    [AutoDomainData]
    public async Task SubmitBorrowAsync_ShouldCreateBorrow_WhenDueDateValid(SubmitBorrowDTO dto, int librarianId)
    {
        // Arrange
        var context = GetDbContext();
        var mapper = GetMapper();
        var service = new BorrowingService(context, mapper);

        dto.DueDate = "1406/01/01";

        // Act
        var result = await service.SubmitBorrowAsync(librarianId, dto);

        // Assert
        result.Status.Should().Be(SubmitBorrowResult.Success);
        (await context.Borrowings.CountAsync()).Should().Be(1);
    }

    [Theory]
    [AutoDomainData]
    public async Task SubmitBorrowAsync_ShouldReturnError_WhenDueDatePassed(SubmitBorrowDTO dto, int librarianId)
    {
        // Arrange
        var context = GetDbContext();
        var mapper = GetMapper();
        var service = new BorrowingService(context, mapper);

        dto.DueDate = "1400/01/01";

        // Act
        var result = await service.SubmitBorrowAsync(librarianId, dto);

        // Assert
        result.Status.Should().Be(SubmitBorrowResult.DueDatePassedFromNow);
    }

    [Theory]
    [AutoDomainData]
    public async Task EditBorrowAsync_ShouldUpdateBorrow(Borrowing borrow, EditBorrowDTO dto)
    {
        // Arrange
        var context = GetDbContext();
        var mapper = GetMapper();
        var service = new BorrowingService(context, mapper);

        borrow.IsDelete = false;

        await context.Borrowings.AddAsync(borrow);
        await context.SaveChangesAsync();

        dto.Id = borrow.Id;
        dto.DueDate = "1406/01/01";

        // Act
        var result = await service.EditBorrowAsync(dto);

        // Assert
        result.Status.Should().Be(EditBorrowResult.Success);
    }

    [Theory]
    [AutoDomainData]
    public async Task EditBorrowAsync_ShouldReturnNotFound_WhenBorrowNotExists(EditBorrowDTO dto)
    {
        // Arrange
        var context = GetDbContext();
        var mapper = GetMapper();
        var service = new BorrowingService(context, mapper);

        dto.DueDate = "1406/01/01";

        // Act
        var result = await service.EditBorrowAsync(dto);

        // Assert
        result.Status.Should().Be(EditBorrowResult.NotFound);
    }

    [Theory]
    [AutoDomainData]
    public async Task ReturnBorrowAsync_ShouldUpdateStatusToReturned(Borrowing borrow)
    {
        // Arrange
        var context = GetDbContext();
        var mapper = GetMapper();
        var service = new BorrowingService(context, mapper);

        borrow.IsDelete = false;
        borrow.Status = BorrowingStatus.Borrowed;

        await context.Borrowings.AddAsync(borrow);
        await context.SaveChangesAsync();

        // Act
        var result = await service.ReturnBorrowAsync(borrow.Id);

        // Assert
        result.Status.Should().Be(ReturnBorrowResult.Success);

        var updated = await context.Borrowings.SingleAsync(x => x.Id == borrow.Id);
        updated.Status.Should().Be(BorrowingStatus.Returned);
    }

    [Theory]
    [AutoDomainData]
    public async Task DeleteBorrowAsync_ShouldSoftDeleteBorrow(Borrowing borrow)
    {
        // Arrange
        var context = GetDbContext();
        var mapper = GetMapper();
        var service = new BorrowingService(context, mapper);

        borrow.IsDelete = false;

        await context.Borrowings.AddAsync(borrow);
        await context.SaveChangesAsync();

        // Act
        var result = await service.DeleteBorrowAsync(borrow.Id);

        // Assert
        result.Status.Should().Be(DeleteBorrowResult.Success);

        var entity = await context.Borrowings.IgnoreQueryFilters()
            .SingleAsync(x => x.Id == borrow.Id);

        entity.IsDelete.Should().BeTrue();
    }

    [Theory]
    [AutoDomainData]
    public async Task GetBorrowDetailsAsync_ShouldReturnDetails_WhenBorrowExists(
        Borrowing borrow,
        User user,
        User librarian,
        BookCopy copy,
        Book book)
    {
        // Arrange
        var context = GetDbContext();
        var mapper = GetMapper();
        var service = new BorrowingService(context, mapper);

        borrow.IsDelete = false;
        book.IsDelete = false;
        copy.IsDelete = false;

        await context.Users.AddRangeAsync(user, librarian);
        await context.Books.AddAsync(book);
        await context.SaveChangesAsync();

        copy.BookId = book.Id;
        copy.Book = null;

        await context.BookCopies.AddAsync(copy);
        await context.SaveChangesAsync();

        borrow.UserId = user.Id;
        borrow.LibrarianId = librarian.Id;
        borrow.BookCopyId = copy.Id;

        borrow.User = null;
        borrow.Librarian = null;
        borrow.BookCopy = null;

        await context.Borrowings.AddAsync(borrow);
        await context.SaveChangesAsync();

        var borrowId = borrow.Id;

        // Act
        var result = await service.GetBorrowDetailsAsync(borrowId);

        // Assert
        result.Status.Should().Be(GetBorrowDetailsResult.Success);
        result.Data.Should().NotBeNull();
    }






    [Theory]
    [AutoDomainData]
    public async Task GetBorrowingsForReportAsync_ShouldReturnBorrowings(
        Borrowing borrow,
        User user,
        User librarian,
        BookCopy copy,
        Book book)
    {
        // Arrange
        var context = GetDbContext();
        var mapper = GetMapper();
        var service = new BorrowingService(context, mapper);

        borrow.IsDelete = false;
        book.IsDelete = false;
        copy.IsDelete = false;

        await context.Users.AddRangeAsync(user, librarian);
        await context.Books.AddAsync(book);
        await context.SaveChangesAsync();

        copy.BookId = book.Id;
        copy.Book = null;

        await context.BookCopies.AddAsync(copy);
        await context.SaveChangesAsync();

        borrow.UserId = user.Id;
        borrow.LibrarianId = librarian.Id;
        borrow.BookCopyId = copy.Id;

        borrow.User = null;
        borrow.Librarian = null;
        borrow.BookCopy = null;

        await context.Borrowings.AddAsync(borrow);
        await context.SaveChangesAsync();

        // Act
        var result = await service.GetBorrowingsForReportAsync();

        // Assert
        result.Status.Should().Be(GetBorrowingsResult.Success);
        result.Data.Should().NotBeNull();
        result.Data.Count.Should().BeGreaterThan(0);
    }

}
