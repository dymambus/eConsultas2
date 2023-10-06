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
            if (user.GetType() == typeof(Doctor))
            {
                Doctor doctor = new Doctor { Email = user.Email, Password = user.Password, RoleId = user.RoleId };
                _context.Users.Add(doctor);
            }
            else if (user.GetType() == typeof(Patient))
            {
                Patient patient = new Patient { Email = user.Email, Password = user.Password, RoleId = user.RoleId };
                _context.Users.Add(patient);
            }
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
        public Doctor UpdateDoctorInfo(int doctorId, string name, string phone);
        public List<Doctor> GetAllDoctors();
        public Doctor GetDoctorById(int id);
        public Patient UpdatePatient(Patient updatedPatient);
        public Appointment CreateAppointment(int doctorId, int patientId, string patientMessage = null);
        public Doctor GetDoctorByEmail(string email);
        public List<Appointment> GetAppointmentsByPatientId(int userId);
        //public List<Appointment> GetAppointmentById(int appointmentId);
        public List<Appointment> GetAppointmentsByDoctorId(int userId);
        public Doctor UpdateDoctorPhoto(Doctor doctor);
        public Doctor UpdateDoctor(Doctor updatedDoctor);
        public Doctor UpdateDoctorSpecialization(int doctorId, string specialization);
        public Doctor UpdateDoctorClinic(int doctorId, string address, string region, string city);
        public Doctor UpdateDoctorFees(int doctorId, int fees);
        public Doctor UpdateDoctorPassword(int doctorId, string oldpassword, string newpassword);
        public Patient? GetPatientByEmail(string? email);
        public Appointment UpdateAppointment(int appointmentId, string? doctorMessage = null);
        public Appointment GetAppointmentById(int appointmentId);

    }

    public class BusinessMethodsImpl : IBusinessMethods
    {
        private readonly ddContext _context;
        public BusinessMethodsImpl(ddContext context)
        {
            _context = context;
        }
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
        //public List<Appointment> GetAppointmentById(int appointmentId)
        //{
        //    var query = _context.Appointments
        //        .Include(x => x.Doctor)
        //        .Include(x => x.Patient)
        //        .Where(x => x.Id == appointmentId);

        //    return query.ToList();
        //}

        public Appointment GetAppointmentById(int appointmentId)
        {
            var query = _context.Appointments
                .Include(x => x.Doctor)
                .Include(x => x.Patient)
                .FirstOrDefault(x => x.Id == appointmentId);

            return query;
        }
        public Patient? GetPatientByEmail(string? email)
        {
            return _context.Patients.Where(x => x.Email == email).FirstOrDefault();
        }
        public Doctor GetDoctorByEmail(string email)
        {
            Doctor doctor = _context.Doctors.Include(d => d.Photograph).FirstOrDefault(d => d.Email == email);
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
        public Doctor UpdateDoctorInfo(int doctorId, string name, string phone)
        {
            var existingDoctor = _context.Doctors.Find(doctorId);
            if (existingDoctor == null)
            {
                throw new Exception("Médico não encontrado");
            }
            existingDoctor.Name = name;
            existingDoctor.Phone = phone;

            _context.SaveChanges();

            return existingDoctor;
        }
        public Doctor UpdateDoctorPhoto(Doctor doctor)
        {
            var existingDoctor = _context.Doctors.Find(doctor.UserId);
            if (existingDoctor == null)
            {
                throw new Exception("Médico não encontrado");
            }

            existingDoctor.Photograph = doctor.Photograph;

            _context.SaveChanges();

            return existingDoctor;
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
        public Doctor UpdateDoctorSpecialization(int doctorId, string specialization)
        {
            var existingDoctor = _context.Doctors.Find(doctorId);
            if (existingDoctor == null)
            {
                throw new Exception("Médico não encontrado");
            }
            existingDoctor.SpecializationName = specialization;

            _context.SaveChanges();

            return existingDoctor;
        }

        public Doctor UpdateDoctorClinic(int doctorId, string address, string region, string city)
        {
            var existingDoctor = _context.Doctors.Find(doctorId);
            if (existingDoctor == null)
            {
                throw new Exception("Médico não encontrado");
            }
            existingDoctor.Address = address;
            existingDoctor.Region = region;
            existingDoctor.City = city;

            _context.SaveChanges();

            return existingDoctor;
        }

        public Doctor UpdateDoctorFees(int doctorId, int fees)
        {
            var existingDoctor = _context.Doctors.Find(doctorId);
            if (existingDoctor == null)
            {
                throw new Exception("Médico não encontrado");
            }
            existingDoctor.Price = fees;

            _context.SaveChanges();

            return existingDoctor;
        }

        public Doctor UpdateDoctorPassword(int doctorId, string oldpassword, string newpassword)
        {
            var existingDoctor = _context.Doctors.Find(doctorId);
            if (existingDoctor == null)
            {
                throw new Exception("Médico não encontrado");
            }
            if (oldpassword==existingDoctor.Password)
            {
                existingDoctor.Password = newpassword;
            }
            else
            {
                throw new Exception("Senha incorreta");
            }

            _context.SaveChanges();

            return existingDoctor;
        }

        public Appointment UpdateAppointment(int appointmentId, string? doctorMessage)
        {
            var existingAppointment = _context.Appointments.Find(appointmentId);
            if (existingAppointment == null)
            {
                throw new Exception("Consulta não encontrada");
            }

            existingAppointment.DoctorMessage = doctorMessage;


            _context.SaveChanges();

            return existingAppointment;
        }
    }
}

