using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebSite.EndPoint.Controllers
{
    [Authorize(Roles = "Admin")]
    public class HomeController : BaseController
    {
        [Route("/")]
        public IActionResult Dashboard()
        {
            return View();
        }
    }
}
