using Microsoft.AspNetCore.Mvc;

namespace WebSite.EndPoint.Controllers
{
    public class HomeController : BaseController
    {
        [Route("/")]
        public IActionResult Dashboard()
        {
            return View();
        }
    }
}
