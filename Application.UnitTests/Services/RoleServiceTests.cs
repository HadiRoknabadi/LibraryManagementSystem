using Application.DTOs.Role;
using Application.Services.Implementations;
using AutoFixture.Xunit2;
using AutoMapper;
using Domain.Entities.Account;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Persistence.Context;
using Xunit;

public class RoleServiceTests
{
    private static ApplicationDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new ApplicationDbContext(options);
    }

    private static IMapper GetMapper()
    {
        var loggerFactory = LoggerFactory.Create(builder => { });

        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Role, RoleListItemDTO>();
        }, loggerFactory);

        return config.CreateMapper();
    }

    private static RoleManager<Role> GetRoleManager(IQueryable<Role> roles)
    {
        var store = new Mock<IRoleStore<Role>>();

        var roleManagerMock = new Mock<RoleManager<Role>>(
            store.Object,
            null,
            null,
            null,
            null);

        roleManagerMock.Setup(r => r.Roles).Returns(roles);

        return roleManagerMock.Object;
    }

    [Theory]
    [AutoDomainData]
    public async Task GetRolesAsync_ShouldReturnRoles(List<Role> roles)
    {
        // Arrange
        var context = GetDbContext();
        var mapper = GetMapper();

        foreach (var role in roles)
        {
            role.Id = 0;
            role.Name ??= Guid.NewGuid().ToString();
            role.NormalizedName = role.Name.ToUpper();
            role.IsDelete = false; // مهم اگر QueryFilter دارید
        }

        await context.Roles.AddRangeAsync(roles);
        await context.SaveChangesAsync();

        var roleManager = GetRoleManager(context.Roles);
        var service = new RoleService(roleManager, mapper);

        // Act
        var result = await service.GetRolesAsync();

        // Assert
        result.Status.Should().Be(GetRolesResult.Success);
        result.Data.Should().NotBeNull();
        result.Data.Count.Should().Be(roles.Count);
    }


    [Fact]
    public async Task GetRolesAsync_ShouldReturnEmptyList_WhenNoRolesExist()
    {
        // Arrange
        var context = GetDbContext();
        var mapper = GetMapper();

        var roleManager = GetRoleManager(context.Roles);

        var service = new RoleService(roleManager, mapper);

        // Act
        var result = await service.GetRolesAsync();

        // Assert
        result.Status.Should().Be(GetRolesResult.Success);
        result.Data.Should().BeEmpty();
    }
}
