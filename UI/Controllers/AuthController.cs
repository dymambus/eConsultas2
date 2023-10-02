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
                    HttpContext.Session.SetString("Email", email);

                    return RedirectToAction("Index", "Patient");
                }
                else if (user.RoleId == 1) // Médico
                {
                    HttpContext.Session.SetString("Token", token);
                    HttpContext.Session.SetString("Email", email);

                    return RedirectToAction("DoctorDashboard", "Doctor");
                }
            }
            // Se as credenciais estiverem erradas ou o usuário não tem um papel válido
            return Unauthorized(new { message = "Credenciais inválidas" });
        }

        public IActionResult SignOut()
        {
            return RedirectToAction("Login", "Auth");
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
                        RoleId = userModel.RoleId
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
        [Route("RegisterDoctorInfo")]
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
        [Route("RegisterPatientInfo")]
        public IActionResult SavePatient(Patient userModel)
        {
            if (ModelState.IsValid)
            {
                _context.Users.Add(userModel);

                _context.SaveChanges();

                return RedirectToAction("Login");
            }

            return RedirectToAction("Register");
        }
    }
}
