namespace UI.Models
{
    public class DoctorConsultationViewModel
    {
        public DoctorViewModel Doctor { get; set; }
        public List<AppointmentViewModel> Appointments { get; set; }
        public int SelectedAppointmentId { get; set; }
        public byte[]? AttachmentData { get; set; }
    }
}
