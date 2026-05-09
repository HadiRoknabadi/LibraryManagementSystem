using Application.DTOs.BookCategory;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebSite.EndPoint.Controllers
{
    [Authorize(Roles ="Admin")]
    public class BookCategoryController : BaseController
    {
        #region Constructor

        private readonly IBookCategoryService _bookCategoryService;

        public BookCategoryController(IBookCategoryService bookCategoryService)
        {
            _bookCategoryService = bookCategoryService;
        }

        #endregion

        #region Book Categories

        [Route("BookCategories")]
        public async Task<IActionResult> BookCategories(FilterBookCategoryDTO filter)
        {
            filter.HowManyShowPageAfterAndBefore = 5;
            filter.TakeEntity = 20;
            var result = await _bookCategoryService.FilterBookCategoryAsync(filter);

            return View(result);
        }

        #endregion
    }
}
