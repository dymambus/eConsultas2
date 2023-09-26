using LibBiz.Data;
using LibBiz.Models;
using Microsoft.AspNetCore.Mvc;
using NuGet.Common;
using System.Net.Http.Headers;
using System.Net.Http;
using UI.Controllers;

namespace UI.Areas.Doctor.Controllers
{
    public class DoctorController : Controller
    {
        private ILogger<DoctorController> _logger;
        private IBusinessMethods _BM;
        private readonly HttpClient _client;
        public DoctorController(ILogger<DoctorController> logger, IBusinessMethods BM)
        {
            _BM = BM;
            _logger = logger;
            _client = new HttpClient();
            _client.BaseAddress = new Uri("https://localhost:44364/api/");
        }


        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult DoctorDashboard()
        {
            var token = HttpContext.Session.GetString("Token");
            if (token == null)
            {
                return RedirectToAction("Login", "Auth");
            }
            else
            {
                return View();
            }
        }
        [HttpGet]
        public IActionResult DoctorProfile()
        {
            var token = HttpContext.Session.GetString("Token");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            return View();
        }

        [HttpGet]
        public IActionResult DoctorSpecialization()
        {
            return View();

        }
        [HttpGet]
        public IActionResult DoctorClinic()
        {
            return View();

        }
        [HttpGet]
        public IActionResult DoctorFees()
        {
            return View();

        }

        [HttpGet]
        public IActionResult GetAllSpecialization()
        {
            List<string> doctorsWithSpecialization = _BM.GetAllSpecializations();
            return Ok(doctorsWithSpecialization);
        }

        [HttpPut]
        public IActionResult UpdateDoctor(LibBiz.Models.Doctor updatedDoctor)
        {
            LibBiz.Models.Doctor doctor = _BM.UpdateDoctor(updatedDoctor);
            return Ok(doctor);
        }

        [HttpGet]
        public IActionResult GetAllDoctors()
        {
            List<LibBiz.Models.Doctor> doctors = _BM.GetAllDoctors();
            return Ok(doctors);
        }

        [HttpGet]
        public IActionResult GetDoctorById(int id)
        {
            LibBiz.Models.Doctor doctor = _BM.GetDoctorById(id);
            return Ok(doctor);
        }
        [HttpPut]
        public IActionResult UpdatePatient(LibBiz.Models.Patient updatedPatient)
        {
            LibBiz.Models.Patient patient = _BM.UpdatePatient(updatedPatient);
            return Ok(patient);
        }
    }
}