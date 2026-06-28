using Application.DTOs.Borrowing;
using Application.Services.Interfaces;
using Domain.Entities.Book;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebSite.EndPoint.Http;
using WebSite.EndPoint.PresentationExtensions;

namespace WebSite.EndPoint.Controllers
{
    [Authorize(Roles = "Admin")]
    public class BorrowingController : BaseController
    {
        #region Constructor

        private readonly IBorrowingService _borrowingService;
        private readonly IBookCopyService _bookCopyService;
        private readonly IUserService _userService;
        private IQuestPDFService _questPDFService;

        public BorrowingController(IBorrowingService borrowingService, IBookCopyService bookCopyService, IUserService userService, IQuestPDFService questPDFService)
        {
            _borrowingService = borrowingService;
            _bookCopyService = bookCopyService;
            _userService = userService;
            _questPDFService = questPDFService;
        }



        #endregion

        #region Borrowings

        [Route("Borrowings")]
        public async Task<IActionResult> Borrowings(FilterBorrowingDTO filter)
        {
            #region Fill Select Lists

            var users = await _userService.GetAllUsersAsync();

            var bookCopies = await _bookCopyService.GetAllBookCopiesAsync();


            ViewData["BookCopies"] = bookCopies.Data;
            ViewData["Users"] = users.Data;

            #endregion

            filter.HowManyShowPageAfterAndBefore = 5;
            filter.TakeEntity = 20;

            var result = await _borrowingService.FilterBorrowingAsync(filter);
            return View(result);
        }

        #endregion

        #region Borrow Details

        [Route("BorrowDetails/{id}")]
        public async Task<IActionResult> BorrowDetails(int id)
        {
            var result = await _borrowingService.GetBorrowDetailsAsync(id);

            switch (result.Status)
            {
                case GetBorrowDetailsResult.Success:
                    return View(result.Data);

                case GetBorrowDetailsResult.NotFound:
                    TempData[Toast_WarningMessage] = result.Message;
                    return RedirectToAction(nameof(Borrowings));

                default:
                    return RedirectToAction(nameof(Borrowings));

            }
        }

        #endregion

        #region Submit Borrow

        [Route("SubmitBorrow")]
        [HttpPost]
        public async Task<IActionResult> SubmitBorrow(SubmitBorrowDTO submitBorrowDTO)
        {
            if (ModelState.IsValid)
            {
                var result = await _borrowingService.SubmitBorrowAsync(User.GetUserId(), submitBorrowDTO);

                switch (result.Status)
                {
                    case SubmitBorrowResult.Success:
                        return JsonResponseStatus.SendStatus(JsonResponseStatusType.Success, result.Message, null);

                    case SubmitBorrowResult.DueDatePassedFromNow:
                        return JsonResponseStatus.SendStatus(JsonResponseStatusType.Error, result.Message, null);

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

        #region Edit Borrow

        [Route("EditBorrow")]
        [HttpPost]
        public async Task<IActionResult> EditBorrow(EditBorrowDTO editBorrowDTO)
        {
            if (ModelState.IsValid)
            {
                var result = await _borrowingService.EditBorrowAsync(editBorrowDTO);

                switch (result.Status)
                {
                    case EditBorrowResult.Success:
                        return JsonResponseStatus.SendStatus(JsonResponseStatusType.Success, result.Message, null);

                    case EditBorrowResult.NotFound:
                        return JsonResponseStatus.SendStatus(JsonResponseStatusType.Warning, result.Message, null);


                    case EditBorrowResult.DueDatePassedFromNow:
                        return JsonResponseStatus.SendStatus(JsonResponseStatusType.Error, result.Message, null);

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

        #region Return Borrow

        [Route("ReturnBorrow/{Id}")]
        public async Task<IActionResult> ReturnBorrow(int Id)
        {
            var result = await _borrowingService.ReturnBorrowAsync(Id);
            switch (result.Status)
            {
                case ReturnBorrowResult.Success:
                    return JsonResponseStatus.SendStatus(JsonResponseStatusType.Success, result.Message, null);

                case ReturnBorrowResult.NotFound:
                    return JsonResponseStatus.SendStatus(JsonResponseStatusType.Warning, result.Message, null);

                default:
                    return JsonResponseStatus.SendStatus(JsonResponseStatusType.Error, "عملیات مورد نظر با خطا مواجه شد", null);


            }
        }


        #endregion

        #region Delete Borrow

        [Route("DeleteBorrow/{Id}")]
        public async Task<IActionResult> DeleteBorrow(int Id)
        {
            var result = await _borrowingService.DeleteBorrowAsync(Id);
            switch (result.Status)
            {
                case DeleteBorrowResult.Success:
                    return JsonResponseStatus.SendStatus(JsonResponseStatusType.Success, result.Message, null);

                case DeleteBorrowResult.NotFound:
                    return JsonResponseStatus.SendStatus(JsonResponseStatusType.Warning, result.Message, null);

                default:
                    return JsonResponseStatus.SendStatus(JsonResponseStatusType.Error, "عملیات مورد نظر با خطا مواجه شد", null);


            }

        }

        #endregion

        #region Report

        [Route("GetBorrowingsReport")]
        public async Task<IActionResult> ExportToPdf()
        {
            var result=await _borrowingService.GetBorrowingsForReportAsync();

            switch (result.Status)
            {
                case GetBorrowingsResult.Success:
                    var pdfBytes = await _questPDFService.GenerateBorrowingsReportAsync(result.Data);
                    return File(pdfBytes, "application/pdf", $"borrowings-{DateTime.Now:yyyyMMddHHmmss}.pdf");


                case GetBorrowingsResult.BorrowingsEmpty:
                    TempData[Toast_WarningMessage] = result.Message;
                    break;

                default:
                    TempData[Toast_ErrorMessage] = "عملیات با خطا مواجه شد";
                    break;
            }

            return RedirectToAction(nameof(Borrowings));

        }


        #endregion

    }
}

