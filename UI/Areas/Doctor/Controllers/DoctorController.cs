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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Numerics;

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
            var userRole = HttpContext.Session.GetInt32("UserRole");
            if (userRole == 0)
            {
                return RedirectToAction("Error", "LandingPage");
            }
            if (token == null)
            {
                return RedirectToAction("Login", "Auth");
            }
            else
            {
                var userEmail = HttpContext.Session.GetString("Email");

                // Consulte o banco de dados para obter as consultas do médico com base no email
                var doctor = _BM.D_GetByEmail(userEmail);

                if (doctor != null)
                {
                    // Consulta as consultas do médico
                    var appointments = _BM.GetAppointmentsByDoctorId(doctor.UserId);

                    // Crie uma instância de DoctorDashboardViewModel e preencha as propriedades Doctor e Appointments
                    var dashboardViewModel = new DoctorConsultationViewModel
                    {
                        Doctor = new DoctorViewModel
                        {
                            // Preencha as propriedades do médico a partir do objeto 'doctor'
                            UserId = doctor.UserId,
                            Email = doctor.Email,
                            Name = doctor.Name,
                            // Outras propriedades do médico
                        },
                        Appointments = appointments.Select(appointment => new AppointmentViewModel
                        {
                            // Preencha as propriedades das consultas a partir dos objetos 'appointments'
                            Id = appointment.Id,
                            Date = appointment.Date,
                            IsDone = appointment.IsDone,
                            PatientName = appointment.Patient.Name,
                            DoctorName = appointment.Doctor.Name,
                            PatientPhone = appointment.Patient.Phone,
                            FeesPaid = appointment.Price,
                            PatientMessage = appointment.PatientMessage,
                            DoctorMessage = appointment.DoctorMessage

                            // Outras propriedades das consultas
                        }).ToList()
                    };

                    // Agora você tem a ViewModel composta pronta para exibição na página
                    return View(dashboardViewModel);
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
            var userRole = HttpContext.Session.GetInt32("UserRole");
            if (userRole == 0)
            {
                return RedirectToAction("Error", "LandingPage");
            }
            if (token == null)
            {
                return RedirectToAction("Login", "Auth");
            }
            else
            {
                // Obtenha o email do médico logado (você pode armazená-lo na sessão ou de outra forma)
                var userEmail = HttpContext.Session.GetString("Email");

                // Consulte o banco de dados para obter as informações do médico com base no email
                var doctor = _BM.D_GetByEmail(userEmail);

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
            var userRole = HttpContext.Session.GetInt32("UserRole");
            if (userRole == 0)
            {
                return RedirectToAction("Error", "LandingPage");
            }
            if (token == null)
            {
                return RedirectToAction("Login", "Auth");
            }
            else
            {
                var userEmail = HttpContext.Session.GetString("Email");

                // Consulte o banco de dados para obter as informações do médico com base no email
                var doctor = _BM.D_GetByEmail(userEmail);

                if (doctor != null)
                {
                    var user = new DoctorViewModel()
                    {
                        // Preencha as propriedades de DoctorInfoViewModel com os dados do médico
                        UserId = doctor.UserId,
                        RoleId = doctor.RoleId,
                        SpecializationName = doctor.SpecializationName,
                        SpecializationDescription = doctor.SpecializationDescription
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
            var userRole = HttpContext.Session.GetInt32("UserRole");
            if (userRole == 0)
            {
                return RedirectToAction("Error", "LandingPage");
            }
            if (token == null)
            {
                return RedirectToAction("Login", "Auth");
            }
            else
            {
                var userEmail = HttpContext.Session.GetString("Email");

                // Consulte o banco de dados para obter as informações do médico com base no email
                var doctor = _BM.D_GetByEmail(userEmail);

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
            var userRole = HttpContext.Session.GetInt32("UserRole");
            if (userRole == 0)
            {
                return RedirectToAction("Error", "LandingPage");
            }
            if (token == null)
            {
                return RedirectToAction("Login", "Auth");
            }
            else
            {
                var userEmail = HttpContext.Session.GetString("Email");

                // Consulte o banco de dados para obter as informações do médico com base no email
                var doctor = _BM.D_GetByEmail(userEmail);

                if (doctor != null)
                {
                    var user = new DoctorViewModel()
                    {
                        UserId = doctor.UserId,
                        Price= (int)doctor.Price,
                        PriceNotes = doctor.PriceDescription
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
        public IActionResult DoctorConsultation(int appointmentId, string userEmail)
        {
            var token = HttpContext.Session.GetString("Token");
            var userRole = HttpContext.Session.GetInt32("UserRole");
            if (userRole == 0)
            {
                return RedirectToAction("Error", "LandingPage");
            }
            if (token == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var doctor = _BM.D_GetByEmail(userEmail);
            var appointment = _BM.GetAppointmentById(appointmentId);

            if (doctor == null || appointment == null)
            {
                return RedirectToAction("Error", "Home");
            }

            var dashboardViewModel = CreateDoctorConsultationViewModel(doctor, appointmentId);

            return View(dashboardViewModel);
        }

        public DoctorConsultationViewModel CreateDoctorConsultationViewModel(LibBiz.Models.Doctor doctor, int selectedAppointmentId)
        {
            var appointments = _BM.GetAppointmentsByDoctorId(doctor.UserId);

            var appointment = _BM.GetAppointmentById(selectedAppointmentId);

            var dashboardViewModel = new DoctorConsultationViewModel
            {
                SelectedAppointmentId = selectedAppointmentId,
                AttachmentData = appointment.Attach?.FileData,
                Doctor = new DoctorViewModel
                {
                    UserId = doctor.UserId,
                    Email = doctor.Email,
                    Name = doctor.Name,
                    // Outras propriedades do médico
                },
                Appointments = appointments.Select(appointment => new AppointmentViewModel
                {
                    Id = appointment.Id,
                    Date = appointment.Date,
                    IsDone = appointment.IsDone,
                    PatientName = appointment.Patient.Name,
                    DoctorName = appointment.Doctor.Name,
                    PatientPhone = appointment.Patient.Phone,
                    FeesPaid = appointment.Price,
                    PatientMessage = appointment.PatientMessage,
                    DoctorMessage = appointment.DoctorMessage
                    // Outras propriedades das consultas
                }).ToList()
            };

            return dashboardViewModel;
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
            List<LibBiz.Models.Doctor> doctors = _BM.D_GetAll();
            return Ok(doctors);
        }

        [HttpGet]
        public IActionResult GetDoctorById(int id)
        {
            LibBiz.Models.Doctor doctor = _BM.D_GetById(id);
            return Ok(doctor);
        }

        [HttpPost]
        public IActionResult UploadProfilePicture(int doctorId, IFormFile profilePicture)
        {
            // Recupere o médico do banco de dados com base no ID ou na sessão
            var doctor = _BM.D_GetById(doctorId);

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
                    _BM.D_UpdatePhoto(doctor);
                }
                // Atualize o médico no banco de dados para incluir a foto

                return RedirectToAction("DoctorProfile");
            }

            return RedirectToAction("DoctorProfile"); // Trate os casos de erro adequadamente
        }

        [HttpPost]
        public IActionResult UpdateDoctorInfo(int doctorId, string name, string phone)
        {
            var doctor = _BM.D_UpdateInfo(doctorId, name, phone);
            return RedirectToAction("DoctorProfile");

        }

        [HttpPost]
        public IActionResult UpdateDoctorSpecialization(int doctorId, string specializationName, string SpecializationDescription)
        {
            var doctor = _BM.D_UpdateSpecialization(doctorId, specializationName, SpecializationDescription);
            return RedirectToAction("DoctorSpecialization");

        }

        [HttpPost]
        public IActionResult UpdateDoctorClinic(int doctorId, string address, string region, string city)
        {
            var doctor = _BM.D_UpdateClinic(doctorId, address, region, city);
            return RedirectToAction("DoctorClinic");

        }

        [HttpPost]
        public IActionResult UpdateDoctorFees(int doctorId, int price, string PriceNotes)
        {
            var doctor = _BM.D_UpdateFees(doctorId, price, PriceNotes);
            return RedirectToAction("DoctorFees");

        }

        [HttpPost]
        public IActionResult UpdateDoctorPassword(int doctorId, string password, string newpassword)
        {
            var doctor = _BM.D_UpdatePassword(doctorId, password, newpassword);
            return RedirectToAction("DoctorProfile");

        }

        [HttpPost]
        public IActionResult UpdateDoctorMessage(int appointmentId, string doctorMessage)
        {
            // Consulte o banco de dados para obter a consulta com base no ID
            var appointment = _BM.GetAppointmentById(appointmentId);

            if (appointment == null)
            {
                return RedirectToAction("Error", "Home");
            }

            // Salve as alterações no banco de dados
            _BM.UpdateDoctorMessage(appointment.Id, doctorMessage);

            // Redirecione de volta à página de detalhes da consulta
            return RedirectToAction("DoctorConsultation", new { appointmentId = appointmentId, userEmail = appointment.Doctor.Email });
        }

        [HttpPost]
        public IActionResult CloseAppointment(int appointmentId)
        {
            var appointment = _BM.GetAppointmentById(appointmentId);

            if (appointment == null)
            {
                return RedirectToAction("Error", "Home");
            }

            // Verifique se a consulta está "em aberto" antes de fechá-la
            if (!appointment.IsDone)
            {
                // Atualize o status da consulta para "concluído"
                appointment.IsDone = true;
                _BM.UpdateDoctorMessage(appointment.Id, appointment.DoctorMessage);
            }

            // Redirecione de volta à página de detalhes da consulta
            return RedirectToAction("DoctorConsultation", new { appointmentId = appointmentId, userEmail = appointment.Doctor.Email });
        }

        public IActionResult DownloadAttachment(int appointmentId)
        {
            var appointment = _BM.GetAppointmentById(appointmentId);
            if (appointment == null || appointment.Attach == null || appointment.Attach.FileData == null)
            {
                // Trate a situação em que o anexo não existe ou está vazio
                return RedirectToAction("Error", "Shared"); // Ou qualquer outra ação de tratamento de erro
            }

            // Retorna o anexo como um arquivo para download
            return File(appointment.Attach.FileData, "application/octet-stream", appointment.Attach.FileName);
        }


    }
}