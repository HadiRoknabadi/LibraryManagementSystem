using Application.Services.Implementations;
using Domain.Entities.Account;
using Domain.Entities.Book;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using Persistence.Context;
using System.Collections;
using Xunit;

namespace Application.Tests.Services;

public class DashboardServiceTests
{
    #region DbContext & UserManager

    private ApplicationDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new ApplicationDbContext(options);
    }

    private UserManager<User> GetUserManager(ApplicationDbContext context)
    {
        var store = new Mock<IUserStore<User>>();

        store.As<IQueryableUserStore<User>>()
            .Setup(x => x.Users)
            .Returns(context.Users);

        return new Mock<UserManager<User>>(
            store.Object,
            null, null, null, null, null, null, null, null)
        {
            CallBase = true
        }.Object;
    }

    #endregion

    #region Helpers (پاکسازی دیتای رندوم AutoFixture)

    private static void SetIsDeleteFalseIfExists(object entity)
    {
        var prop = entity.GetType().GetProperty("IsDelete");

        if (prop is not null &&
            prop.CanWrite &&
            prop.PropertyType == typeof(bool))
        {
            prop.SetValue(entity, false);
        }
    }

    private static void ClearReferenceNavigations(object entity)
    {
        if (entity == null) return;

        var properties = entity.GetType().GetProperties();

        foreach (var property in properties)
        {
            if (!property.CanWrite)
                continue;

            if (property.PropertyType == typeof(string))
                continue;

            if (property.PropertyType.IsValueType)
                continue;

            if (typeof(IEnumerable).IsAssignableFrom(property.PropertyType))
                continue;

            property.SetValue(entity, null);
        }
    }

    private static void CleanUser(User user)
    {
        ClearReferenceNavigations(user);
        SetIsDeleteFalseIfExists(user);

        user.UserRoles = new List<UserRole>();
    }

    private static void CleanRole(Role role, string name)
    {
        ClearReferenceNavigations(role);
        SetIsDeleteFalseIfExists(role);

        role.Name = name;

        var normalized = role.GetType().GetProperty("NormalizedName");
        if (normalized is not null && normalized.CanWrite)
        {
            normalized.SetValue(role, name.ToUpper());
        }

        role.UserRoles = new List<UserRole>();
    }

    private static void CleanBook(Book book)
    {
        ClearReferenceNavigations(book);
        SetIsDeleteFalseIfExists(book);
    }

    private static void CleanBorrowing(Borrowing borrowing)
    {
        ClearReferenceNavigations(borrowing);
        SetIsDeleteFalseIfExists(borrowing);
    }

    #endregion

    // ----------------------------------------------------------

    [Theory]
    [AutoDomainData]
    public async Task GetDashboardDataAsync_ShouldReturnCorrectTotalBooks(
        Book book1,
        Book book2,
        Book deletedBook)
    {
        var context = GetDbContext();

        CleanBook(book1);
        CleanBook(book2);
        CleanBook(deletedBook);

        book1.IsDelete = false;
        book2.IsDelete = false;
        deletedBook.IsDelete = true;

        await context.Books.AddRangeAsync(book1, book2, deletedBook);
        await context.SaveChangesAsync();

        var service = new DashboardService(context, GetUserManager(context));

        var result = await service.GetDashboardDataAsync();

        result.TotalBooks.Should().Be(2);
    }

    // ----------------------------------------------------------

    [Theory]
    [AutoDomainData]
    public async Task GetDashboardDataAsync_ShouldReturnCorrectTotalBorrowed(
        Borrowing b1,
        Borrowing b2,
        Borrowing returnedBorrow)
    {
        var context = GetDbContext();

        CleanBorrowing(b1);
        CleanBorrowing(b2);
        CleanBorrowing(returnedBorrow);

        b1.Status = BorrowingStatus.Borrowed;
        b2.Status = BorrowingStatus.Borrowed;
        returnedBorrow.Status = BorrowingStatus.Returned;

        await context.Borrowings.AddRangeAsync(b1, b2, returnedBorrow);
        await context.SaveChangesAsync();

        var service = new DashboardService(context, GetUserManager(context));

        var result = await service.GetDashboardDataAsync();

        result.TotalBorrowed.Should().Be(2);
    }

    // ----------------------------------------------------------

    [Theory]
    [AutoDomainData]
    public async Task GetDashboardDataAsync_ShouldReturnCorrectTotalOverDue(
        Borrowing overdue,
        Borrowing normal,
        Borrowing returnedBorrow)
    {
        var context = GetDbContext();

        CleanBorrowing(overdue);
        CleanBorrowing(normal);
        CleanBorrowing(returnedBorrow);

        overdue.Status = BorrowingStatus.Borrowed;
        overdue.DueDate = DateTime.Now.AddDays(-1);

        normal.Status = BorrowingStatus.Borrowed;
        normal.DueDate = DateTime.Now.AddDays(3);

        returnedBorrow.Status = BorrowingStatus.Returned;

        await context.Borrowings.AddRangeAsync(
            overdue,
            normal,
            returnedBorrow);

        await context.SaveChangesAsync();

        var service = new DashboardService(context, GetUserManager(context));

        var result = await service.GetDashboardDataAsync();

        result.ToalOverDue.Should().Be(1);
    }

    // ----------------------------------------------------------

    [Theory]
    [AutoDomainData]
    public async Task GetDashboardDataAsync_ShouldReturnCorrectTotalMembers(
        User memberUser,
        User adminUser,
        Role memberRole,
        Role adminRole)
    {
        var context = GetDbContext();

        CleanUser(memberUser);
        CleanUser(adminUser);

        CleanRole(memberRole, "Member");
        CleanRole(adminRole, "Admin");

        await context.Users.AddRangeAsync(memberUser, adminUser);
        await context.Roles.AddRangeAsync(memberRole, adminRole);
        await context.SaveChangesAsync();

        var memberUserRole = new UserRole
        {
            UserId = memberUser.Id,
            RoleId = memberRole.Id,
            User = memberUser,
            Role = memberRole
        };

        var adminUserRole = new UserRole
        {
            UserId = adminUser.Id,
            RoleId = adminRole.Id,
            User = adminUser,
            Role = adminRole
        };

        memberUser.UserRoles = new List<UserRole> { memberUserRole };
        adminUser.UserRoles = new List<UserRole> { adminUserRole };

        await context.UserRoles.AddRangeAsync(
            memberUserRole,
            adminUserRole);

        await context.SaveChangesAsync();

        var service = new DashboardService(context, GetUserManager(context));

        var result = await service.GetDashboardDataAsync();

        result.TotalMembers.Should().Be(1);
    }

    // ----------------------------------------------------------

    [Theory]
    [AutoDomainData]
    public async Task GetDashboardDataAsync_ShouldReturnDashboardData(
        Book book1,
        Book book2,
        Borrowing borrowed,
        Borrowing overdue,
        Borrowing returnedBorrow,
        User memberUser,
        User adminUser,
        Role memberRole,
        Role adminRole)
    {
        var context = GetDbContext();

        // Books
        CleanBook(book1);
        CleanBook(book2);

        await context.Books.AddRangeAsync(book1, book2);

        // Borrowings
        CleanBorrowing(borrowed);
        CleanBorrowing(overdue);
        CleanBorrowing(returnedBorrow);

        borrowed.Status = BorrowingStatus.Borrowed;
        borrowed.DueDate = DateTime.Now.AddDays(2);

        overdue.Status = BorrowingStatus.Borrowed;
        overdue.DueDate = DateTime.Now.AddDays(-1);

        returnedBorrow.Status = BorrowingStatus.Returned;

        await context.Borrowings.AddRangeAsync(
            borrowed,
            overdue,
            returnedBorrow);

        // Users & Roles
        CleanUser(memberUser);
        CleanUser(adminUser);

        CleanRole(memberRole, "Member");
        CleanRole(adminRole, "Admin");

        await context.Users.AddRangeAsync(memberUser, adminUser);
        await context.Roles.AddRangeAsync(memberRole, adminRole);
        await context.SaveChangesAsync();

        var memberUserRole = new UserRole
        {
            UserId = memberUser.Id,
            RoleId = memberRole.Id,
            User = memberUser,
            Role = memberRole
        };

        var adminUserRole = new UserRole
        {
            UserId = adminUser.Id,
            RoleId = adminRole.Id,
            User = adminUser,
            Role = adminRole
        };

        memberUser.UserRoles = new List<UserRole> { memberUserRole };
        adminUser.UserRoles = new List<UserRole> { adminUserRole };

        await context.UserRoles.AddRangeAsync(
            memberUserRole,
            adminUserRole);

        await context.SaveChangesAsync();

        var service = new DashboardService(context, GetUserManager(context));

        var result = await service.GetDashboardDataAsync();

        result.TotalBooks.Should().Be(2);
        result.TotalBorrowed.Should().Be(2);
        result.ToalOverDue.Should().Be(1);
        result.TotalMembers.Should().Be(1);
    }
}
