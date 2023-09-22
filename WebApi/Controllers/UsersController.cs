using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LibBiz.Data;
using LibBiz.Models;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly Gateway _ddLayer;
        private readonly ILogger<UsersController> _logger;

        public UsersController(Gateway ddlayer, ILogger<UsersController> logger)
        {
            _ddLayer = ddlayer;
            _logger = logger;
        }

        [Authorize]
        [HttpGet]
        [Route("GetUsers")]
        public IActionResult GetUsers()
        {
            return Ok(_ddLayer.GetUsers());
        }

        [HttpPost]
        [Route("CreateDoctor")]
        public IActionResult CreateDoctor(Doctor doctor)
        {
            if (ModelState.IsValid)
            {
                doctor.RoleId = 1;

                _logger.LogCritical("Doctor created.");
                return Ok(_ddLayer.CreateUser(doctor));
            }
            else
                return NotFound();
        }

        [HttpPost]
        [Route("CreateDoctor_Parms")]
        public void CreateDoctor(string email, string pwd)
        {
            Doctor doctor = new Doctor();
            doctor.Email = email;
            doctor.Password = pwd;
            
            CreateDoctor(doctor);
        }


        [HttpPost]
        [Route("CreatePatient")]
        public IActionResult CreatePatient(Patient patient)
        {
            if (ModelState.IsValid)
            {
                patient.RoleId = 0;

                return Ok(_ddLayer.CreateUser(patient));
            }
            else
                return NotFound();
        }

        [HttpPost]
        [Route("CreatePatient_Parms")]
        public void CreatePatient(string user, string pwd)
        {
            Patient patient = new Patient();
            patient.Email = user;
            patient.Password = pwd;

            CreatePatient(patient);
        }


        [Authorize]
        [HttpDelete]
        public IActionResult DeleteUser(int id)
        {
            _ddLayer.DeleteUser(id);

            return Ok();
        }

    }
}
