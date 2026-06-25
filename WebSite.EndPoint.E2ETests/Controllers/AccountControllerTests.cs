using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Text.RegularExpressions;
using WebSite.EndPoint.E2ETests.Infrastructure;
using Xunit;

namespace WebSite.EndPoint.E2ETests.Controllers;

public class AccountControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public AccountControllerTests(CustomWebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false,
            HandleCookies = true
        });
    }

    [Fact]
    public async Task Login_Get_ShouldReturnOk_AndRenderLoginForm()
    {
        var response = await _client.GetAsync("/Login");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var html = await response.Content.ReadAsStringAsync();

        html.Should().Contain("PhoneNumber");
        html.Should().Contain("Password");
        html.Should().Contain("RememberMe");
    }

    [Fact]
    public async Task Login_Post_WithValidCredentials_ShouldRedirectToDashboard()
    {
        var antiForgeryToken = await GetAntiForgeryTokenAsync();

        var form = new Dictionary<string, string>
        {
            ["__RequestVerificationToken"] = antiForgeryToken,
            ["PhoneNumber"] = "09123456789",
            ["Password"] = "Password123!",   // ← علامت ! اضافه شد
            ["RememberMe"] = "true",
            ["ReturnUrl"] = string.Empty
        };

        var response = await _client.PostAsync("/Login", new FormUrlEncodedContent(form));
        var html = await response.Content.ReadAsStringAsync();

        if (response.StatusCode != HttpStatusCode.Found)
        {
            throw new Xunit.Sdk.XunitException(
                $"Expected 302 but got {(int)response.StatusCode}.\n\nResponse HTML:\n{html}");
        }

        response.StatusCode.Should().Be(HttpStatusCode.Found);
    }

    private async Task<string> GetAntiForgeryTokenAsync()
    {
        var response = await _client.GetAsync("/Login");
        var html = await response.Content.ReadAsStringAsync();

        // Regex اصلاح‌شده — بدون escape مضاعف
        var match = Regex.Match(
            html,
            @"name=[""']?__RequestVerificationToken[""']?\s*(?:type=[""']?hidden[""']?\s+)?value=[""']?([^""'\s>]+)[""']?",
            RegexOptions.IgnoreCase);

        match.Success.Should().BeTrue("the login page must emit an anti-forgery token");

        return match.Groups[1].Value;
    }
}
