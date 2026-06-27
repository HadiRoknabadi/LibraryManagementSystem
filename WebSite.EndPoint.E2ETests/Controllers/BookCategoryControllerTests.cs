using System.Net;
using Domain.Entities.Book;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Context;
using WebSite.EndPoint.Tests.E2E.Fixtures;
using Xunit;

namespace WebSite.EndPoint.E2ETests.Controllers;

public class BookCategoryControllerTests
    : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory<Program> _factory;

    public BookCategoryControllerTests(
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

    private async Task<int> SeedCategory()
    {
        using var scope =
            _factory.Services.CreateScope();

        var db =
            scope.ServiceProvider
                .GetRequiredService<ApplicationDbContext>();

        var category =
            new BookCategory
            {
                Title = "Test Category",
                CreateDate = DateTime.Now
            };

        db.BookCategories.Add(category);

        await db.SaveChangesAsync();

        return category.Id;
    }

    [Fact]
    public async Task BookCategories_Should_ReturnOk()
    {
        await ResetDatabase();

        var response =
            await _client.GetAsync(
                "/BookCategories");

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
    public async Task AddBookCategory_WithValidData_ShouldSucceed()
    {
        await ResetDatabase();

        var form =
            new Dictionary<string, string>
            {
                ["Title"] = "رمان"
            };

        var response =
            await _client.PostAsync(
                "/AddBookCategory",
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
            .Contain("Success");
    }

    [Fact]
    public async Task AddBookCategory_WithInvalidData_ShouldFail()
    {
        await ResetDatabase();

        var response =
            await _client.PostAsync(
                "/AddBookCategory",
                new FormUrlEncodedContent(
                    new Dictionary<string, string>()));

        var body =
            await response.Content
                .ReadAsStringAsync();

        response.StatusCode
            .Should()
            .Be(
                HttpStatusCode.OK);

        body.Should()
            .Contain("Error");
    }

    [Fact]
    public async Task EditBookCategory_WithValidData_ShouldSucceed()
    {
        await ResetDatabase();

        var id =
            await SeedCategory();

        var form =
            new Dictionary<string, string>
            {
                ["Id"] = id.ToString(),
                ["Title"] = "Edited"
            };

        var response =
            await _client.PostAsync(
                "/EditBookCategory",
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
            .Contain("Success");
    }

    [Fact]
    public async Task EditBookCategory_WithWrongId_ShouldReturnWarning()
    {
        await ResetDatabase();

        var form =
            new Dictionary<string, string>
            {
                ["Id"] = "999",
                ["Title"] = "Edited"
            };

        var response =
            await _client.PostAsync(
                "/EditBookCategory",
                new FormUrlEncodedContent(form));

        var body =
            await response.Content
                .ReadAsStringAsync();

        body.Should()
            .Contain("Warning");
    }

    [Fact]
    public async Task DeleteBookCategory_WithValidId_ShouldSucceed()
    {
        await ResetDatabase();

        var id =
            await SeedCategory();

        var response =
            await _client.GetAsync(
                $"/DeleteBookCategory/{id}");

        var body =
            await response.Content
                .ReadAsStringAsync();

        response.StatusCode
            .Should()
            .Be(
                HttpStatusCode.OK,
                body);

        body.Should()
            .Contain("Success");
    }

    [Fact]
    public async Task DeleteBookCategory_WithWrongId_ShouldReturnWarning()
    {
        await ResetDatabase();

        var response =
            await _client.GetAsync(
                "/DeleteBookCategory/999");

        var body =
            await response.Content
                .ReadAsStringAsync();

        body.Should()
            .Contain("Warning");
    }
}