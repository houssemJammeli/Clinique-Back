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
    public class MedecinController : ControllerBase
    {
        private readonly CliniqueDbContext _context;
        private readonly PasswordHasher<Medecin> _passwordHasher;

        public MedecinController(CliniqueDbContext context)
        {
            _context = context;
            _passwordHasher = new PasswordHasher<Medecin>();
        }

        // 🔹 GET all
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Medecin>>> GetMedecins()
        {
            return await _context.Medecins
                .Include(m => m.Clinique)
                .Include(m => m.Consultations)
                .ToListAsync();
        }

        // 🔹 GET by id
        [HttpGet("{id}")]
        public async Task<ActionResult<Medecin>> GetMedecin(int id)
        {
            var medecin = await _context.Medecins
                .Include(m => m.Clinique)
                .Include(m => m.Consultations)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (medecin == null) return NotFound();
            return medecin;
        }

        // 🔹 POST (Créer un médecin)
        [HttpPost]
        public async Task<IActionResult> CreateMedecin([FromBody] Medecin medecin)
        {
            // Vérifie que la clinique existe
            if (!await _context.Cliniques.AnyAsync(c => c.Id == medecin.CliniqueId))
                return BadRequest("Clinique invalide");

            // Hash du mot de passe
            medecin.Password = _passwordHasher.HashPassword(medecin, medecin.Password);

            _context.Medecins.Add(medecin);
            await _context.SaveChangesAsync();
            return Ok(medecin);
        }

        // 🔹 PUT (Modifier un médecin)
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMedecin(int id, [FromBody] Medecin medecin)
        {
            var existing = await _context.Medecins.FindAsync(id);
            if (existing == null) return NotFound();

            existing.Nom = medecin.Nom;
            existing.Prenom = medecin.Prenom;
            existing.Email = medecin.Email;
            existing.Password = _passwordHasher.HashPassword(existing, medecin.Password);
            existing.Specialite = medecin.Specialite;
            existing.CliniqueId = medecin.CliniqueId;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // 🔹 DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMedecin(int id)
        {
            var medecin = await _context.Medecins.FindAsync(id);
            if (medecin == null) return NotFound();

            _context.Medecins.Remove(medecin);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
