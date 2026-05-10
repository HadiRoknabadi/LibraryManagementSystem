using Application.DTOs.Author;
using Application.DTOs.BookCategory;
using Application.Services.Implementations;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebSite.EndPoint.Http;

namespace WebSite.EndPoint.Controllers
{
    [Authorize(Roles ="Admin")]
    public class AuthorController : BaseController
    {
        #region Constructor

        private readonly IAuthorService _authorService;

        public AuthorController(IAuthorService authorService)
        {
            _authorService = authorService;
        }

        #endregion

        #region Authors

        [Route("Authors")]
        public async Task<IActionResult> Authors(FilterAuthorDTO filter)
        {
            filter.HowManyShowPageAfterAndBefore = 5;
            filter.TakeEntity = 20;

            var result = await _authorService.FilterAuthorAsync(filter);
            return View(result);
        }

        #endregion

        #region Add Book Category

        [Route("AddAuthor")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddAuthor(AddAuthorDTO addAuthorDTO)
        {
            if (ModelState.IsValid)
            {
                var result = await _authorService.AddAuthorAsync(addAuthorDTO);

                switch (result.Status)
                {
                    case AddAuthorResult.Success:
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
