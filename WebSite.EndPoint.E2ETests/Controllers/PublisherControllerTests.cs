using System.Net;
using Domain.Entities.Book;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Context;
using WebSite.EndPoint.Tests.E2E.Fixtures;
using Xunit;

namespace WebSite.EndPoint.E2ETests.Controllers;

public class PublisherControllerTests
    : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory<Program> _factory;

    public PublisherControllerTests(
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

    private async Task<int> SeedPublisher()
    {
        using var scope =
            _factory.Services.CreateScope();

        var db =
            scope.ServiceProvider
                .GetRequiredService<ApplicationDbContext>();

        var publisher =
            new Publisher
            {
                Name = "Test Publisher",
                CreateDate = DateTime.Now
            };

        db.Publishers.Add(
            publisher);

        await db.SaveChangesAsync();

        return publisher.Id;
    }

    [Fact]
    public async Task Publishers_Should_ReturnOk()
    {
        await ResetDatabase();

        var response =
            await _client.GetAsync(
                "/Publishers");

        response.StatusCode
            .Should()
            .Be(
                HttpStatusCode.OK);
    }

    [Fact]
    public async Task AddPublisher_WithValidData_ShouldReturnSuccess()
    {
        await ResetDatabase();

        var form =
            new Dictionary<string, string>
            {
                ["Name"] =
                    "Publisher 1"
            };

        var response =
            await _client.PostAsync(
                "/AddPublisher",
                new FormUrlEncodedContent(form));

        var body =
            await response.Content
                .ReadAsStringAsync();

        body.Should()
            .Contain(
                "Success");
    }

    [Fact]
    public async Task AddPublisher_WithInvalidData_ShouldReturnError()
    {
        await ResetDatabase();

        var response =
            await _client.PostAsync(
                "/AddPublisher",
                new FormUrlEncodedContent(
                    new Dictionary<string, string>
                    {
                        ["Name"] = ""
                    }));

        var body =
            await response.Content
                .ReadAsStringAsync();

        body.Should()
            .Contain(
                "Error");
    }

    [Fact]
    public async Task EditPublisher_WithValidData_ShouldReturnSuccess()
    {
        await ResetDatabase();

        var id =
            await SeedPublisher();

        var form =
            new Dictionary<string, string>
            {
                ["Id"] =
                    id.ToString(),

                ["Name"] =
                    "Edited Publisher"
            };

        var response =
            await _client.PostAsync(
                "/EditPublisher",
                new FormUrlEncodedContent(form));

        var body =
            await response.Content
                .ReadAsStringAsync();

        body.Should()
            .Contain(
                "Success");
    }

    [Fact]
    public async Task EditPublisher_WithWrongId_ShouldReturnWarning()
    {
        await ResetDatabase();

        var form =
            new Dictionary<string, string>
            {
                ["Id"] = "999",
                ["Name"] = "Edited"
            };

        var response =
            await _client.PostAsync(
                "/EditPublisher",
                new FormUrlEncodedContent(form));

        var body =
            await response.Content
                .ReadAsStringAsync();

        body.Should()
            .Contain(
                "Warning");
    }

    [Fact]
    public async Task DeletePublisher_WithValidId_ShouldReturnSuccess()
    {
        await ResetDatabase();

        var id =
            await SeedPublisher();

        var response =
            await _client.GetAsync(
                $"/DeletePublisher/{id}");

        var body =
            await response.Content
                .ReadAsStringAsync();

        body.Should()
            .Contain(
                "Success");
    }

    [Fact]
    public async Task DeletePublisher_WithWrongId_ShouldReturnWarning()
    {
        await ResetDatabase();

        var response =
            await _client.GetAsync(
                "/DeletePublisher/999");

        var body =
            await response.Content
                .ReadAsStringAsync();

        body.Should()
            .Contain(
                "Warning");
    }
}