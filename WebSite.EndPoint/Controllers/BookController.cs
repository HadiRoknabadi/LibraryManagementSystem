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
        private readonly IBookCategoryService _bookCategoryService;
        private readonly IAuthorService _authorService;
        private readonly IPublisherService _publisherService;

        public BookController(IBookService bookService, IBookCategoryService bookCategoryService, IAuthorService authorService, IPublisherService publisherService)
        {
            _bookService = bookService;
            _bookCategoryService = bookCategoryService;
            _authorService = authorService;
            _publisherService = publisherService;
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

        #region Add Book

        [Route("AddBook")]
        public async Task<IActionResult> AddBook()
        {
            #region Fill Select Lists

            var categories = await _bookCategoryService.GetAllBookCategoriesAsync();

            var authors=await _authorService.GetAllAuthorsAsync();

            var publishers=await _publisherService.GetPublishersAsync();

            ViewData["Categories"] = categories.Data;
            ViewData["Authors"] = authors.Data;
            ViewData["Publishers"] = publishers.Data;

            #endregion

            return View();
        }

        [Route("AddBook")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddBook(AddBookDTO addBookDTO)
        {
            if(ModelState.IsValid)
            {
                var result=await _bookService.AddBookAsync(addBookDTO);

                switch (result.Status)
                {
                    case AddBookResult.Success:

                        TempData[Toast_SuccessMessage]=result.Message;
                        return RedirectToAction(nameof(Books));
                }
            }

            #region Fill Select Lists

            var categories = await _bookCategoryService.GetAllBookCategoriesAsync();

            var authors = await _authorService.GetAllAuthorsAsync();

            var publishers = await _publisherService.GetPublishersAsync();

            ViewData["Categories"] = categories.Data;
            ViewData["Authors"] = authors.Data;
            ViewData["Publishers"] = publishers.Data;

            #endregion

            return View(addBookDTO);
        }

        #endregion

    }
}
