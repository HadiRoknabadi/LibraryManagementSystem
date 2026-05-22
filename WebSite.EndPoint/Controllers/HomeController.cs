using Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebSite.EndPoint.Controllers
{
    [Authorize(Roles = "Admin")]
    public class HomeController : BaseController
    {
        #region Constructor

        private readonly IDashboardService _dashboardService;

        public HomeController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        #endregion

        [Route("/")]
        public async Task<IActionResult> Dashboard()
        {
            var dashboardData=await _dashboardService.GetDashboardDataAsync();

            return View(dashboardData);
        }
    }
}
