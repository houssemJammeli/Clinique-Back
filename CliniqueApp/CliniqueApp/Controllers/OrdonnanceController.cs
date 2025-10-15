using CliniqueApp.Context;
using CliniqueApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CliniqueApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdonnanceController : ControllerBase
    {
        private readonly CliniqueDbContext _context;

        public OrdonnanceController(CliniqueDbContext context)
        {
            _context = context;
        }

        // 🔹 GET : api/Ordonnance
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ordonnance>>> GetAll()
        {
            return await _context.Ordonnances
                .Include(o => o.Consultation)
                .ToListAsync();
        }

        // 🔹 GET : api/Ordonnance/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Ordonnance>> GetById(int id)
        {
            var ordonnance = await _context.Ordonnances
                .Include(o => o.Consultation)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (ordonnance == null) return NotFound();

            return ordonnance;
        }

        // 🔹 POST : api/Ordonnance
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Ordonnance ordonnance)
        {
            // Vérifier que la consultation existe
            var consultation = await _context.Consultations.FindAsync(ordonnance.ConsultationId);
            if (consultation == null)
                return BadRequest("Consultation non trouvée.");

            ordonnance.Date = DateTime.Now; // Date actuelle
            _context.Ordonnances.Add(ordonnance);
            await _context.SaveChangesAsync();

            // Lier l'ordonnance à la consultation
            consultation.Ordonnance = ordonnance;
            await _context.SaveChangesAsync();

            return Ok(ordonnance);
        }

        // 🔹 PUT : api/Ordonnance/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Ordonnance updated)
        {
            var ordonnance = await _context.Ordonnances.FindAsync(id);
            if (ordonnance == null) return NotFound();

            if (!string.IsNullOrEmpty(updated.Contenu))
                ordonnance.Contenu = updated.Contenu;

            await _context.SaveChangesAsync();
            return Ok(ordonnance);
        }

        // 🔹 DELETE : api/Ordonnance/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ordonnance = await _context.Ordonnances.FindAsync(id);
            if (ordonnance == null) return NotFound();

            _context.Ordonnances.Remove(ordonnance);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
