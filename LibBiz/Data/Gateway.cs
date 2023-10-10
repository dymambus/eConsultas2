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

        // Patient CRUD
        public Patient? P_Update(Patient patient);
        public Patient? P_GetByEmail(string? email);

        //  Doctor CRUD
        public List<Doctor> D_GetAll();
        public Doctor D_UpdateInfo(int doctorId, string name, string phone);
        public Doctor D_GetById(int id);
        public Doctor D_GetByEmail(string email);
        public Doctor D_UpdatePhoto(Doctor doctor);
        public Doctor D_Update(Doctor updatedDoctor);
        public Doctor D_UpdateSpecialization(int doctorId, string specialization, string? message);
        public Doctor D_UpdateClinic(int doctorId, string address, string region, string city);
        public Doctor D_UpdateFees(int doctorId, int fees, string PriceNotes);
        public Doctor D_UpdatePassword(int doctorId, string oldpassword, string newpassword);
        public List<Doctor> D_GetBySpecialization(string spName);

        //  Appointment CRUD
        public Appointment CreateAppointment(int doctorId, int patientId, string patientMessage = null);
        public Appointment UpdateDoctorMessage(int appointmentId, string? doctorMessage);
        public Appointment UpdatePatientMessage(int appointmentId, string? patientMessage);
        public Appointment GetAppointmentById(int appointmentId);
        public List<Appointment> GetAppointmentsByDoctorId(int userId);
        public List<Appointment> GetAppointmentsByPatientId(int userId);
        public Task<int> SaveAttachment(Attach attach);

    }

    public class BusinessMethodsImpl : IBusinessMethods
    {
        private readonly ddContext _context;
        public BusinessMethodsImpl(ddContext context)
        {
            _context = context;
        }
        public Patient? P_Update(Patient newPatient)
        {
            var oldPatient = _context.Patients.Find(newPatient.UserId);

            if (oldPatient == null)
            {
                throw new Exception("Médico não encontrado");
            }

            oldPatient.Email = newPatient.Email;
            oldPatient.Phone = newPatient.Phone;
            oldPatient.Name = newPatient.Name;
            oldPatient.UserId = newPatient.UserId;
            oldPatient.RoleId = newPatient.RoleId;

            _context.SaveChanges();

            return newPatient;
        }
        public List<Doctor> D_GetBySpecialization(string spName)
        {
            var allDoctors = D_GetAll();

            var filteredDoctors = allDoctors.FindAll(doctor => doctor.SpecializationName == spName);

            return filteredDoctors;
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
                .Include(x => x.Attach)
                .Include(x => x.Patient)
                .Where(x => x.Patient.UserId == userId);

            return query.ToList();
        }
        public List<string>? GetAllSpecializations()
        {
            List<string>? specializations = _context.Doctors
                .Select(doctor => doctor.SpecializationName)
                .Distinct()
                .ToList();

            return specializations;
        }
        public List<Doctor> D_GetAll()
        {
            List<Doctor> doctors = _context.Doctors.ToList();

            return doctors;
        }
        public Doctor D_GetById(int id)
        {
            Doctor doctor = _context.Doctors.Find(id);

            if (doctor == null)
            {
                throw new Exception("Médico não encontrado");
            }

            return doctor;
        }
        public Appointment GetAppointmentById(int appointmentId)
        {
            var query = _context.Appointments
                .Include(x => x.Doctor)
                .Include(x => x.Attach)
                .Include(x => x.Patient)
                .FirstOrDefault(x => x.Id == appointmentId);

            return query;
        }
        public Patient? P_GetByEmail(string? email)
        {
            return _context.Patients.Where(x => x.Email == email).FirstOrDefault();
        }
        public Doctor D_GetByEmail(string email)
        {
            Doctor doctor = _context.Doctors.Include(d => d.Photograph).FirstOrDefault(d => d.Email == email);
            if (doctor == null)
            {
                throw new Exception("Médico não encontrado");
            }

            return doctor;
        }
        public Doctor D_Update(Doctor updatedDoctor)
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
        public Doctor D_UpdateInfo(int doctorId, string name, string phone)
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
        public Doctor D_UpdatePhoto(Doctor doctor)
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
        public Appointment CreateAppointment(int doctorId, int patientId, string? patientMessage)
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
        public Doctor D_UpdateSpecialization(int doctorId, string specialization, string? s)
        {
            var existingDoctor = _context.Doctors.Find(doctorId);
            if (existingDoctor == null)
            {
                throw new Exception("Médico não encontrado");
            }
            existingDoctor.SpecializationName = specialization;
            existingDoctor.SpecializationDescription = s;

            _context.SaveChanges();

            return existingDoctor;
        }
        public Doctor D_UpdateClinic(int doctorId, string address, string region, string city)
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
        public Doctor D_UpdateFees(int doctorId, int fees, string PriceNotes)
        {
            var existingDoctor = _context.Doctors.Find(doctorId);
            if (existingDoctor == null)
            {
                throw new Exception("Médico não encontrado");
            }
            existingDoctor.Price = fees;
            existingDoctor.PriceDescription = PriceNotes;

            _context.SaveChanges();

            return existingDoctor;
        }
        public Doctor D_UpdatePassword(int doctorId, string oldpassword, string newpassword)
        {
            var existingDoctor = _context.Doctors.Find(doctorId);
            if (existingDoctor == null)
            {
                throw new Exception("Médico não encontrado");
            }
            if (oldpassword == existingDoctor.Password)
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
        public Appointment UpdateDoctorMessage(int appointmentId, string? doctorMessage)
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
        public Appointment UpdatePatientMessage(int appointmentId, string? patientMessage)
        {
            var existingAppointment = _context.Appointments.Find(appointmentId);
            if (existingAppointment == null)
            {
                throw new Exception("Consulta não encontrada");
            }

            existingAppointment.PatientMessage = patientMessage;
            _context.SaveChanges();
            return existingAppointment;
        }
        public async Task<int> SaveAttachment(Attach attach)
        {
            _context.Attachments.Add(attach);
            await _context.SaveChangesAsync();
            return attach.AttachId;
        }
    }
}

