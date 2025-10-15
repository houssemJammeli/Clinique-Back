using CliniqueApp.Context;
using CliniqueApp.DTOs.ConsultationDTOs;
using CliniqueApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CliniqueApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConsultationController : ControllerBase
    {
        private readonly CliniqueDbContext _context;

        public ConsultationController(CliniqueDbContext context)
        {
            _context = context;
        }

        // 🔹 GET : api/Consultation
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Consultation>>> GetConsultations()
        {
            return await _context.Consultations
                .Include(c => c.RendezVous)
                .ThenInclude(r => r.Patient)
                .Include(c => c.RendezVous)
                .ThenInclude(r => r.Medecin)
                .ToListAsync();
        }

        // 🔹 GET : api/Consultation/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Consultation>> GetConsultation(int id)
        {
            var consultation = await _context.Consultations
                .Include(c => c.RendezVous)
                .ThenInclude(r => r.Patient)
                .Include(c => c.RendezVous)
                .ThenInclude(r => r.Medecin)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (consultation == null) return NotFound();

            return consultation;
        }

        // 🔹 POST : api/Consultation
        [HttpPost]
        public async Task<IActionResult> CreateConsultation([FromBody] Consultation consultation)
        {
            _context.Consultations.Add(consultation);
            await _context.SaveChangesAsync();
            return Ok(consultation);
        }

        // 🔹 PUT : api/Consultation/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateConsultation(int id, [FromBody] Consultation updated)
        {
            var consultation = await _context.Consultations.FindAsync(id);
            if (consultation == null) return NotFound();

            if (!string.IsNullOrEmpty(updated.Rapport))
                consultation.Rapport = updated.Rapport;

            if (updated.Facture != null)
                consultation.Facture = updated.Facture;

            if (updated.Ordonnance != null)
                consultation.Ordonnance = updated.Ordonnance;

            if (updated.RendezVousId != 0)
                consultation.RendezVousId = updated.RendezVousId;

            await _context.SaveChangesAsync();
            return Ok(consultation);
        }

        [HttpPut("{id}/rapport")]
        public async Task<IActionResult> UpdateConsultationRapport(int id, [FromBody] UpdateRapportDto dto)
        {
            var consultation = await _context.Consultations.FindAsync(id);
            if (consultation == null)
                return NotFound();

            // 🔹 Mettre à jour uniquement le rapport
            if (!string.IsNullOrWhiteSpace(dto.Rapport))
                consultation.Rapport = dto.Rapport;

            await _context.SaveChangesAsync();
            return Ok(consultation);
        }

        // 🔹 DELETE : api/Consultation/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteConsultation(int id)
        {
            var consultation = await _context.Consultations.FindAsync(id);
            if (consultation == null) return NotFound();

            _context.Consultations.Remove(consultation);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
