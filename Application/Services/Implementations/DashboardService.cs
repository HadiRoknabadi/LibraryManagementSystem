using Application.DTOs.Dashboard;
using Application.Services.Interfaces;
using Application.Services.Interfaces.Context;
using Domain.Entities.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Domain.Entities.Book;

namespace Application.Services.Implementations
{
    public class DashboardService:IDashboardService
    {
        #region Constructor

        private readonly IDatabaseContext _context;
        private readonly UserManager<User> _userManager;

        public DashboardService(IDatabaseContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }



        #endregion

        public async Task<DashboardDataDTO> GetDashboardDataAsync()
        {
            var totalMembers = await _userManager.Users.AsQueryable().AsNoTracking()
                .Where(u => u.UserRoles.Any(s => s.Role.Name == "Member")).CountAsync();

            var totalBooks = await _context.Books.AsQueryable().AsNoTracking().CountAsync();

            var totalBorrowed=await _context.Borrowings.AsQueryable().AsNoTracking()
                .Where(b=>b.Status==BorrowingStatus.Borrowed).CountAsync();

            var totalOverDue=await _context.Borrowings.AsQueryable().AsNoTracking()
                .Where(b => b.Status == BorrowingStatus.Borrowed && DateTime.Now>b.DueDate).CountAsync();


            var dashboardData = new DashboardDataDTO
            {
                TotalMembers= totalMembers,
                TotalBooks= totalBooks,
                TotalBorrowed= totalBorrowed,
                ToalOverDue= totalOverDue
            };

            return dashboardData;

        }


    }
}
