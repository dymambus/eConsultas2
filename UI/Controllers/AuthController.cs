using LibBiz.Data;
using LibBiz.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UI.Models;

namespace UI.Controllers
{

    public class AuthController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ddContext _context;
        private readonly Gateway _gateway;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IConfiguration configuration, ddContext context, Gateway gateway, ILogger<AuthController> logger)
        {
            _configuration = configuration;
            _context = context;
            _gateway = gateway;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email);

            if (user != null && VerifyPassword(user, password))
            {
                var token = GenerateJwtToken(user);

                if (user.RoleId == 0) // Paciente
                {
                    HttpContext.Session.SetString("Token", token);
                    // Redirecionar para o Dashboard do Paciente
                    return RedirectToAction("Index", "Patient");
                }
                else if (user.RoleId == 1) // Médico
                {
                    HttpContext.Session.SetString("Token", token);
                    // Redirecionar para o Dashboard do Médico
                    return RedirectToAction("DoctorDashboard", "Doctor");
                }
            }
            // Se as credenciais estiverem erradas ou o usuário não tem um papel válido
            return Unauthorized(new { message = "Credenciais inválidas" });
        }


        private bool VerifyPassword(User user, string password)
        {
            return user.Password == password;
        }

        private string GenerateJwtToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email)

            };
            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(20),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(User userModel)
        {
            if (ModelState.IsValid)
            {
                if (userModel.RoleId == 1)
                {
                    Doctor doctor = new()
                    {
                        Email = userModel.Email,
                        Password = userModel.Password,
                        RoleId = userModel.RoleId
                    };

                    return RedirectToAction("RegisterDoctorInfo", doctor);
                }
                else if (userModel.RoleId == 0)
                {

                    Patient patient = new()
                    {
                        Email = userModel.Email,
                        Password = userModel.Password,
                    };

                    return RedirectToAction("RegisterPatientInfo", patient);
                }
            }

            return NotFound();
        }

        [HttpPost]
        public IActionResult CreateDoctor(Doctor doctor)
        {
            if (ModelState.IsValid)
            {
                doctor.RoleId = 1;

                _logger.LogCritical("Doctor created.");
                return Ok(_gateway.CreateUser<Doctor>(doctor));
            }
            else
                return NotFound();
        }

        [HttpPost]
        public IActionResult CreatePatient(Patient patient)
        {
            if (ModelState.IsValid)
            {
                patient.RoleId = 0;

                return Ok(_gateway.CreateUser<Patient>(patient));
            }
            else
                return NotFound();
        }

        [HttpPost]
        public IActionResult Register<T>(T user) where T : User
        {
            var userModel = new User();
            return View(userModel);
        }

        [HttpGet]
        public IActionResult RegisterDoctorInfo(Doctor doctor)
        {
            return View(doctor);
        }

        [HttpPost]
        public IActionResult SaveDoctor(Doctor userModel)
        {

            if (ModelState.IsValid)
            {

                _context.Users.Add(userModel);


                _context.SaveChanges();

                return RedirectToAction("Login");
            }

            return View(userModel);
        }

        [HttpGet]
        public IActionResult RegisterPatientInfo(Patient patient)
        {
            return View(patient);
        }

        [HttpPost]
        public IActionResult SavePatient(Patient userModel)
        {

            if (ModelState.IsValid)
            {

                _context.Users.Add(userModel);


                _context.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(userModel);
        }



        [HttpGet]
        public IActionResult RegisterEmail()
        {
            return View();
        }
        [HttpPost]
        public IActionResult RegisterEmail(UserEmailViewModel model)
        {
            if (ModelState.IsValid)
            {
                HttpContext.Session.SetString("UserEmail", model.Email);
                HttpContext.Session.SetInt32("UserRole", model.RoleId);

                return RedirectToAction("RegisterAdditionalInfo");
            }

            return View(model);
        }
        [HttpGet]
        public IActionResult RegisterAdditionalInfo()
        {

            int? userRole = HttpContext.Session.GetInt32("UserRole");

            if (userRole == null)
            {

                return RedirectToAction("Index", "Home");
            }


            if (userRole == 0)
            {

                return View("RegisterPatientInfo");
            }
            else if (userRole == 1)
            {
                return View("RegisterDoctorInfo");
            }

            return RedirectToAction("Index", "Home");
        }
        //[HttpGet]
        //public IActionResult RegisterPatientInfo()
        //{
        //    return View();
        //}
        //[HttpPost]
        //public IActionResult RegisterPatientInfo(PatientInfoViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var userEmail = HttpContext.Session.GetString("UserEmail");
        //        var userRole = HttpContext.Session.GetInt32("UserRole");

        //        if (!string.IsNullOrEmpty(userEmail) && userRole.HasValue)
        //        {
        //            if (userRole == 0)
        //            {
        //                var patient = new Patient
        //                {
        //                    Email = userEmail,
        //                    Password = model.Password,
        //                    RoleId = userRole.Value,
        //                    Name = model.Name,
        //                    Phone = model.Phone
        //                };
        //                _context.Patients.Add(patient);
        //            }
        //            _context.SaveChanges();

        //            return RedirectToAction("Login", "Auth");
        //        }
        //    }

        //    return View(model);
        //}
        //[HttpGet]
        //public IActionResult RegisterDoctorInfo()
        //{
        //    return View();
        //}
        //[HttpPost]
        //public IActionResult RegisterDoctorInfo(DoctorInfoViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var userEmail = HttpContext.Session.GetString("UserEmail");
        //        var userRole = HttpContext.Session.GetInt32("UserRole");

        //        if (!string.IsNullOrEmpty(userEmail) && userRole.HasValue)
        //        {
        //            if (userRole == 1)
        //            {
        //                var doctor = new LibBiz.Models.Doctor
        //                {
        //                    Name = model.Name,
        //                    Phone = model.Phone,
        //                    Email = userEmail,
        //                    Password = model.Password,
        //                    RoleId = userRole.Value,
        //                    Region = model.Region,
        //                    City = model.City,
        //                    Address = model.Address,
        //                    SpecializationName = model.SpecializationName,
        //                    Price = model.Price,
        //                };
        //                _context.Doctors.Add(doctor);
        //            }
        //            _context.SaveChanges();

        //            return RedirectToAction("Login", "Auth");
        //        }
        //    }

        //    return View(model);
        //}


    }
}
