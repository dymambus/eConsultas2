using Microsoft.AspNetCore.Mvc;

namespace UI.Controllers
{
    public class LandingPageController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Error()
        {
            return View();
        }
    }
}
