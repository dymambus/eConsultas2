using LibBiz.Models;

namespace UI.Models
{
    public class PatientAreaModel
    {
        public Patient? Patient { get; set; }

        public string? Specialization { get; set; }
        public string? CityCode { get; set; }
        public List<Doctor>? Doctors { get; set; }
    }
}
