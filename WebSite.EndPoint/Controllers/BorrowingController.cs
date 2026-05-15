using Application.DTOs.Borrowing;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebSite.EndPoint.Controllers
{
    [Authorize(Roles ="Admin")]
    public class BorrowingController : BaseController
    {
        #region Constructor

        private readonly IBorrowingService _borrowingService;

        public BorrowingController(IBorrowingService borrowingService)
        {
            _borrowingService = borrowingService;
        }

        #endregion

        [Route("Borrowings")]
        public async Task<IActionResult> Borrowings(FilterBorrowingDTO filter)
        {
            filter.HowManyShowPageAfterAndBefore = 5;
            filter.TakeEntity = 20;

            var result = await _borrowingService.FilterBorrowingAsync(filter);
            return View(result);
        }
    }
}
