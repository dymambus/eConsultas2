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
        public AuthController(IConfiguration configuration, ddContext context)
        {
            _configuration = configuration;
            _context = context;

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
                    return RedirectToAction("PatientDashboard", "Patient");
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
                new Claim(ClaimTypes.Name, user.Email),
            };

            HttpContext.Session.SetString("Email", user.Email);
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
            var userModel = new User();
            return View(userModel);
        }

        [HttpPost]
        public IActionResult Register(User userModel)
        {

            if (ModelState.IsValid)
            {

                _context.Users.Add(userModel);


                _context.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(userModel);
        }
        //
        //O que fiz aqui para baixo é um teste
        //
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
        [HttpGet]
        public IActionResult RegisterPatientInfo()
        {
            return View();
        }
        [HttpPost]
        public IActionResult RegisterPatientInfo(PatientInfoViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userEmail = HttpContext.Session.GetString("UserEmail");
                var userRole = HttpContext.Session.GetInt32("UserRole");

                if (!string.IsNullOrEmpty(userEmail) && userRole.HasValue)
                {
                    if (userRole == 0)
                    {
                        var patient = new Patient
                        {
                            Email = userEmail,
                            Password = model.Password,
                            RoleId = userRole.Value,
                            Name = model.Name,
                            Phone = model.Phone
                        };
                        _context.Patients.Add(patient);
                    }
                    _context.SaveChanges();

                    return RedirectToAction("Login", "Auth");
                }
            }

            return View(model);
        }
        [HttpGet]
        public IActionResult RegisterDoctorInfo()
        {
            return View();
        }
        [HttpPost]
        public IActionResult RegisterDoctorInfo(DoctorInfoViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userEmail = HttpContext.Session.GetString("UserEmail");
                var userRole = HttpContext.Session.GetInt32("UserRole");

                if (!string.IsNullOrEmpty(userEmail) && userRole.HasValue)
                {
                    if (userRole == 1)
                    {
                        var doctor = new LibBiz.Models.Doctor
                        {
                            Name = model.Name,
                            Phone = model.Phone,
                            Email = userEmail,
                            Password = model.Password,
                            RoleId = userRole.Value,
                            Region = model.Region,
                            City = model.City,
                            Address = model.Address,
                            SpecializationName = model.SpecializationName,
                            Price = model.Price,
                        };
                        _context.Doctors.Add(doctor);
                    }
                    _context.SaveChanges();

                    return RedirectToAction("Login", "Auth");
                }
            }

            return View(model);
        }


    }
}
