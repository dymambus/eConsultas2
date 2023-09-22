using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibBiz.Models
{
    public class User
    {
        public int UserId { get; set; }
        public int RoleId { get; set; } // 0 - Pacient, 1 - Doctor, 2 - Admin

        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public string? Name { get; set; }
        public string? Phone { get; set; }
    }
}
