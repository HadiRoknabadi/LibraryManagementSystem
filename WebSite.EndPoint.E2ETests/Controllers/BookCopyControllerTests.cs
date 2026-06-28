using System.Net;
using Domain.Entities.Book;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Context;
using WebSite.EndPoint.Tests.E2E.Fixtures;
using Xunit;

namespace WebSite.EndPoint.E2ETests.Controllers;

public class BookCopyControllerTests
    : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory<Program> _factory;

    public BookCopyControllerTests(
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

    private async Task<int> SeedBook()
    {
        using var scope =
            _factory.Services.CreateScope();

        var db =
            scope.ServiceProvider
                .GetRequiredService<ApplicationDbContext>();

        var category =
            new BookCategory
            {
                Title = "Category",
                CreateDate = DateTime.Now
            };

        var publisher =
            new Publisher
            {
                Name = "Publisher",
                CreateDate = DateTime.Now
            };

        db.BookCategories.Add(category);
        db.Publishers.Add(publisher);

        await db.SaveChangesAsync();

        var book =
            new Book
            {
                Title = "Book Test",

                ISBN = "9786001234567",


                CategoryId = category.Id,

                PublisherId = publisher.Id,

                CreateDate = DateTime.Now
            };

        db.Books.Add(book);

        await db.SaveChangesAsync();

        return book.Id;
    }

    private async Task<int> SeedBookCopy()
    {
        var bookId =
            await SeedBook();

        using var scope =
            _factory.Services.CreateScope();

        var db =
            scope.ServiceProvider
                .GetRequiredService<ApplicationDbContext>();

        var copy =
            new BookCopy
            {
                BookId = bookId,

                ShelfLocation =
                    "A-12",

                InventoryCode =
                    "INV-1001",

                CreateDate =
                    DateTime.Now
            };

        db.BookCopies.Add(copy);

        await db.SaveChangesAsync();

        return copy.Id;
    }

    [Fact]
    public async Task BookCopies_Should_ReturnOk()
    {
        await ResetDatabase();

        var response =
            await _client.GetAsync(
                "/BookCopies");

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
    public async Task AddBookCopy_WithValidData_ShouldReturnSuccess()
    {
        await ResetDatabase();

        var bookId =
            await SeedBook();

        var form =
            new Dictionary<string, string>
            {
                ["BookId"] =
                    bookId.ToString()
            };

        var response =
            await _client.PostAsync(
                "/AddBookCopy",
                new FormUrlEncodedContent(form));

        var body =
            await response.Content
                .ReadAsStringAsync();

        response.StatusCode
            .Should()
            .Be(
                HttpStatusCode.OK,
                body);

        body.Should()
            .Contain(
                "Success");
    }

    [Fact]
    public async Task AddBookCopy_WithInvalidData_ShouldReturnError()
    {
        await ResetDatabase();

        var bookId =
            await SeedBook();

        var form =
            new Dictionary<string, string>
            {
                ["BookId"] =
                    bookId.ToString(),

                ["ShelfLocation"] =
                    new string('A', 101) // بیشتر از حد مجاز
            };

        var response =
            await _client.PostAsync(
                "/AddBookCopy",
                new FormUrlEncodedContent(form));

        var body =
            await response.Content
                .ReadAsStringAsync();

        response.StatusCode
            .Should()
            .Be(
                HttpStatusCode.OK);

        body.Should()
            .Contain(
                "Error");
    }

    [Fact]
    public async Task EditBookCopy_WithValidData_ShouldReturnSuccess()
    {
        await ResetDatabase();

        var copyId =
            await SeedBookCopy();

        using var scope =
            _factory.Services.CreateScope();

        var db =
            scope.ServiceProvider
                .GetRequiredService<ApplicationDbContext>();

        var book =
            db.Books.First();

        var form =
            new Dictionary<string, string>
            {
                ["Id"] =
                    copyId.ToString(),

                ["BookId"] =
                    book.Id.ToString()
            };

        var response =
            await _client.PostAsync(
                "/EditBookCopy",
                new FormUrlEncodedContent(form));

        var body =
            await response.Content
                .ReadAsStringAsync();

        body.Should()
            .Contain(
                "Success");
    }

    [Fact]
    public async Task EditBookCopy_WithWrongId_ShouldReturnWarning()
    {
        await ResetDatabase();

        var bookId =
            await SeedBook();

        var form =
            new Dictionary<string, string>
            {
                ["Id"] = "999",

                ["BookId"] =
                    bookId.ToString()
            };

        var response =
            await _client.PostAsync(
                "/EditBookCopy",
                new FormUrlEncodedContent(form));

        var body =
            await response.Content
                .ReadAsStringAsync();

        body.Should()
            .Contain(
                "Warning");
    }

    [Fact]
    public async Task DeleteBookCopy_WithValidId_ShouldReturnSuccess()
    {
        await ResetDatabase();

        var id =
            await SeedBookCopy();

        var response =
            await _client.GetAsync(
                $"/DeleteBookCopy/{id}");

        var body =
            await response.Content
                .ReadAsStringAsync();

        body.Should()
            .Contain(
                "Success");
    }

    [Fact]
    public async Task DeleteBookCopy_WithWrongId_ShouldReturnWarning()
    {
        await ResetDatabase();

        var response =
            await _client.GetAsync(
                "/DeleteBookCopy/999");

        var body =
            await response.Content
                .ReadAsStringAsync();

        body.Should()
            .Contain(
                "Warning");
    }
}