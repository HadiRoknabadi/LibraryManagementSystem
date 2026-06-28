using System.Net;
using Domain.Entities.Account;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Context;
using WebSite.EndPoint.Tests.E2E.Fixtures;
using Xunit;

namespace WebSite.EndPoint.E2ETests.Controllers;

public class UserControllerTests
    : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory<Program> _factory;

    public UserControllerTests(
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

    private async Task<int> SeedAdminRole()
    {
        using var scope =
            _factory.Services.CreateScope();

        var db =
            scope.ServiceProvider
                .GetRequiredService<ApplicationDbContext>();

        var existing =
            db.Roles
                .FirstOrDefault(r =>
                    r.Name == "Admin");

        if (existing != null)
            return existing.Id;

        var role =
            new Domain.Entities.Account.Role
            {
                Name = "Admin",
                NormalizedName = "ADMIN"
            };

        db.Roles.Add(role);

        await db.SaveChangesAsync();

        return role.Id;
    }
    private async Task<int> SeedUser()
    {
        using var scope =
            _factory.Services.CreateScope();

        var userManager =
            scope.ServiceProvider
                .GetRequiredService<
                    UserManager<User>>();

        var existing =
            await userManager
                .FindByNameAsync(
                    "09111111111");

        if (existing != null)
            return existing.Id;

        var user =
            new User
            {
                UserName =
                    "09111111111",

                PhoneNumber =
                    "09111111111",

                PhoneNumberConfirmed =
                    true,

                Name =
                    "Ali",

                Family =
                    "Test",

                CreateDate =
                    DateTime.Now
            };

        await userManager
            .CreateAsync(
                user,
                "Password123!");

        return user.Id;
    }

    [Fact]
    public async Task Users_ShouldReturnOk()
    {
        await ResetDatabase();

        var response =
            await _client.GetAsync(
                "/Users");

        response.StatusCode
            .Should()
            .Be(
                HttpStatusCode.OK);
    }

    [Fact]
    public async Task AddUser_Get_ShouldReturnOk()
    {
        await ResetDatabase();

        var response =
            await _client.GetAsync(
                "/AddUser");

        response.StatusCode
            .Should()
            .Be(
                HttpStatusCode.OK);
    }

    [Fact]
    public async Task AddUser_Post_WithValidData_ShouldRedirect()
    {
        await ResetDatabase();

        await SeedAdminRole();

        var form =
            new Dictionary<string, string>
            {
                ["Name"] = "Ali",
                ["Family"] = "Ahmadi",
                ["PhoneNumber"] = "09121234567",
                ["Password"] = "Test12345",

                // مهم
                ["RoleName"] = "Admin"
            };

        var response =
            await _client.PostAsync(
                "/AddUser",
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
                "/Users");
    }

    [Fact]
    public async Task AddUser_Post_WithInvalidData_ShouldReturnOk()
    {
        await ResetDatabase();

        var response =
            await _client.PostAsync(
                "/AddUser",
                new FormUrlEncodedContent(
                    new Dictionary<string, string>()));

        response.StatusCode
            .Should()
            .Be(
                HttpStatusCode.OK);
    }

    [Fact]
    public async Task EditUser_Get_WithValidId_ShouldReturnOk()
    {
        await ResetDatabase();

        var id =
            await SeedUser();

        var response =
            await _client.GetAsync(
                $"/EditUser/{id}");

        response.StatusCode
            .Should()
            .Be(
                HttpStatusCode.OK);
    }

    [Fact]
    public async Task EditUser_Post_WithValidData_ShouldRedirect()
    {
        await ResetDatabase();

        using var scope =
            _factory.Services.CreateScope();

        var services =
            scope.ServiceProvider;

        var roleManager =
            services.GetRequiredService<RoleManager<Role>>();

        var userManager =
            services.GetRequiredService<UserManager<User>>();

        // Seed Role
        var role =
            await roleManager.FindByNameAsync("Admin");

        if (role == null)
        {
            role = new Role
            {
                Name = "Admin",
                NormalizedName = "ADMIN"
            };

            await roleManager.CreateAsync(role);
        }

        // Seed User
        var user =
            new User
            {
                Name = "Ali",
                Family = "Test",
                PhoneNumber = "09120001111",
                UserName = "09120001111",
                CreateDate = DateTime.Now
            };

        await userManager.CreateAsync(
            user,
            "Test123@");

        await userManager.AddToRoleAsync(
            user,
            "Admin");

        var form =
            new Dictionary<string, string>
            {
                ["Id"] = user.Id.ToString(),

                ["Name"] = "Edited",
                ["Family"] = "User",

                ["PhoneNumber"] = "09120001111",

                ["RoleName"] = "Admin",

                ["Password"] = ""
            };

        var response =
            await _client.PostAsync(
                $"/EditUser/{user.Id}",
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
                "Users");
    }

    [Fact]
    public async Task DeleteUser_WithValidId_ShouldReturnSuccess()
    {
        await ResetDatabase();

        var id =
            await SeedUser();

        var response =
            await _client.GetAsync(
                $"/DeleteUser/{id}");

        var body =
            await response.Content
                .ReadAsStringAsync();

        body.Should()
            .Contain(
                "Success");
    }

    [Fact]
    public async Task DeleteUser_WithWrongId_ShouldReturnWarning()
    {
        await ResetDatabase();

        var response =
            await _client.GetAsync(
                "/DeleteUser/999999");

        var body =
            await response.Content
                .ReadAsStringAsync();

        body.Should()
            .Contain(
                "Warning");
    }
}