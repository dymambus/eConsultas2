namespace UI.Models
{
    public class DoctorDashboardViewModel
    {
        public DoctorViewModel Doctor { get; set; }
        public List<AppointmentViewModel> Appointments { get; set; }
    }
}
