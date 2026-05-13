using Application.DTOs.BookCopy;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebSite.EndPoint.Controllers
{
    [Authorize(Roles ="Admin")]
    public class BookCopyController : BaseController
    {
        #region Constructor

        private readonly IBookCopyService _bookCopyService;

        public BookCopyController(IBookCopyService bookCopyService)
        {
            _bookCopyService = bookCopyService;
        }

        #endregion

        #region Book Copies

        [Route("BookCopies")]
        public async Task<IActionResult> BookCopies(FilterBookCopyDTO filter)
        {
            filter.HowManyShowPageAfterAndBefore = 5;
            filter.TakeEntity = 20;

            var result=await _bookCopyService.FilterBookCopyAsync(filter);

            return View(result);
        }

        #endregion
    }
}
