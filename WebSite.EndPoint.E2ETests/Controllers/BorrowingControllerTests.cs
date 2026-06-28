using System.Net;
using Domain.Entities.Account;
using Domain.Entities.Book;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Context;
using WebSite.EndPoint.Tests.E2E.Fixtures;
using Xunit;

namespace WebSite.EndPoint.E2ETests.Controllers;

public class BorrowingControllerTests
    : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory<Program> _factory;

    public BorrowingControllerTests(
        CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;

        _client =
            factory.CreateClient(
                new WebApplicationFactoryClientOptions
                {
                    AllowAutoRedirect = false
                });
    }

    private async Task ResetDatabase()
    {
        using var scope =
            _factory.Services.CreateScope();

        var db =
            scope.ServiceProvider
                .GetRequiredService<ApplicationDbContext>();

        await db.Database.EnsureDeletedAsync();
        await db.Database.EnsureCreatedAsync();
    }

    private async Task<(int userId, int copyId)> SeedDependencies()
    {
        using var scope =
            _factory.Services.CreateScope();

        var db =
            scope.ServiceProvider
                .GetRequiredService<ApplicationDbContext>();

        var user =
            new User
            {
                UserName = "09111111111",
                PhoneNumber = "09111111111",
                Name = "Test",
                Family = "User"
            };

        db.Users.Add(user);

        var category =
            new BookCategory
            {
                Title = "Category"
            };

        db.BookCategories.Add(category);

        var author =
            new Author
            {
                Name = "Ali",
                Family = "Ahmadi"
            };

        db.Authors.Add(author);

        var publisher =
            new Publisher
            {
                Name = "Publisher"
            };

        db.Publishers.Add(publisher);

        await db.SaveChangesAsync();

        var book =
            new Book
            {
                Title = "Book",

                ISBN =
                    "9786001234567",

                CategoryId =
                    category.Id,

                PublisherId =
                    publisher.Id
            };

        db.Books.Add(book);

        await db.SaveChangesAsync();

        var copy =
            new BookCopy
            {
                BookId =
                    book.Id,

                ShelfLocation =
                    "A1",

                InventoryCode =
                    "INV001"
            };

        db.BookCopies.Add(copy);

        await db.SaveChangesAsync();

        return (
            user.Id,
            copy.Id
        );
    }

    private async Task<int> SeedBorrow()
    {
        var dep =
            await SeedDependencies();

        using var scope =
            _factory.Services.CreateScope();

        var db =
            scope.ServiceProvider
                .GetRequiredService<ApplicationDbContext>();

        var borrow =
            new Borrowing
            {
                UserId =
                    dep.userId,

                BookCopyId =
                    dep.copyId,

                BorrowDate =
                    DateTime.Now,

                DueDate =
                    DateTime.Now.AddDays(7)
            };

        db.Borrowings.Add(borrow);

        await db.SaveChangesAsync();

        return borrow.Id;
    }

    [Fact]
    public async Task Borrowings_Should_ReturnOk()
    {
        await ResetDatabase();

        var response =
            await _client.GetAsync("/Borrowings");

        response.StatusCode
            .Should()
            .Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task BorrowDetails_WithValidId_ShouldReturnOk()
    {
        await ResetDatabase();

        var id =
            await SeedBorrow();

        var response =
            await _client.GetAsync(
                $"/BorrowDetails/{id}");

        var body =
            await response.Content
                .ReadAsStringAsync();

        response.StatusCode
            .Should()
            .Be(
                HttpStatusCode.OK,
                body);
    }

    [Fact]
    public async Task SubmitBorrow_WithValidData_ShouldReturnSuccess()
    {
        await ResetDatabase();

        var borrowId =
            await SeedBorrow();

        using var scope =
            _factory.Services.CreateScope();

        var db =
            scope.ServiceProvider
                .GetRequiredService<ApplicationDbContext>();

        var user =
            db.Users.First();

        var copy =
            db.BookCopies.First();

        var form =
            new Dictionary<string, string>
            {
                ["UserId"] =
                    user.Id.ToString(),

                ["BookCopyId"] =
                    copy.Id.ToString(),

                // تاریخ شمسی
                ["DueDate"] =
                    "1405/04/12"
            };

        var response =
            await _client.PostAsync(
                "/SubmitBorrow",
                new FormUrlEncodedContent(form));

        var body =
            await response.Content
                .ReadAsStringAsync();

        body.Should()
            .Contain("Success");
    }

    [Fact]
    public async Task EditBorrow_WithWrongId_ShouldReturnWarning()
    {
        await ResetDatabase();

        var form =
            new Dictionary<string, string>
            {
                ["Id"] = "999",

                ["BookCopyId"] = "1",

                ["UserId"] = "1",

                ["BorrowDate"] =
                    DateTime.Now
                        .ToString("yyyy-MM-dd"),

                ["DueDate"] =
                    DateTime.Now
                        .AddDays(7)
                        .ToString("yyyy-MM-dd")
            };

        var response =
            await _client.PostAsync(
                "/EditBorrow",
                new FormUrlEncodedContent(form));

        var body =
            await response.Content
                .ReadAsStringAsync();

        body.Should()
            .Contain("Warning");
    }

    [Fact]
    public async Task ReturnBorrow_WithValidId_ShouldReturnSuccess()
    {
        await ResetDatabase();

        var id =
            await SeedBorrow();

        var response =
            await _client.GetAsync(
                $"/ReturnBorrow/{id}");

        var body =
            await response.Content
                .ReadAsStringAsync();

        body.Should()
            .Contain("Success");
    }

    [Fact]
    public async Task DeleteBorrow_WithValidId_ShouldReturnSuccess()
    {
        await ResetDatabase();

        var id =
            await SeedBorrow();

        var response =
            await _client.GetAsync(
                $"/DeleteBorrow/{id}");

        var body =
            await response.Content
                .ReadAsStringAsync();

        body.Should()
            .Contain("Success");
    }

    [Fact]
    public async Task ExportToPdf_ShouldReturnPdf()
    {
        await ResetDatabase();

        await SeedBorrow();

        var response =
            await _client.GetAsync(
                "/GetBorrowingsReport");

        response.StatusCode
            .Should()
            .Be(HttpStatusCode.OK);

        response.Content
            .Headers
            .ContentType!
            .MediaType
            .Should()
            .Be("application/pdf");
    }
}