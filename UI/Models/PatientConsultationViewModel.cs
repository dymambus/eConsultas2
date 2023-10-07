using LibBiz.Models;
using System.ComponentModel;

namespace UI.Models
{
    public class PatientConsultationViewModel
    {
        public PatientInfoViewModel Patient { get; set; }
        public List<AppointmentViewModel> Appointments { get; set; }
        public int SelectedAppointmentId { get; set; }
        public Attach? Attachment { get; set; }
        public byte[]? AttachmentData { get; set; }

    }
}
