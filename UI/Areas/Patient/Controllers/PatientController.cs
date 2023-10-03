﻿using Microsoft.AspNetCore.Mvc;
using LibBiz.Data;
using LibBiz.Models;
using Newtonsoft.Json;
using UI.Models;
using Microsoft.AspNetCore.Authorization;

namespace UI.Areas.Patient.Controllers
{
    public class PatientController : Controller
    {
        private ILogger<PatientController> _logger;
        private IBusinessMethods _BM;

        public PatientController(ILogger<PatientController> logger, IBusinessMethods BM)
        {
            _logger = logger;
            _BM = BM;
        }

        [HttpGet]
        private LibBiz.Models.Patient? GetPatient()
        {
            if (HttpContext.Session.GetString("Token") == null)
            {
                RedirectToAction("Login", "Auth");
                return null;
            }

            var patient = _BM.GetPatientByEmail(HttpContext.Session.GetString("Email"));

            if (patient != null)
            {
                var user = new LibBiz.Models.Patient()
                {
                    Name = patient.Name,
                    Email = patient.Email,
                    Phone = patient.Phone
                };

                return user;
            }

            RedirectToAction("Error", "Shared");

            return null;
        }

        [HttpGet]
        public IActionResult Index()
        {
            _logger.LogCritical("Chegou ao Index do paciente!"); // Demonstração do Logger

            PatientAreaModel patient = new()
            {
                Patient = GetPatient()
            };

            return View(patient);
        }

        [HttpGet]
        public IActionResult Profile()
        {
            PatientAreaModel patient = new()
            {
                Patient = GetPatient()
            };


            return View(patient);
        }

        [HttpGet]
        public IActionResult Search(PatientAreaModel? model = null)
        {
            if (model.Doctors == null)
            {
                model = new PatientAreaModel()
                {
                    Patient = GetPatient()
                };
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult Search()
        {
            PatientAreaModel searchModel = new()
            {
                Doctors = _BM.GetAllDoctors(),
                Patient = GetPatient()
            };

            return View(searchModel);
        }
    }
}
