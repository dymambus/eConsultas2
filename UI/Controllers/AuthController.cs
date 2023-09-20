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
            // Implemente a lógica para verificar a senha de forma segura
            // Por exemplo, você pode usar bibliotecas de hashing de senha
            // ou outras técnicas seguras para verificar a senha do usuário.
            // Se a senha corresponder, retorne true; caso contrário, retorne false.

            // Exemplo simples de verificação (não seguro):
            return user.Password == password;
        }

        private string GenerateJwtToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Email),
                // Você pode adicionar mais claims aqui conforme necessário
            };

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(20), // Defina o tempo de expiração do token conforme necessário
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
    }
}
