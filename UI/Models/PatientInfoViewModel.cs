using System.ComponentModel.DataAnnotations;

namespace UI.Models
{
    public class PatientInfoViewModel
    {
        [Required]
        public string Password { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [Phone]
        public string Phone { get; set; }
    }
}
