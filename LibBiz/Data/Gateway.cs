using LibBiz.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibBiz.Data
{
    public class Gateway
    {
        private readonly ddContext _context;

        public Gateway(ddContext context)
        {
            _context = context;
        }

        public T CreateUser<T>(T user) where T : User
        {
            _context.Users.Add(user);
            _context.SaveChanges();

            return user;
        }

        public List<User> GetUsers()
        {
            return _context.Users.ToList();
        }

        public void DeleteUser(int id)
        {
            var user = _context.Users.Find(id);
            _context.Users.Remove(user);
            _context.SaveChanges();
        }


    }

    public interface IBusinessMethods
    {
        public List<string> GetAllSpecializations();
        public Doctor UpdateDoctor(Doctor updatedDoctor);
        public List<Doctor> GetAllDoctors();
        public Doctor GetDoctorById(int id);
        public Patient UpdatePatient(Patient updatedPatient);
        public Appointment CreateAppointment(int doctorId, int patientId, string patientMessage = null);

        public List<Appointment> GetAppointmentsByPatientId(int userId);
        public List<Appointment> GetAppointmentsByDoctorId(int userId);
    }

    public class BusinessMethodsImpl : IBusinessMethods
    {
        private readonly ddContext _context;

        public BusinessMethodsImpl(ddContext context)
        {
            _context = context;
        }

        // Appointments
        public List<Appointment> GetAppointmentsByDoctorId(int userId)
        {
            var query = _context.Appointments
                .Include(x => x.Doctor)
                .Include(x => x.Patient)
                .Where(x => x.Doctor.UserId == userId);

            return query.ToList();
        }

        public List<Appointment> GetAppointmentsByPatientId(int userId)
        {
            var query = _context.Appointments
                .Include(x => x.Doctor)
                .Include(x => x.Patient)
                .Where(x => x.Patient.UserId == userId);

            return query.ToList();
        }



        public List<string> GetAllSpecializations()
        {
            List<string> specializations = _context.Doctors
                .Select(doctor => doctor.SpecializationName)
                .Distinct()
                .ToList();

            return specializations;
        }
        public List<Doctor> GetAllDoctors()
        {
            List<Doctor> doctors = _context.Doctors.ToList();

            return doctors;
        }
        public Doctor GetDoctorById(int id)
        {
            Doctor doctor = _context.Doctors.Find(id);

            if (doctor == null)
            {
                throw new Exception("Médico não encontrado");
            }

            return doctor;
        }
        public Doctor UpdateDoctor(Doctor updatedDoctor)
        {
            var existingDoctor = _context.Doctors.Find(updatedDoctor.UserId);
            if (existingDoctor == null)
            {
                throw new Exception("Médico não encontrado");
            }

            existingDoctor.Email = updatedDoctor.Email;
            existingDoctor.Name = updatedDoctor.Name;
            existingDoctor.Phone = updatedDoctor.Phone;
            existingDoctor.SpecializationName = updatedDoctor.SpecializationName;
            existingDoctor.Price = updatedDoctor.Price;

            _context.SaveChanges();

            return updatedDoctor;
        }
        public Patient UpdatePatient(Patient updatedPatient)
        {
            var existingPatient = _context.Patients.Find(updatedPatient.UserId);
            if (existingPatient == null)
            {
                throw new Exception("Paciente não encontrado");
            }

            existingPatient.Email = updatedPatient.Email;
            existingPatient.Name = updatedPatient.Name;
            existingPatient.Phone = updatedPatient.Phone;

            _context.SaveChanges();

            return updatedPatient;
        }

        public Appointment CreateAppointment(int doctorId, int patientId, string? patientMessage = null)
        {
            var doctor = _context.Doctors.Find(doctorId);
            var patient = _context.Patients.Find(patientId);

            if (doctor == null)
            {
                throw new Exception("Médico não encontrado");
            }

            if (patient == null)
            {
                throw new Exception("Paciente não encontrado");
            }

            var appointment = new Appointment
            {
                Doctor = doctor,
                Patient = patient,
                IsDone = false,
                Date = DateTime.Now,
                Price = doctor.Price,
                PatientMessage = patientMessage
            };

            _context.Appointments.Add(appointment);
            _context.SaveChanges();

            return appointment;
        }
    }

}