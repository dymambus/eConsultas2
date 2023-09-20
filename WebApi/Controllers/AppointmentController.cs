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
        private BusinessMethods _BM;
        public AppointmentController(ILogger<AppointmentController> logger, BusinessMethods BM)
        {
            _logger = logger;
            _BM = BM;
        }

        [HttpGet]
        public IActionResult GetAllAppointments()
        {
            List<Appointment> appointments = _BM.GetAllAppointments();
            return Ok(appointments);
        }

        [HttpGet]
        public IActionResult GetAppointmentById(int id)
        {
            Appointment appointment = _BM.GetAppointmentById(id);
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
