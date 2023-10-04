using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace LibBiz.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public Patient Patient { get; set; }
        public Doctor Doctor { get; set; }
        public bool IsDone { get; set; } = false;
        public DateTime Date { get; set; }
        public int? Price { get; set; }
        public string? PatientMessage { get; set; }
        public Attach? Attach { get; set; }
        public string? DoctorMessage { get; set; }
    }
}
