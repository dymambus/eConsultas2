using Microsoft.AspNetCore.Mvc;

namespace UI.Areas.Patient.Controllers
{
    public class PatientController : Controller
    {
        private ILogger<PatientController> _logger;

        public PatientController(ILogger<PatientController> logger)
        {
            _logger = logger;
        }

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
