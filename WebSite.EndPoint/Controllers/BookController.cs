using Application.DTOs.Book;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebSite.EndPoint.Controllers
{
    [Authorize(Roles ="Admin")]
    public class BookController : BaseController
    {
        #region Constructor

        private readonly IBookService _bookService;

        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }

        #endregion

        #region Books

        [Route("Books")]
        public async Task<IActionResult> Books(FilterBookDTO filter)
        {
            filter.HowManyShowPageAfterAndBefore = 5;
            filter.TakeEntity = 20;

            var result=await _bookService.FilterBookAsync(filter);

            return View(result);
        }

        #endregion
    }
}
