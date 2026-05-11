using Application.DTOs.Publisher;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace WebSite.EndPoint.Controllers
{
    [Authorize(Roles ="Admin")]
    public class PublisherController : BaseController
    {
        #region Constructor

        private readonly IPublisherService _publisherService;

        public PublisherController(IPublisherService publisherService)
        {
            _publisherService = publisherService;
        }

        #endregion

        #region Publishers

        [Route("Publishers")]
        public async Task<IActionResult> Publishers(FilterPublisherDTO filter)
        {
            filter.HowManyShowPageAfterAndBefore = 5;
            filter.TakeEntity = 20;

            var result=await _publisherService.FilterPublisherAsync(filter);

            return View(result);
        }

        #endregion


    }
}
