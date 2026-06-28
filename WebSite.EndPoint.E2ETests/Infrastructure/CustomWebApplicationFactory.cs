using Application.Services.Interfaces;
using DNTCaptcha.Core;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Persistence.Context;
using System.Security.Claims;
using System.Text.Encodings.Web;
using WebSite.EndPoint.Tests.E2E.Fakes;

namespace WebSite.EndPoint.Tests.E2E.Fixtures;

#region FakeCaptcha

public class FakeCaptchaValidatorService
    : IDNTCaptchaValidatorService
{
    public bool HasRequestValidCaptchaEntry()
    {
        return true;
    }
}

#endregion

#region FakeAuth

public class TestAuthHandler
    : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public TestAuthHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder)
        : base(options, logger, encoder)
    {
    }

    protected override Task<AuthenticateResult>
        HandleAuthenticateAsync()
    {
        var claims =
            new[]
            {
                new Claim(
                    ClaimTypes.Name,
                    "Admin"),

                new Claim(
                    ClaimTypes.Role,
                    "Admin")
            };

        var identity =
            new ClaimsIdentity(
                claims,
                "Test");

        var principal =
            new ClaimsPrincipal(
                identity);

        return Task.FromResult(
            AuthenticateResult.Success(
                new AuthenticationTicket(
                    principal,
                    "Test")));
    }
}

#endregion

public class FakeViewComponentHelper
    : IViewComponentHelper
{
    public void Contextualize(
        ViewContext viewContext)
    {
    }

    public Task<IHtmlContent>
        InvokeAsync(
            string name,
            object arguments)
    {
        return Task.FromResult<IHtmlContent>(
            HtmlString.Empty);
    }

    public Task<IHtmlContent>
        InvokeAsync(
            Type componentType,
            object arguments)
    {
        return Task.FromResult<IHtmlContent>(
            HtmlString.Empty);
    }
}

public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    private bool _useAuthentication = true;
    public CustomWebApplicationFactory<TProgram>
    WithoutAuthentication()
    {
        _useAuthentication = false;

        return this;
    }
    protected override void ConfigureWebHost(
        IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(
            services =>
            {
                services.RemoveAll<
                    DbContextOptions<ApplicationDbContext>>();

                services.RemoveAll<
                    ApplicationDbContext>();

                services.RemoveAll<
    IViewComponentHelper>();

                services.AddSingleton<
                    IViewComponentHelper,
                    FakeViewComponentHelper>();

                services.AddDbContext<
                    ApplicationDbContext>(
                    options =>
                    {
                        options.UseInMemoryDatabase(
                            "E2E_TEST_DB");
                    });

                services.RemoveAll<IQuestPDFService>();

                services.AddScoped<IQuestPDFService, FakeQuestPdfService>();

                if (_useAuthentication)
                {
                    services
                        .AddAuthentication(
                            options =>
                            {
                                options.DefaultAuthenticateScheme =
                                    "Test";

                                options.DefaultChallengeScheme =
                                    "Test";
                            })
                        .AddScheme
                        <
                            AuthenticationSchemeOptions,
                            TestAuthHandler
                        >
                        (
                            "Test",
                            _ => { }
                        );

                    services.AddAuthorization();
                }

                services.RemoveAll<
                    IDNTCaptchaValidatorService>();

                services.AddSingleton
                <
                    IDNTCaptchaValidatorService,
                    FakeCaptchaValidatorService
                >();

                var provider =
                    services.BuildServiceProvider();

                using var scope =
                    provider.CreateScope();

                var db =
                    scope.ServiceProvider
                        .GetRequiredService<
                            ApplicationDbContext>();

                db.Database.EnsureDeleted();

                db.Database.EnsureCreated();
            });
    }
}