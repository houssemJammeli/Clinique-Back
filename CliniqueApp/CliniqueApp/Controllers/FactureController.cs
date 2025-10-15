using CliniqueApp.Context;
using CliniqueApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CliniqueApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FactureController : ControllerBase
    {
        private readonly CliniqueDbContext _context;

        public FactureController(CliniqueDbContext context)
        {
            _context = context;
        }

        // 🔹 GET : api/Facture
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Facture>>> GetFactures()
        {
            return await _context.Factures
                .Include(f => f.Consultation)
                .Include(f => f.Paiement)
                .ToListAsync();
        }

        // 🔹 GET : api/Facture/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Facture>> GetFacture(int id)
        {
            var facture = await _context.Factures
                .Include(f => f.Consultation)
                .Include(f => f.Paiement)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (facture == null)
                return NotFound();

            return facture;
        }

        // 🔹 POST : api/Facture
        [HttpPost]
        public async Task<IActionResult> CreateFacture([FromBody] Facture facture)
        {
            _context.Factures.Add(facture);
            await _context.SaveChangesAsync();
            return Ok(facture);
        }

        // 🔹 PUT : api/Facture/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFacture(int id, [FromBody] Facture updated)
        {
            var facture = await _context.Factures.FindAsync(id);
            if (facture == null) return NotFound();

            if (updated.Montant != 0)
                facture.Montant = updated.Montant;

            facture.StatutPaiement = updated.StatutPaiement;

            if (updated.ConsultationId != 0)
                facture.ConsultationId = updated.ConsultationId;

            if (updated.Paiement != null)
                facture.Paiement = updated.Paiement;

            await _context.SaveChangesAsync();
            return Ok(facture);
        }

        // 🔹 DELETE : api/Facture/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFacture(int id)
        {
            var facture = await _context.Factures.FindAsync(id);
            if (facture == null)
                return NotFound();

            _context.Factures.Remove(facture);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
