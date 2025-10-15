using CliniqueApp.Context;
using CliniqueApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CliniqueApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CliniqueController : ControllerBase
    {
        private readonly CliniqueDbContext _context;

        public CliniqueController(CliniqueDbContext context)
        {
            _context = context;
        }

        // GET all
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Clinique>>> GetCliniques()
        {
            return await _context.Cliniques
                .Include(c => c.Admin)
                .Include(c => c.Receptionnistes)
                .Include(c => c.Medecins)
                .ToListAsync();
        }

        // GET by id
        [HttpGet("{id}")]
        public async Task<ActionResult<Clinique>> GetClinique(int id)
        {
            var clinique = await _context.Cliniques
                .Include(c => c.Admin)
                .Include(c => c.Receptionnistes)
                .Include(c => c.Medecins)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (clinique == null) return NotFound();
            return clinique;
        }

        // POST
        [HttpPost]
        public async Task<IActionResult> CreateClinique([FromBody] Clinique clinique)
        {
            // Vérifie que l’Admin existe
            if (!await _context.Admins.AnyAsync(a => a.Id == clinique.AdminId))
                return BadRequest("Admin invalide");

            _context.Cliniques.Add(clinique);
            await _context.SaveChangesAsync();
            return Ok(clinique);
        }

        // PUT
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateClinique(int id, [FromBody] Clinique clinique)
        {
            var existing = await _context.Cliniques.FindAsync(id);
            if (existing == null) return NotFound();

            existing.Nom = clinique.Nom;
            existing.Adresse = clinique.Adresse;
            existing.Telephone = clinique.Telephone;
            existing.AdminId = clinique.AdminId;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClinique(int id)
        {
            var clinique = await _context.Cliniques.FindAsync(id);
            if (clinique == null) return NotFound();

            _context.Cliniques.Remove(clinique);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
