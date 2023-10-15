using LibBiz.Data;
using LibBiz.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]/[Action]")]
    public class DoctorsController : ControllerBase
    {

        private ILogger<DoctorsController> _logger;
        private IBusinessMethods _BM;

        public DoctorsController(ILogger<DoctorsController> logger, IBusinessMethods BM)
        {
            _logger = logger;
            _BM = BM;
        }

        [HttpGet]
        public IActionResult GetAllSpecialization()
        {
            List<string> doctorsWithSpecialization = _BM.GetAllSpecializations();
            return Ok(doctorsWithSpecialization);
        }

        [HttpPost]
        public IActionResult UpdateDoctorInfo(Doctor updatedDoctor)
        {
            Doctor doctor = _BM.D_Update(updatedDoctor);
            return Ok(doctor);
        }

        [HttpGet]
        public IActionResult GetAllDoctors()
        {
            List<Doctor> doctors = _BM.D_GetAll();
            return Ok(doctors);
        }

        [HttpGet]
        public IActionResult GetDoctorById(int id)
        {
            Doctor doctor = _BM.D_GetById(id);
            return Ok(doctor);
        }
        [HttpPut]
        public IActionResult UpdatePatient(Patient updatedPatient)
        {
            Patient patient = _BM.P_Update(updatedPatient);
            return Ok(patient);
        }

    }
}