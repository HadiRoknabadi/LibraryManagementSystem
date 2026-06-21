using Application.DTOs.Account;
using Application.DTOs.User;
using Application.Services.Implementations;
using AutoFixture.Xunit2;
using AutoMapper;
using Domain.Entities.Account;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Persistence.Context;
using Xunit;

public class UserServiceTests
{
    #region Helpers

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
            cfg.CreateMap<User, UserListItemDTO>();
            cfg.CreateMap<User, UserDetailsDTO>();

            cfg.CreateMap<AddUserDTO, User>();
            cfg.CreateMap<EditUserDTO, User>();

            cfg.CreateMap<User, EditUserDTO>();
        }, loggerFactory);

        return config.CreateMapper();
    }

    private static Mock<UserManager<User>> GetUserManagerMock(IQueryable<User> users)
    {
        var store = new Mock<IUserStore<User>>();

        var userManagerMock = new Mock<UserManager<User>>(
            store.Object,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null);

        userManagerMock.Setup(x => x.Users).Returns(users);

        return userManagerMock;
    }

    private static Mock<UserManager<User>> GetUserManagerMock()
    {
        var store = new Mock<IUserStore<User>>();

        return new Mock<UserManager<User>>(
            store.Object,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null);
    }

    private static Mock<SignInManager<User>> GetSignInManagerMock(UserManager<User> userManager)
    {
        var httpContextAccessor = new Mock<IHttpContextAccessor>();
        var claimsFactory = new Mock<IUserClaimsPrincipalFactory<User>>();

        return new Mock<SignInManager<User>>(
            userManager,
            httpContextAccessor.Object,
            claimsFactory.Object,
            null,
            null,
            null,
            null);
    }

    private static UserService GetService(
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        IMapper mapper)
    {
        return new UserService(userManager, signInManager, mapper);
    }

    private static void CleanUser(User user, string? phoneNumber = null)
    {
        user.Id = 0;

        user.IsDelete = false;

        user.Name = string.IsNullOrWhiteSpace(user.Name)
            ? $"Name-{Guid.NewGuid():N}"
            : user.Name;

        user.Family = string.IsNullOrWhiteSpace(user.Family)
            ? $"Family-{Guid.NewGuid():N}"
            : user.Family;

        user.PhoneNumber = phoneNumber ?? GeneratePhoneNumber();

        user.UserName = user.PhoneNumber;
        user.NormalizedUserName = user.UserName.ToUpper();

        user.Email = string.IsNullOrWhiteSpace(user.Email)
            ? $"{Guid.NewGuid():N}@test.com"
            : user.Email;

        user.NormalizedEmail = user.Email.ToUpper();

        user.UserAvatar = string.IsNullOrWhiteSpace(user.UserAvatar)
            ? "default.png"
            : user.UserAvatar;

        user.PasswordHash = "PasswordHash";

        user.SecurityStamp = Guid.NewGuid().ToString();
        user.ConcurrencyStamp = Guid.NewGuid().ToString();

        user.PhoneNumberConfirmed = true;
        user.EmailConfirmed = true;
        user.LockoutEnabled = false;
        user.AccessFailedCount = 0;

        user.UserRoles = new List<UserRole>();
    }

    private static string GeneratePhoneNumber()
    {
        return $"09{Random.Shared.Next(100000000, 999999999)}";
    }

    #endregion

    #region GetAllUsersAsync

    [Theory]
    [AutoDomainData]
    public async Task GetAllUsersAsync_ShouldReturnUsers(List<User> users)
    {
        // Arrange
        var context = GetDbContext();
        var mapper = GetMapper();

        for (var i = 0; i < users.Count; i++)
        {
            CleanUser(users[i], $"09123456{i:D3}");
        }

        await context.Users.AddRangeAsync(users);
        await context.SaveChangesAsync();

        var userManagerMock = GetUserManagerMock(context.Users);
        var signInManagerMock = GetSignInManagerMock(userManagerMock.Object);

        var service = GetService(
            userManagerMock.Object,
            signInManagerMock.Object,
            mapper);

        // Act
        var result = await service.GetAllUsersAsync();

        // Assert
        result.Should().NotBeNull();
        result.Status.Should().Be(GetAllUsersResult.Success);
        result.Data.Should().NotBeNull();
        result.Data.Count.Should().Be(users.Count);
    }

    [Fact]
    public async Task GetAllUsersAsync_ShouldReturnEmptyList_WhenThereIsNoUser()
    {
        // Arrange
        var context = GetDbContext();
        var mapper = GetMapper();

        var userManagerMock = GetUserManagerMock(context.Users);
        var signInManagerMock = GetSignInManagerMock(userManagerMock.Object);

        var service = GetService(
            userManagerMock.Object,
            signInManagerMock.Object,
            mapper);

        // Act
        var result = await service.GetAllUsersAsync();

        // Assert
        result.Should().NotBeNull();
        result.Status.Should().Be(GetAllUsersResult.Success);
        result.Data.Should().NotBeNull();
        result.Data.Count.Should().Be(0);
    }

    #endregion

    #region GetUserDetailsAsync

    [Theory]
    [AutoDomainData]
    public async Task GetUserDetailsAsync_ShouldReturnUser(User user)
    {
        // Arrange
        var mapper = GetMapper();

        CleanUser(user);
        user.Id = 1;

        var userManagerMock = GetUserManagerMock();

        userManagerMock
            .Setup(x => x.FindByIdAsync(user.Id.ToString()))
            .ReturnsAsync(user);

        var signInManagerMock = GetSignInManagerMock(userManagerMock.Object);

        var service = GetService(
            userManagerMock.Object,
            signInManagerMock.Object,
            mapper);

        // Act
        var result = await service.GetUserDetailsAsync(user.Id);

        // Assert
        result.Should().NotBeNull();
        result.Status.Should().Be(GetUserDetailsResult.Success);
        result.Data.Should().NotBeNull();
    }

    [Fact]
    public async Task GetUserDetailsAsync_ShouldReturnUserNotFound_WhenUserDoesNotExist()
    {
        // Arrange
        var mapper = GetMapper();

        var userManagerMock = GetUserManagerMock();

        userManagerMock
            .Setup(x => x.FindByIdAsync(It.IsAny<string>()))
            .ReturnsAsync((User)null);

        var signInManagerMock = GetSignInManagerMock(userManagerMock.Object);

        var service = GetService(
            userManagerMock.Object,
            signInManagerMock.Object,
            mapper);

        // Act
        var result = await service.GetUserDetailsAsync(1);

        // Assert
        result.Should().NotBeNull();
        result.Status.Should().Be(GetUserDetailsResult.UserNotFound);
        result.Data.Should().BeNull();
    }

    #endregion

    #region IsExistPhoneNumberAsync

    [Theory]
    [AutoDomainData]
    public async Task IsExistPhoneNumberAsync_ShouldReturnTrue(User user)
    {
        // Arrange
        var context = GetDbContext();
        var mapper = GetMapper();

        const string phoneNumber = "09123456789";

        CleanUser(user, phoneNumber);

        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();

        var userManagerMock = GetUserManagerMock(context.Users);
        var signInManagerMock = GetSignInManagerMock(userManagerMock.Object);

        var service = GetService(
            userManagerMock.Object,
            signInManagerMock.Object,
            mapper);

        // Act
        var result = await service.IsExistPhoneNumberAsync(phoneNumber);

        // Assert
        result.Should().BeTrue();
    }

    [Theory]
    [AutoDomainData]
    public async Task IsExistPhoneNumberAsync_ShouldReturnFalse_WhenPhoneNumberDoesNotExist(User user)
    {
        // Arrange
        var context = GetDbContext();
        var mapper = GetMapper();

        CleanUser(user, "09111111111");

        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();

        var userManagerMock = GetUserManagerMock(context.Users);
        var signInManagerMock = GetSignInManagerMock(userManagerMock.Object);

        var service = GetService(
            userManagerMock.Object,
            signInManagerMock.Object,
            mapper);

        // Act
        var result = await service.IsExistPhoneNumberAsync("09999999999");

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task IsExistPhoneNumberAsync_ShouldReturnFalse_WhenThereIsNoUser()
    {
        // Arrange
        var context = GetDbContext();
        var mapper = GetMapper();

        var userManagerMock = GetUserManagerMock(context.Users);
        var signInManagerMock = GetSignInManagerMock(userManagerMock.Object);

        var service = GetService(
            userManagerMock.Object,
            signInManagerMock.Object,
            mapper);

        // Act
        var result = await service.IsExistPhoneNumberAsync("09123456789");

        // Assert
        result.Should().BeFalse();
    }

    #endregion

    #region DeleteUserAsync

    [Theory]
    [AutoDomainData]
    public async Task DeleteUserAsync_ShouldDeleteUser(User user)
    {
        // Arrange
        var mapper = GetMapper();

        CleanUser(user);
        user.Id = 1;

        var userManagerMock = GetUserManagerMock();

        userManagerMock
            .Setup(x => x.FindByIdAsync(user.Id.ToString()))
            .ReturnsAsync(user);

        userManagerMock
            .Setup(x => x.UpdateAsync(user))
            .ReturnsAsync(IdentityResult.Success);

        var signInManagerMock = GetSignInManagerMock(userManagerMock.Object);

        var service = GetService(
            userManagerMock.Object,
            signInManagerMock.Object,
            mapper);

        // Act
        var result = await service.DeleteUserAsync(user.Id);

        // Assert
        result.Should().NotBeNull();
        result.Status.Should().Be(DeleteUserResult.Success);

        user.IsDelete.Should().BeTrue();

        userManagerMock.Verify(
            x => x.FindByIdAsync(user.Id.ToString()),
            Times.Once);

        userManagerMock.Verify(
            x => x.UpdateAsync(user),
            Times.Once);
    }

    [Fact]
    public async Task DeleteUserAsync_ShouldReturnUserNotFound_WhenUserDoesNotExist()
    {
        // Arrange
        var mapper = GetMapper();

        var userManagerMock = GetUserManagerMock();

        userManagerMock
            .Setup(x => x.FindByIdAsync(It.IsAny<string>()))
            .ReturnsAsync((User)null);

        var signInManagerMock = GetSignInManagerMock(userManagerMock.Object);

        var service = GetService(
            userManagerMock.Object,
            signInManagerMock.Object,
            mapper);

        // Act
        var result = await service.DeleteUserAsync(1);

        // Assert
        result.Should().NotBeNull();
        result.Status.Should().Be(DeleteUserResult.NotFound);

        userManagerMock.Verify(
            x => x.UpdateAsync(It.IsAny<User>()),
            Times.Never);
    }

    #endregion

    #region LoginUserAsync

    [Theory]
    [AutoDomainData]
    public async Task LoginUserAsync_ShouldLoginSuccessfully(User user, LoginUserDTO dto)
    {
        // Arrange
        var mapper = GetMapper();

        dto.PhoneNumber = "09123456789";
        dto.Password = "123456";
        dto.RememberMe = true;

        CleanUser(user, dto.PhoneNumber);
        user.Id = 1;

        var userManagerMock = GetUserManagerMock();

        userManagerMock
            .Setup(x => x.FindByNameAsync(dto.PhoneNumber))
            .ReturnsAsync(user);

        var signInManagerMock = GetSignInManagerMock(userManagerMock.Object);

        signInManagerMock
            .Setup(x => x.PasswordSignInAsync(
                user,
                dto.Password,
                dto.RememberMe,
                false))
            .ReturnsAsync(SignInResult.Success);

        var service = GetService(
            userManagerMock.Object,
            signInManagerMock.Object,
            mapper);

        // Act
        var result = await service.LoginUserAsync(dto);

        // Assert
        result.Should().NotBeNull();
        result.Status.Should().Be(LoginUserResult.Success);

        userManagerMock.Verify(
            x => x.FindByNameAsync(dto.PhoneNumber),
            Times.Once);

        signInManagerMock.Verify(
            x => x.PasswordSignInAsync(
                user,
                dto.Password,
                dto.RememberMe,
                false),
            Times.Once);
    }

    [Theory]
    [AutoDomainData]
    public async Task LoginUserAsync_ShouldReturnUserNotFound_WhenUserDoesNotExist(LoginUserDTO dto)
    {
        // Arrange
        var mapper = GetMapper();

        dto.PhoneNumber = "09123456789";
        dto.Password = "123456";

        var userManagerMock = GetUserManagerMock();

        userManagerMock
            .Setup(x => x.FindByNameAsync(dto.PhoneNumber))
            .ReturnsAsync((User)null);

        var signInManagerMock = GetSignInManagerMock(userManagerMock.Object);

        var service = GetService(
            userManagerMock.Object,
            signInManagerMock.Object,
            mapper);

        // Act
        var result = await service.LoginUserAsync(dto);

        // Assert
        result.Should().NotBeNull();
        result.Status.Should().Be(LoginUserResult.UserNotFound);

        signInManagerMock.Verify(
            x => x.PasswordSignInAsync(
                It.IsAny<User>(),
                It.IsAny<string>(),
                It.IsAny<bool>(),
                It.IsAny<bool>()),
            Times.Never);
    }

    [Theory]
    [AutoDomainData]
    public async Task LoginUserAsync_ShouldReturnInvalidPassword_WhenPasswordIsWrong(User user, LoginUserDTO dto)
    {
        // Arrange
        var mapper = GetMapper();

        dto.PhoneNumber = "09123456789";
        dto.Password = "wrong-password";
        dto.RememberMe = false;

        CleanUser(user, dto.PhoneNumber);
        user.Id = 1;

        var userManagerMock = GetUserManagerMock();

        userManagerMock
            .Setup(x => x.FindByNameAsync(dto.PhoneNumber))
            .ReturnsAsync(user);

        var signInManagerMock = GetSignInManagerMock(userManagerMock.Object);

        signInManagerMock
            .Setup(x => x.PasswordSignInAsync(
                user,
                dto.Password,
                dto.RememberMe,
                false))
            .ReturnsAsync(SignInResult.Failed);

        var service = GetService(
            userManagerMock.Object,
            signInManagerMock.Object,
            mapper);

        // Act
        var result = await service.LoginUserAsync(dto);

        // Assert
        result.Should().NotBeNull();
        result.Status.Should().Be(LoginUserResult.IdentityError);
    }

    #endregion
}
