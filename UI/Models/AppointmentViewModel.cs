﻿namespace UI.Models
{
    public class AppointmentViewModel
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public bool Status { get; set; } // Aberto ou concluído
        public string PatientName { get; set; }
        public string DoctorName { get; set; }
        public string PatientPhone { get; set; }
        public int? FeesPaid { get; set; }
        public string? PatientMessage { get; set; }
        public string? DoctorMessage { get; set; }
    }

}