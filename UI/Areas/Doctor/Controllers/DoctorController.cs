﻿using LibBiz.Data;
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
                    var user = new DoctorViewModel()
                    {
                        // Preencha as propriedades de DoctorInfoViewModel com os dados do médico
                        UserId = doctor.UserId,
                        RoleId = doctor.RoleId,
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
        public IActionResult DoctorProfile()
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
                    if (doctor.Photograph == null)
                    {
                        doctor.Photograph = new Photograph();
                    }
                    var user = new DoctorViewModel()
                    {
                        // Preencha as propriedades de DoctorInfoViewModel com os dados do médico
                        UserId = doctor.UserId,
                        RoleId = doctor.RoleId,
                        Name = doctor.Name,
                        Email = userEmail,
                        Phone = doctor.Phone,
                        Address = doctor.Address,
                        Region = doctor.Region,
                        City = doctor.City,
                        SpecializationName = doctor.SpecializationName,
                        Price = (int)doctor.Price,
                        ImageData = doctor.Photograph.ImageData
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
                    var user = new DoctorViewModel()
                    {
                        // Preencha as propriedades de DoctorInfoViewModel com os dados do médico
                        UserId = doctor.UserId,
                        RoleId = doctor.RoleId,
                        SpecializationName = doctor.SpecializationName,
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
                    var user = new DoctorViewModel()
                    {
                        // Preencha as propriedades de DoctorInfoViewModel com os dados do médico
                        UserId = doctor.UserId,
                        Address = doctor.Address,
                        Region = doctor.Region,
                        City = doctor.City
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
                    var user = new DoctorViewModel()
                    {
                        UserId = doctor.UserId,
                        Price= (int)doctor.Price
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

        [HttpPost]
        public IActionResult UploadProfilePicture(int doctorId, IFormFile profilePicture)
        {
            // Recupere o médico do banco de dados com base no ID ou na sessão
            var doctor = _BM.GetDoctorById(doctorId);

            if (doctor != null && profilePicture != null && profilePicture.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    profilePicture.CopyTo(ms);
                    doctor.Photograph = new Photograph
                    {
                        UserId = doctor.UserId,
                        ImageData = ms.ToArray()
                    };
                    _BM.UpdateDoctorPhoto(doctor);
                }
                // Atualize o médico no banco de dados para incluir a foto

                return RedirectToAction("DoctorProfile");
            }

            return RedirectToAction("DoctorProfile"); // Trate os casos de erro adequadamente
        }

        [HttpPost]
        public IActionResult UpdateDoctorInfo(int doctorId, string name, string phone)
        {
            var doctor = _BM.UpdateDoctorInfo(doctorId, name, phone);
            return RedirectToAction("DoctorProfile");

        }

        [HttpPost]
        public IActionResult UpdateDoctorSpecialization(int doctorId, string specializationName)
        {
            var doctor = _BM.UpdateDoctorSpecialization(doctorId, specializationName);
            return RedirectToAction("DoctorSpecialization");

        }

        [HttpPost]
        public IActionResult UpdateDoctorClinic(int doctorId, string address, string region, string city)
        {
            var doctor = _BM.UpdateDoctorClinic(doctorId, address, region, city);
            return RedirectToAction("DoctorClinic");

        }

        [HttpPost]
        public IActionResult UpdateDoctorFees(int doctorId, int price)
        {
            var doctor = _BM.UpdateDoctorFees(doctorId, price);
            return RedirectToAction("DoctorFees");

        }
        [HttpPost]
        public IActionResult UpdateDoctorPassword(int doctorId, string oldpassword, string newpassword)
        {
            var doctor = _BM.UpdateDoctorPassword(doctorId, oldpassword, newpassword);
            return RedirectToAction("DoctorProfile");

        }


    }
}