using Domain.Entities.Account;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Context;
using System.Net;
using Xunit;

namespace WebSite.EndPoint.E2ETests.Controllers;

public class AccountControllerTests
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public AccountControllerTests()
    {
        _factory =
            new WebApplicationFactory<Program>();

        _client =
            _factory.CreateClient(
                new WebApplicationFactoryClientOptions
                {
                    AllowAutoRedirect = false,
                    HandleCookies = true
                });
    }

    private async Task SeedUser()
    {
        using var scope =
            _factory.Services
                .CreateScope();

        var userManager =
            scope.ServiceProvider
                .GetRequiredService<
                    UserManager<User>>();

        var existing =
            await userManager
                .FindByNameAsync(
                    "09123456789");

        if (existing != null)
            return;

        var user =
            new User
            {
                UserName =
                    "09123456789",

                PhoneNumber =
                    "09123456789",

                PhoneNumberConfirmed =
                    true,

                Name =
                    "Test",

                Family =
                    "User"
            };

        var result =
            await userManager
                .CreateAsync(
                    user,
                    "Password123!");

        result.Succeeded
            .Should()
            .BeTrue();
    }

    [Fact]
    public async Task Login_Get_ShouldReturnOk_AndRenderLoginForm()
    {
        var response =
            await _client.GetAsync(
                "/Login");

        response.StatusCode
            .Should()
            .Be(
                HttpStatusCode.OK);

        var html =
            await response.Content
                .ReadAsStringAsync();

        html.Should()
            .Contain("PhoneNumber");

        html.Should()
            .Contain("Password");
    }

    [Fact]
    public async Task Login_Post_WithValidCredentials_ShouldRedirect()
    {
        await SeedUser();

        var form =
            new Dictionary<string, string>
            {
                ["PhoneNumber"] =
                    "09123456789",

                ["Password"] =
                    "Password123!",

                ["RememberMe"] =
                    "true"
            };

        var response =
            await _client.PostAsync(
                "/Login",
                new FormUrlEncodedContent(
                    form));

        var body =
            await response.Content
                .ReadAsStringAsync();

        response.StatusCode
            .Should()
            .Be(
                HttpStatusCode.Found,
                body);

        response.Headers.Location
            .Should()
            .NotBeNull();
    }
}