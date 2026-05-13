using Application.DTOs.BookCopy;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebSite.EndPoint.Http;

namespace WebSite.EndPoint.Controllers
{
    [Authorize(Roles ="Admin")]
    public class BookCopyController : BaseController
    {
        #region Constructor

        private readonly IBookCopyService _bookCopyService;
        private readonly IBookService _bookService;

        public BookCopyController(IBookCopyService bookCopyService, IBookService bookService)
        {
            _bookCopyService = bookCopyService;
            _bookService = bookService;
        }


        #endregion

        #region Book Copies

        [Route("BookCopies")]
        public async Task<IActionResult> BookCopies(FilterBookCopyDTO filter)
        {
            #region Fill Select Lists

            var books = await _bookService.GetAllBooksAsync();


            ViewData["Books"] = books.Data;;

            #endregion

            filter.HowManyShowPageAfterAndBefore = 5;
            filter.TakeEntity = 20;

            var result=await _bookCopyService.FilterBookCopyAsync(filter);

            return View(result);
        }

        #endregion

        #region Add Book Copy

        [Route("AddBookCopy")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddBookCopy(AddBookCopyDTO addBookCopyDTO)
        {
            if (ModelState.IsValid)
            {
                var result = await _bookCopyService.AddBookCopyAsync(addBookCopyDTO);

                switch (result.Status)
                {
                    case AddBookCopyResult.Success:
                        return JsonResponseStatus.SendStatus(JsonResponseStatusType.Success, result.Message, null);

                    default:
                        return JsonResponseStatus.SendStatus(JsonResponseStatusType.Error, "عملیات با خطا مواجه شد", null);
                }


            }

            var errors = string.Join(" | ", ModelState.Values
           .SelectMany(v => v.Errors)
           .Select(e => e.ErrorMessage));
            return JsonResponseStatus.SendStatus(JsonResponseStatusType.Error, errors, null);

        }

        #endregion


    }
}
