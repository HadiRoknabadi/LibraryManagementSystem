using Application.DTOs.Author;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

    }
}
