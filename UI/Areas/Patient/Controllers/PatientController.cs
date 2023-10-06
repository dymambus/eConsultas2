using Microsoft.AspNetCore.Mvc;
using LibBiz.Data;
using LibBiz.Models;
using Newtonsoft.Json;
using UI.Models;

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

            PatientAreaModel patient = new()
            {
                Patient = GetPatient()
            };

            return View(patient);
        }

        [HttpGet]
        public IActionResult Profile()
        {
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
            if (token == null)
            {
                return RedirectToAction("Login", "Auth");
            }
            else
            {
                // Consulte o banco de dados para obter a lista de médicos
                var doctors = _BM.GetAllDoctors();

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
            if (ModelState.IsValid)
            {
                // Verifique o valor selecionado em SelectSpecialization e filtre a lista de médicos conforme necessário
                if (!string.IsNullOrEmpty(model.SelectSpecialization))
                {
                    var filteredDoctors = _BM.GetDoctorsBySpecialization(model.SelectSpecialization);
                    model.Doctors = filteredDoctors;
                }

                // Consulte o banco de dados para obter a lista completa de especializações
                var specializations = _BM.GetAllSpecializations();
                model.Specializations = specializations;

                // Agora você tem o modelo preenchido com os médicos filtrados (se houver) e a lista completa de especializações
                return View("Search", model);
            }
            else
            {
                // Se o modelo não for válido, retorne à página de pesquisa
                return View("Search", model);
            }
        }




    }
}
