﻿using LibBiz.Data;
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

        [HttpPut]
        public IActionResult UpdateDoctor(Doctor updatedDoctor)
        {
            Doctor doctor = _BM.UpdateDoctor(updatedDoctor);
            return Ok(doctor);
        }

        [HttpGet]
        public IActionResult GetAllDoctors()
        {
            List<Doctor> doctors = _BM.GetAllDoctors();
            return Ok(doctors);
        }

        [HttpGet]
        public IActionResult GetDoctorById(int id)
        {
            Doctor doctor = _BM.GetDoctorById(id);
            return Ok(doctor);
        }
        [HttpPut]
        public IActionResult UpdatePatient(Patient updatedPatient)
        {
            Patient patient = _BM.UpdatePatient(updatedPatient);
            return Ok(patient);
        }

    }
}