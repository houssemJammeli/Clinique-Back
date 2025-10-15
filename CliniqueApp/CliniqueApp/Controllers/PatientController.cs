using CliniqueApp.Context;
using CliniqueApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CliniqueApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly CliniqueDbContext _context;

        public PatientController(CliniqueDbContext context)
        {
            _context = context;
        }

        // 🔹 GET: api/patient
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Patient>>> GetPatients()
        {
            return await _context.Patients
                .Include(p => p.RendezVous) // Inclut les rendez-vous liés
                .ToListAsync();
        }

        // 🔹 GET: api/patient/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Patient>> GetPatient(int id)
        {
            var patient = await _context.Patients
                .Include(p => p.RendezVous)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (patient == null)
                return NotFound();

            return patient;
        }

        // 🔹 POST: api/patient
        [HttpPost]
        public async Task<IActionResult> CreatePatient([FromBody] Patient patient)
        {
            // Hasher le mot de passe
            var passwordHasher = new PasswordHasher<Patient>();
            patient.Password = passwordHasher.HashPassword(patient, patient.Password);

            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();
            return Ok(patient);
        }


        // 🔹 PUT: api/patient/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPatient(int id, [FromBody] Patient patient)
        {
            // Récupère le patient existant depuis la base
            var existingPatient = await _context.Patients.FindAsync(id);
            if (existingPatient == null)
                return NotFound();

            // Met à jour uniquement les champs modifiables
            existingPatient.Nom = patient.Nom;
            existingPatient.Prenom = patient.Prenom;
            existingPatient.Email = patient.Email;
            existingPatient.Password = patient.Password;
            existingPatient.DateNaissance = patient.DateNaissance;
            existingPatient.Telephone = patient.Telephone;

            // Sauvegarde les changements
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // 🔹 DELETE: api/patient/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePatient(int id)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null)
                return NotFound();

            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
