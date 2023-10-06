using LibBiz.Models;

namespace UI.Models
{
    public class PatientAreaModel
    {
        public Patient? Patient { get; set; }
        public string SelectSpecialization { get; set; }
        public List<string>? Specializations { get; set; }
        public string? CityCode { get; set; }
        public List<Doctor>? Doctors { get; set; }
    }
}
