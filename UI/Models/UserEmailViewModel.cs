using System.ComponentModel.DataAnnotations;

namespace UI.Models
{
    public class UserEmailViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public int RoleId { get; set; } // 0 - Paciente, 1 - Médico
    }
}