using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using WebSite.EndPoint.Tests.E2E.Fixtures;
using Xunit;

namespace WebSite.EndPoint.E2ETests.Controllers;

public class HomeControllerTests
    : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public HomeControllerTests(
        CustomWebApplicationFactory<Program> factory)
    {
        _client =
            factory.CreateClient(
                new WebApplicationFactoryClientOptions
                {
                    AllowAutoRedirect = false
                });
    }

    [Fact]
    public async Task Dashboard_ShouldReturnOk()
    {
        var response =
            await _client.GetAsync("/");

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
    public async Task Dashboard_ShouldRenderView()
    {
        var response =
            await _client.GetAsync("/");

        var body =
            await response.Content
                .ReadAsStringAsync();

        body.Should()
            .NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task Dashboard_ResponseShouldBeHtml()
    {
        var response =
            await _client.GetAsync("/");

        response.Content
            .Headers
            .ContentType!
            .MediaType
            .Should()
            .Be(
                "text/html");
    }
}