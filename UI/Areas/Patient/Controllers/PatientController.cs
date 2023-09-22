using Microsoft.AspNetCore.Mvc;

namespace UI.Areas.Patient.Controllers
{
    public class PatientController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult PatientDashboard()
        {
            return View();
        }
    }
}
