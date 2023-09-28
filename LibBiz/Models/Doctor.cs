using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibBiz.Models
{
    public class Doctor : User
    {
        public string? Region { get; set; }
        public string? City { get; set; }
        public string? Address { get; set; }
        public string? SpecializationName { get; set; }
        public int? Price { get; set; }

        [NotMapped] // Isso indica que essa propriedade não é mapeada para o banco de dados
        public IFormFile Photo { get; set; }
        public Photograph? Photograph { get; set; }
    }
}
