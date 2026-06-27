using System.Net;
using Domain.Entities.Book;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Context;
using WebSite.EndPoint.Tests.E2E.Fixtures;
using Xunit;

namespace WebSite.EndPoint.E2ETests.Controllers;

public class BookControllerTests
    : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory<Program> _factory;

    public BookControllerTests(
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

    private async Task<(int categoryId, int authorId, int publisherId)>
        SeedDependencies()
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

        var author =
            new Author
            {
                Name = "Ali",
                Family = "Ahmadi",
                CreateDate = DateTime.Now
            };

        var publisher =
            new Publisher
            {
                Name = "Publisher",
                CreateDate = DateTime.Now
            };

        db.BookCategories.Add(category);
        db.Authors.Add(author);
        db.Publishers.Add(publisher);

        await db.SaveChangesAsync();

        return (
            category.Id,
            author.Id,
            publisher.Id
        );
    }

    private async Task<int> SeedBook()
    {
        var dep =
            await SeedDependencies();

        using var scope =
            _factory.Services.CreateScope();

        var db =
            scope.ServiceProvider
                .GetRequiredService<ApplicationDbContext>();

        var book =
            new Book
            {
                Title = "Book 1",

                ISBN = "9786001234567",

                CategoryId =
                    dep.categoryId,

                PublisherId =
                    dep.publisherId,

                CreateDate =
                    DateTime.Now
            };

        db.Books.Add(book);

        await db.SaveChangesAsync();

        return book.Id;
    }

    [Fact]
    public async Task Books_Should_ReturnOk()
    {
        await ResetDatabase();

        var response =
            await _client.GetAsync("/Books");

        response.StatusCode
            .Should()
            .Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task AddBook_Get_ShouldReturnOk()
    {
        await ResetDatabase();

        await SeedDependencies();

        var response =
            await _client.GetAsync("/AddBook");

        response.StatusCode
            .Should()
            .Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task AddBook_WithValidData_ShouldRedirect()
    {
        await ResetDatabase();

        var dep =
            await SeedDependencies();

        var form =
            new Dictionary<string, string>
            {
                ["Title"] = "Book Test",

                ["Description"] = "Desc",

                ["ISBN"] = "9786001234567",

                ["Count"] = "5",

                // اگر DTO لیست می‌خواهد
                ["BookCategoryIds[0]"] =
                    dep.categoryId.ToString(),

                ["AuthorIds[0]"] =
                    dep.authorId.ToString(),

                ["PublisherId"] =
                    dep.publisherId.ToString()
            };

        var response =
            await _client.PostAsync(
                "/AddBook",
                new FormUrlEncodedContent(form));

        var body =
            await response.Content
                .ReadAsStringAsync();

        response.StatusCode
            .Should()
            .Be(
                HttpStatusCode.Found,
                body);

        response.Headers.Location!
            .ToString()
            .Should()
            .Contain(
                "Books");
    }

    [Fact]
    public async Task EditBook_Get_WithValidId_ShouldReturnOk()
    {
        await ResetDatabase();

        var id =
            await SeedBook();

        var response =
            await _client.GetAsync(
                $"/EditBook/{id}");

        response.StatusCode
            .Should()
            .Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task EditBook_Post_WithValidData_ShouldRedirect()
    {
        await ResetDatabase();

        var id =
            await SeedBook();

        using var scope =
            _factory.Services.CreateScope();

        var db =
            scope.ServiceProvider
                .GetRequiredService<ApplicationDbContext>();

        var category =
            db.BookCategories.First();

        var author =
            db.Authors.First();

        var publisher =
            db.Publishers.First();

        var form =
            new Dictionary<string, string>
            {
                ["Id"] =
                    id.ToString(),

                ["Title"] =
                    "Edited Book",

                ["Description"] =
                    "Edited Description",

                ["ISBN"] =
                    "9786009999999",

                ["Count"] =
                    "10",

                // مهم ← لیستی بفرست
                ["BookCategoryIds[0]"] =
                    category.Id.ToString(),

                ["AuthorIds[0]"] =
                    author.Id.ToString(),

                ["PublisherId"] =
                    publisher.Id.ToString()
            };

        var response =
            await _client.PostAsync(
                $"/EditBook/{id}",
                new FormUrlEncodedContent(form));

        var body =
            await response.Content
                .ReadAsStringAsync();

        response.StatusCode
            .Should()
            .Be(
                HttpStatusCode.Found,
                body);

        response.Headers.Location!
            .ToString()
            .Should()
            .Contain(
                "Books");
    }

    [Fact]
    public async Task DeleteBook_WithValidId_ShouldReturnSuccess()
    {
        await ResetDatabase();

        var id =
            await SeedBook();

        var response =
            await _client.GetAsync(
                $"/DeleteBook/{id}");

        var body =
            await response.Content
                .ReadAsStringAsync();

        body.Should()
            .Contain("Success");
    }

    [Fact]
    public async Task DeleteBook_WithWrongId_ShouldReturnWarning()
    {
        await ResetDatabase();

        var response =
            await _client.GetAsync(
                "/DeleteBook/999");

        var body =
            await response.Content
                .ReadAsStringAsync();

        body.Should()
            .Contain("Warning");
    }
}