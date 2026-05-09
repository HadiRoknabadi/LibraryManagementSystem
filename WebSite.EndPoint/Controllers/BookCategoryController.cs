using Application.DTOs.BookCategory;
using Application.DTOs.User;
using Application.Services.Implementations;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebSite.EndPoint.Http;

namespace WebSite.EndPoint.Controllers
{
    [Authorize(Roles = "Admin")]
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

        #region Add Book Category

        [Route("AddBookCategory")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddBookCategory(AddBookCategoryDTO addBookCategoryDTO)
        {
            if (ModelState.IsValid)
            {
                var result = await _bookCategoryService.AddBookCategoryAsync(addBookCategoryDTO);

                switch (result.Status)
                {
                    case AddBookCategoryResult.Success:
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

        #region Edit Book Category

        [Route("EditBookCategory")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditBookCategory(EditBookCategoryDTO editBookCategoryDTO)
        {
            if (ModelState.IsValid)
            {
                var result = await _bookCategoryService.EditBookCategoryAsync(editBookCategoryDTO);

                switch (result.Status)
                {
                    case EditBookCategoryResult.Success:
                        return JsonResponseStatus.SendStatus(JsonResponseStatusType.Success, result.Message, null);

                    case EditBookCategoryResult.NotFound:
                        return JsonResponseStatus.SendStatus(JsonResponseStatusType.Warning, result.Message, null);

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

        #region Delete Book Category

        [Route("DeleteBookCategory/{bookCategoryId}")]
        public async Task<IActionResult> DeleteBookCategory(int bookCategoryId)
        {
            var result = await _bookCategoryService.DeleteBookCategoryAsync(bookCategoryId);
            switch (result.Status)
            {
                case DeleteBookCategoryResult.Success:
                    return JsonResponseStatus.SendStatus(JsonResponseStatusType.Success, result.Message, null);

                case DeleteBookCategoryResult.NotFound:
                    return JsonResponseStatus.SendStatus(JsonResponseStatusType.Warning, result.Message, null);

                default:
                    return JsonResponseStatus.SendStatus(JsonResponseStatusType.Error, "عملیات مورد نظر با خطا مواجه شد", null);


            }

        }

        #endregion


    }
}
