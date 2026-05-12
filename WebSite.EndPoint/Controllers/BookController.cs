using Application.DTOs.Author;
using Application.DTOs.Book;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebSite.EndPoint.Http;

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

        #region Edit Book

        [Route("EditBook/{bookId}")]
        public async Task<IActionResult> EditBook(int bookId)
        {
            var result=await _bookService.GetBookDetailsForEditAsync(bookId);

            switch (result.Status)
            {
                case GetBookDetailsResult.Success:

                    #region Fill Select Lists

                    var categories = await _bookCategoryService.GetAllBookCategoriesAsync();

                    var authors = await _authorService.GetAllAuthorsAsync();

                    var publishers = await _publisherService.GetPublishersAsync();

                    ViewData["Categories"] = categories.Data;
                    ViewData["Authors"] = authors.Data;
                    ViewData["Publishers"] = publishers.Data;

                    #endregion

                    return View(result.Data);

                case GetBookDetailsResult.NotFound:
                    TempData[Toast_WarningMessage]=result.Message;
                    return RedirectToAction(nameof(Books));

                default:
                    TempData[Toast_ErrorMessage] = "عملیات با خطا مواجه شد";
                    return RedirectToAction(nameof(Books));
            }
        }

        [Route("EditBook/{bookId?}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditBook(EditBookDTO editBookDTO)
        {
            if(ModelState.IsValid)
            {
                var result=await _bookService.EditBookAsync(editBookDTO);

                switch (result.Status)
                {
                    case EditBookResult.Success:

                        TempData[Toast_SuccessMessage] = result.Message;
                        return RedirectToAction(nameof(Books));

                    case EditBookResult.NotFound:
                        TempData[Toast_WarningMessage] = result.Message;
                        break;


                }
            }

            return View(editBookDTO);
        }

        #endregion

        #region Delete Book

        [Route("DeleteBook/{bookId}")]
        public async Task<IActionResult> DeleteBook(int bookId)
        {
            var result = await _bookService.DeleteBookAsync(bookId);
            switch (result.Status)
            {
                case DeleteBookResult.Success:
                    return JsonResponseStatus.SendStatus(JsonResponseStatusType.Success, result.Message, null);

                case DeleteBookResult.NotFound:
                    return JsonResponseStatus.SendStatus(JsonResponseStatusType.Warning, result.Message, null);

                default:
                    return JsonResponseStatus.SendStatus(JsonResponseStatusType.Error, "عملیات مورد نظر با خطا مواجه شد", null);


            }

        }

        #endregion


    }
}
