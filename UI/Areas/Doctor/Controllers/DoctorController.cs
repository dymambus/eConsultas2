using LibBiz.Data;
using LibBiz.Models;
using Microsoft.AspNetCore.Mvc;
using NuGet.Common;
using System.Net.Http.Headers;
using System.Net.Http;
using UI.Controllers;
using System.Net;
using UI.Models;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;

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
                var userEmail = HttpContext.Session.GetString("Email");

                // Consulte o banco de dados para obter as informações do médico com base no email
                var doctor = _BM.GetDoctorByEmail(userEmail);

                if (doctor != null)
                {
                    var user = new LibBiz.Models.Doctor()
                    {
                        // Preencha as propriedades de DoctorInfoViewModel com os dados do médico
                        Name = doctor.Name,
                        Email = userEmail,
                        Phone = doctor.Phone,
                        Address = doctor.Address,
                        Region = doctor.Region,
                        City = doctor.City,
                        SpecializationName = doctor.SpecializationName,
                        Price = (int)doctor.Price
                    };

                    // Renderize a página DoctorProfile com as informações do médico
                    return View(user); // Certifique-se de que está direcionando para a ação correta
                }
                else
                {
                    return RedirectToAction("Error", "Home");
                }
            }
        }
        [HttpGet]
        public IActionResult Profile()
        {
            var token = HttpContext.Session.GetString("Token");
            if (token == null)
            {
                return RedirectToAction("Login", "Auth");
            }
            else
            {
                // Obtenha o email do médico logado (você pode armazená-lo na sessão ou de outra forma)
                var userEmail = HttpContext.Session.GetString("Email");

                // Consulte o banco de dados para obter as informações do médico com base no email
                var doctor = _BM.GetDoctorByEmail(userEmail);

                if (doctor != null)
                {
                    var user = new LibBiz.Models.Doctor()
                    {
                        // Preencha as propriedades de DoctorInfoViewModel com os dados do médico
                        Name = doctor.Name,
                        Email = userEmail,
                        Phone = doctor.Phone,
                        Address = doctor.Address,
                        Region = doctor.Region,
                        City = doctor.City,
                        SpecializationName = doctor.SpecializationName,
                        Price = (int)doctor.Price
                    };

                    // Renderize a página DoctorProfile com as informações do médico
                    return View(user);
                }
                else
                {
                    // O médico não foi encontrado no banco de dados
                    return RedirectToAction("Error", "Home");
                }
            }
        }
        [HttpGet]
        public IActionResult DoctorSpecialization()
        {
            var token = HttpContext.Session.GetString("Token");
            if (token == null)
            {
                return RedirectToAction("Login", "Auth");
            }
            else
            {
                var userEmail = HttpContext.Session.GetString("Email");

                // Consulte o banco de dados para obter as informações do médico com base no email
                var doctor = _BM.GetDoctorByEmail(userEmail);

                if (doctor != null)
                {
                    var user = new LibBiz.Models.Doctor()
                    {
                        // Preencha as propriedades de DoctorInfoViewModel com os dados do médico
                        Name = doctor.Name,
                        Email = userEmail,
                        Phone = doctor.Phone,
                        Address = doctor.Address,
                        Region = doctor.Region,
                        City = doctor.City,
                        SpecializationName = doctor.SpecializationName,
                        Price = (int)doctor.Price
                    };

                    // Renderize a página DoctorProfile com as informações do médico
                    return View(user); // Certifique-se de que está direcionando para a ação correta
                }
                else
                {
                    return RedirectToAction("Error", "Home");
                }
            }

        }
        [HttpGet]
        public IActionResult DoctorClinic()
        {
            var token = HttpContext.Session.GetString("Token");
            if (token == null)
            {
                return RedirectToAction("Login", "Auth");
            }
            else
            {
                var userEmail = HttpContext.Session.GetString("Email");

                // Consulte o banco de dados para obter as informações do médico com base no email
                var doctor = _BM.GetDoctorByEmail(userEmail);

                if (doctor != null)
                {
                    var user = new LibBiz.Models.Doctor()
                    {
                        // Preencha as propriedades de DoctorInfoViewModel com os dados do médico
                        Name = doctor.Name,
                        Email = userEmail,
                        Phone = doctor.Phone,
                        Address = doctor.Address,
                        Region = doctor.Region,
                        City = doctor.City,
                        SpecializationName = doctor.SpecializationName,
                        Price = (int)doctor.Price
                    };

                    // Renderize a página DoctorProfile com as informações do médico
                    return View(user); // Certifique-se de que está direcionando para a ação correta
                }
                else
                {
                    return RedirectToAction("Error", "Home");
                }
            }

        }
        [HttpGet]
        public IActionResult DoctorFees()
        {
            var token = HttpContext.Session.GetString("Token");
            if (token == null)
            {
                return RedirectToAction("Login", "Auth");
            }
            else
            {
                var userEmail = HttpContext.Session.GetString("Email");

                // Consulte o banco de dados para obter as informações do médico com base no email
                var doctor = _BM.GetDoctorByEmail(userEmail);

                if (doctor != null)
                {
                    var user = new LibBiz.Models.Doctor()
                    {
                        // Preencha as propriedades de DoctorInfoViewModel com os dados do médico
                        Name = doctor.Name,
                        Email = userEmail,
                        Phone = doctor.Phone,
                        Address = doctor.Address,
                        Region = doctor.Region,
                        City = doctor.City,
                        SpecializationName = doctor.SpecializationName,
                        Price = (int)doctor.Price
                    };

                    // Renderize a página DoctorProfile com as informações do médico
                    return View(user); // Certifique-se de que está direcionando para a ação correta
                }
                else
                {
                    return RedirectToAction("Error", "Home");
                }
            }

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