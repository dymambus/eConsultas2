using LibBiz.Models;
using System.ComponentModel.DataAnnotations;

namespace UI.Models
{
    public class DoctorViewModel
    {
        public int UserId { get; set; }
        public int RoleId { get; set; } // 0 - Pacient, 1 - Doctor, 2 - Admin
        public string Email { get; set; }
        public string Password { get; set; }
        public string? NewPassword { get; set; } // for changing password
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public string? Region { get; set; }
        public string? City { get; set; }
        public string? Address { get; set; }
        public string? SpecializationName { get; set; }
        public string? SpecializationDescription { get; set; }
        public string? PriceNotes { get; set; }
        public int? Price { get; set; }
        public Photograph? Photograph { get; set; }
        public byte[]? ImageData { get; set; }
        public List<Appointment>? Appointments { get; set; }
    }
}
