using LibBiz.Data;
using LibBiz.Models;
using Microsoft.AspNetCore.Mvc;
using UI.Controllers;

namespace UI.Areas.Doctor.Controllers
{
    public class DoctorController : Controller
    {
        private ILogger<DoctorController> _logger;
        private IBusinessMethods _BM;
        //private JwtService _jwtService;
        public DoctorController(ILogger<DoctorController> logger, IBusinessMethods BM/*, JwtService jwtService*/)
        {
            _BM = BM;
            _logger = logger;
            //_jwtService = jwtService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult DoctorDashboard()
        {
            return View();

        }
        [HttpGet]
        public IActionResult DoctorProfile()
        {
            // Recupere o token JWT do cabeçalho da solicitação
            //var token = HttpContext.Request.Headers["Authorization"].ToString();

            // Decodifique o token JWT para obter informações do usuário
            //var userInfo = _jwtService.DecodeJwtToken(token);

            // Envie as informações do usuário para a visualização
            //ViewData["UserInfo"] = userInfo;

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