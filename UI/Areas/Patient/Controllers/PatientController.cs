using Microsoft.AspNetCore.Mvc;
using LibBiz.Data;
using LibBiz.Models;
using Newtonsoft.Json;
using UI.Models;
using System.Numerics;
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

            var patient = _BM.P_GetByEmail(HttpContext.Session.GetString("Email"));

            if (patient != null)
            {
                var user = new LibBiz.Models.Patient()
                {
                    Name = patient.Name,
                    Email = patient.Email,
                    Phone = patient.Phone,
                    UserId = patient.UserId,
                    RoleId = patient.RoleId,
                    Password = patient.Password
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
            var token = HttpContext.Session.GetString("Token");
            var userRole = HttpContext.Session.GetInt32("UserRole");
            if (userRole == 1)
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
                var patient = _BM.P_GetByEmail(userEmail);

                if (patient != null)
                {
                    // Consulta as consultas do médico
                    var appointments = _BM.GetAppointmentsByPatientId(patient.UserId);

                    // Crie uma instância de DoctorDashboardViewModel e preencha as propriedades Doctor e Appointments
                    var IndexViewModel = new PatientConsultationViewModel
                    {
                        Patient = new PatientInfoViewModel
                        {
                            // Preencha as propriedades do médico a partir do objeto 'doctor'
                            UserId = patient.UserId,
                            Email = patient.Email,
                            Name = patient.Name,
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
                    return View(IndexViewModel);
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
            var userRole = HttpContext.Session.GetInt32("UserRole");
            if (userRole == 1)
            {
                return RedirectToAction("Error", "LandingPage");
            }
            P_ProfileModel currPatient = new()
            {
                Patient = GetPatient()
            };


            return View(currPatient);
        }

        [HttpPost]
        public IActionResult P_UpdateProfile(P_ProfileModel model)
        {
            var patient = GetPatient();

            patient.Name = model.Patient.Name;
            patient.Phone = model.Patient.Phone;

            _BM.P_Update(patient);

            return RedirectToAction("Profile");
        }

        [HttpGet]
        public IActionResult Search()
        {
            var token = HttpContext.Session.GetString("Token");
            var userRole = HttpContext.Session.GetInt32("UserRole");
            if (userRole == 1)
            {
                return RedirectToAction("Error", "LandingPage");
            }
            if (token == null)
            {
                return RedirectToAction("Login", "Auth");
            }
            else
            {
                // Consulte o banco de dados para obter a lista de médicos
                var doctors = _BM.D_GetAll();

                // Consulte o banco de dados para obter a lista de especializações
                var specializations = _BM.GetAllSpecializations();

                // Crie uma instância de PatientAreaModel e preencha as propriedades Doctors e Specializations
                var model = new PatientAreaModel
                {
                    Doctors = doctors,
                    Specializations = specializations
                };

                // Agora você tem a ViewModel composta pronta para exibição na página Search.cshtml
                return View(model);
            }
        }

        [HttpPost]
        public IActionResult SearchDoctors(PatientAreaModel model)
        {
            // Consulte o banco de dados para obter a lista completa de especializações
            var specializations = _BM.GetAllSpecializations();
            model.Specializations = specializations;

            // Verifique o valor selecionado em SelectSpecialization e filtre a lista de médicos conforme necessário
            if (string.IsNullOrEmpty(model.SelectSpecialization) || model.SelectSpecialization == "Show All")
            {
                // Carregar a lista completa de médicos sem aplicar filtros
                var doctors = _BM.D_GetAll();
                model.Doctors = doctors;
            }
            else
            {
                // Filtrar a lista de médicos com base na especialização selecionada
                var filteredDoctors = _BM.D_GetBySpecialization(model.SelectSpecialization);
                model.Doctors = filteredDoctors;
            }

            if (ModelState.IsValid)
            {
                return View("Search", model);
            }
            else
            {
                return View("Search", model);
            }
        }

        [HttpPost]
        public IActionResult MakePay(int doctorId)
        {
            var patient = GetPatient();

            // Salve a consulta no banco de dados
            var appointment = _BM.CreateAppointment(doctorId, patient.UserId); // Supondo que você tenha um método para criar consultas


            // Redirecione para a página de confirmação ou para a página de detalhes da consulta do paciente
            return RedirectToAction("PatientConsultation", new { appointmentId = appointment.Id, userEmail = patient.Email });
        }

        [HttpGet]
        public IActionResult PatientConsultation(int appointmentId, string userEmail)
        {
            var token = HttpContext.Session.GetString("Token");
            var userRole = HttpContext.Session.GetInt32("UserRole");
            if (userRole == 1)
            {
                return RedirectToAction("Error", "LandingPage");
            }
            if (token == null)
            {
                return RedirectToAction("Login", "Auth");
            }
            else
            {
                var patient = _BM.P_GetByEmail(userEmail);
                var appointment = _BM.GetAppointmentById(appointmentId);

                if (patient == null || appointment == null)
                {
                    return RedirectToAction("Error", "Shared");
                }
                var ViewModel = CreatePacientConsultationViewModel(patient, appointmentId);

                return View(ViewModel);
            }
        }
        public PatientConsultationViewModel CreatePacientConsultationViewModel(LibBiz.Models.Patient patient, int appointmentId)
        {
            var appointments = _BM.GetAppointmentsByPatientId(patient.UserId);

            var appointment = _BM.GetAppointmentById(appointmentId);

            var ViewModel = new PatientConsultationViewModel
            {
                SelectedAppointmentId = appointmentId,
                AttachmentData = appointment.Attach?.FileData,
                Patient= new PatientInfoViewModel
                {
                    Name = patient.Name,
                    Email = patient.Email,
                    Phone = patient.Phone
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
                }).ToList()
            };
            return ViewModel;
        }

        [HttpPost]
        public IActionResult UpdatePatientMessage(int appointmentId, string patientMessage)
        {
            var appointment = _BM.GetAppointmentById(appointmentId);

            if (appointment == null)
            {
                return RedirectToAction("Error", "Shared");
            }

            _BM.UpdatePatientMessage(appointment.Id, patientMessage);

            return RedirectToAction("PatientConsultation", new { appointmentId = appointmentId, userEmail = appointment.Patient.Email });
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile(int SelectedAppointmentId, IFormFile file)
        {
            var appointment = _BM.GetAppointmentById(SelectedAppointmentId);
            if (appointment == null)
            {
                return RedirectToAction("Error", "Shared");
            }
            else
            {
                if (appointment.Attach == null)
                {
                    appointment.Attach = new Attach(); // Inicialize Attach se for null
                }

                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    appointment.Attach.FileName = file.FileName; // Defina o nome do arquivo
                    appointment.Attach.FileData = memoryStream.ToArray();
                }

                // Salve o anexo no banco de dados
                await _BM.SaveAttachment(appointment.Attach);
            }

            // Corrija a maneira como você passa o valor do email do paciente (caso contrário, pode estar vazio)
            var userEmail = appointment.Patient?.Email ?? "";

            return RedirectToAction("PatientConsultation", new { appointmentId = SelectedAppointmentId, userEmail });
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
