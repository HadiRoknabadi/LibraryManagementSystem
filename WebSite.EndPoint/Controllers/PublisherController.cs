using Application.DTOs.Publisher;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebSite.EndPoint.Http;


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

        #region Add Publisher

        [Route("AddPublisher")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPublisher(AddPublisherDTO addPublisherDTO)
        {
            if (ModelState.IsValid)
            {
                var result = await _publisherService.AddPublisherAsync(addPublisherDTO);

                switch (result.Status)
                {
                    case AddPublisherResult.Success:
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

        #region Edit Publisher

        [Route("EditPublisher")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPublisher(EditPublisherDTO editPublisherDTO)
        {
            if (ModelState.IsValid)
            {
                var result = await _publisherService.EditPublisherAsync(editPublisherDTO);

                switch (result.Status)
                {
                    case EditPublisherResult.Success:
                        return JsonResponseStatus.SendStatus(JsonResponseStatusType.Success, result.Message, null);

                    case EditPublisherResult.NotFound:
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

        #region Delete Publisher

        [Route("DeletePublisher/{publisherId}")]
        public async Task<IActionResult> DeletePublisher(int publisherId)
        {
            var result = await _publisherService.DeletePublisherAsync(publisherId);
            switch (result.Status)
            {
                case DeletePublisherResult.Success:
                    return JsonResponseStatus.SendStatus(JsonResponseStatusType.Success, result.Message, null);

                case DeletePublisherResult.NotFound:
                    return JsonResponseStatus.SendStatus(JsonResponseStatusType.Warning, result.Message, null);

                default:
                    return JsonResponseStatus.SendStatus(JsonResponseStatusType.Error, "عملیات مورد نظر با خطا مواجه شد", null);


            }

        }

        #endregion


    }
}
