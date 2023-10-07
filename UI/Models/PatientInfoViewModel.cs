using System.ComponentModel.DataAnnotations;

namespace UI.Models
{
    public class PatientInfoViewModel
    {
        public int? UserId { get; set; }
        [Required]
        public string Password { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [Phone]
        public string Phone { get; set; }

        public string Email { get; set; }
    }
}
