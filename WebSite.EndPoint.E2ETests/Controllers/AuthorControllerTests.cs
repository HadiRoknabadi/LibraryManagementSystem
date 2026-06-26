using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Context;
using Domain.Entities.Book;
using WebSite.EndPoint.Tests.E2E.Fixtures;
using Xunit;

namespace WebSite.EndPoint.E2ETests.Controllers;

public class AuthorControllerTests
    : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory<Program> _factory;

    public AuthorControllerTests(
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

    private async Task<int> SeedAuthorAsync()
    {
        using var scope =
            _factory.Services.CreateScope();

        var db =
            scope.ServiceProvider
                .GetRequiredService<ApplicationDbContext>();

        var author =
            new Author
            {
                Name = "Ali",
                Family = "Ahmadi",
                CreateDate = DateTime.Now
            };

        db.Authors.Add(author);

        await db.SaveChangesAsync();

        return author.Id;
    }

    [Fact]
    public async Task Authors_Should_ReturnOk()
    {
        await ResetDatabase();

        var response =
            await _client.GetAsync(
                "/Authors");

        response.StatusCode
            .Should()
            .Be(
                HttpStatusCode.OK);
    }

    [Fact]
    public async Task AddAuthor_WithValidData_ShouldSucceed()
    {
        await ResetDatabase();

        var form =
            new Dictionary<string, string>
            {
                ["Name"] = "محمد",
                ["Family"] = "احمدی"
            };

        var response =
            await _client.PostAsync(
                "/AddAuthor",
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
                "Success");
    }

    [Fact]
    public async Task AddAuthor_WithInvalidData_ShouldFail()
    {
        await ResetDatabase();

        var response =
            await _client.PostAsync(
                "/AddAuthor",
                new FormUrlEncodedContent(
                    new Dictionary<string, string>()));

        var body =
            await response.Content
                .ReadAsStringAsync();

        body.Should()
            .Contain(
                "Error");
    }

    [Fact]
    public async Task EditAuthor_WithValidData_ShouldSucceed()
    {
        await ResetDatabase();

        var id =
            await SeedAuthorAsync();

        var form =
            new Dictionary<string, string>
            {
                ["Id"] = id.ToString(),
                ["Name"] = "Edited",
                ["Family"] = "Edited"
            };

        var response =
            await _client.PostAsync(
                "/EditAuthor",
                new FormUrlEncodedContent(form));

        var body =
            await response.Content
                .ReadAsStringAsync();

        body.Should()
            .Contain(
                "Success");
    }

    [Fact]
    public async Task DeleteAuthor_WithValidId_ShouldSucceed()
    {
        await ResetDatabase();

        var id =
            await SeedAuthorAsync();

        var response =
            await _client.GetAsync(
                $"/DeleteAuthor/{id}");

        var body =
            await response.Content
                .ReadAsStringAsync();

        body.Should()
            .Contain(
                "Success");
    }

    [Fact]
    public async Task DeleteAuthor_WithWrongId_ShouldReturnWarning()
    {
        await ResetDatabase();

        var response =
            await _client.GetAsync(
                "/DeleteAuthor/999");

        var body =
            await response.Content
                .ReadAsStringAsync();

        body.Should()
            .Contain(
                "Warning");
    }
}