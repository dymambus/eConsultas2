using System.ComponentModel.DataAnnotations;

namespace UI.Models
{
    public class DoctorInfoViewModel
    {
        [Required]
        public string Password { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [Phone]
        public string Phone { get; set; }

        [Required]
        public string Region { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string SpecializationName { get; set; }

        [Required]
        [Range(0, 500)]
        public int Price { get; set; }

    }
}
