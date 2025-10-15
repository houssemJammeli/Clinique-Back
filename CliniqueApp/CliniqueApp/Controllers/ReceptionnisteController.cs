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
    public class ReceptionnisteController : ControllerBase
    {
        private readonly CliniqueDbContext _context;
        private readonly PasswordHasher<Receptionniste> _passwordHasher;

        public ReceptionnisteController(CliniqueDbContext context)
        {
            _context = context;
            _passwordHasher = new PasswordHasher<Receptionniste>();
        }

        // 🔹 GET all
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Receptionniste>>> GetReceptionnistes()
        {
            return await _context.Receptionnistes
                .Include(r => r.Clinique)
                .Include(r => r.RendezVous)
                .ToListAsync();
        }

        // 🔹 GET by id
        [HttpGet("{id}")]
        public async Task<ActionResult<Receptionniste>> GetReceptionniste(int id)
        {
            var recep = await _context.Receptionnistes
                .Include(r => r.Clinique)
                .Include(r => r.RendezVous)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (recep == null) return NotFound();
            return recep;
        }

        // 🔹 POST
        [HttpPost]
        public async Task<IActionResult> CreateReceptionniste([FromBody] Receptionniste recep)
        {
            // Vérifie CliniqueId
            if (!await _context.Cliniques.AnyAsync(c => c.Id == recep.CliniqueId))
                return BadRequest("Clinique invalide");

            // Hash du mot de passe
            recep.Password = _passwordHasher.HashPassword(recep, recep.Password);

            _context.Receptionnistes.Add(recep);
            await _context.SaveChangesAsync();
            return Ok(recep);
        }

        // 🔹 PUT
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReceptionniste(int id, [FromBody] Receptionniste recep)
        {
            var existing = await _context.Receptionnistes.FindAsync(id);
            if (existing == null) return NotFound();

            existing.Nom = recep.Nom;
            existing.Prenom = recep.Prenom;
            existing.Email = recep.Email;
            existing.Password = _passwordHasher.HashPassword(existing, recep.Password);
            existing.Salaire = recep.Salaire;
            existing.CliniqueId = recep.CliniqueId;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // 🔹 DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReceptionniste(int id)
        {
            var recep = await _context.Receptionnistes.FindAsync(id);
            if (recep == null) return NotFound();

            _context.Receptionnistes.Remove(recep);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
