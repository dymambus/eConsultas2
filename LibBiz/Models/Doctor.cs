using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public string? SpecializationDescription { get; set; }
        public int? Price { get; set; }
        public string? PriceDescription { get; set; }
        public Photograph? Photograph { get; set; }
    }
}
