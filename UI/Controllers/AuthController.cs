using LibBiz.Data;
using LibBiz.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

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
                return Ok(new { token });
            }
            else
            {
                return Unauthorized(new { message = "Credenciais inválidas" });
            }
        }

        private bool VerifyPassword(User user, string password)
        {

            return user.Password == password;
        }

        private string GenerateJwtToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Email),

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
            var userModel = new User();
            return View(userModel);
        }

        [HttpPost]
        public IActionResult Register(User userModel)
        {

            return RedirectToAction("Index");
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
    }
}
