using LibBiz.Data;
using LibBiz.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]/[Action]")]
    public class AppointmentController : ControllerBase
    {
        private ILogger<AppointmentController> _logger;
        private IBusinessMethods _BM;
        public AppointmentController(ILogger<AppointmentController> logger, IBusinessMethods BM)
        {
            _logger = logger;
            _BM = BM;
        }

        [HttpGet]
        public IActionResult GetAppointmentsByDoctorId(int id)
        {
            List<Appointment> appointment = _BM.GetAppointmentsByDoctorId(id);
            return Ok(appointment);
        }

        [HttpGet]
        public IActionResult GetAppointmentsByPatientId(int id)
        {
            List<Appointment> appointment = _BM.GetAppointmentsByPatientId(id);
            return Ok(appointment);
        }


        [HttpPost]
        public IActionResult CreateAppointment(int doctorId, int patientId, string patientMessage = null)
        {
            Appointment appointment = _BM.CreateAppointment(doctorId, patientId, patientMessage);
            return Ok(appointment);
        }
    }
}
