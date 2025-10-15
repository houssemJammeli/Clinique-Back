using CliniqueApp.Context;
using CliniqueApp.DTOs.RendezVousDTOs;
using CliniqueApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CliniqueApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RendezVousController : ControllerBase
    {
        private readonly CliniqueDbContext _context;

        public RendezVousController(CliniqueDbContext context)
        {
            _context = context;
        }

        // 🔹 GET : api/RendezVous
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ListRendezVousDTO>>> GetRendezVous()
        {
            return await _context.RendezVous
                .Include(r => r.Patient)
                .Include(r => r.Receptionniste)
                .Include(r => r.Medecin) 
                .Select(r => new ListRendezVousDTO()
                {
                    Id = r.Id,
                    DateHeure = r.DateHeure,
                    Statut = (int?) r.Statut,
                    MedecinId = r.MedecinId,
                    MedecinNom = r.Medecin!=null? r.Medecin.Nom: "Médecin non assigné",
                    PatientId = r.PatientId,
                    PatientNom = r.Patient!=null? r.Patient.Nom: "Patient inconnu",
                    ReceptionnisteId = r.ReceptionnisteId,
                    ReceptionnisteNom = r.Receptionniste!=null? r.Receptionniste.Nom: "Réceptionniste inconnu"
                })
                .ToListAsync();
        }

        // 🔹 GET : api/RendezVous/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RendezVous>> GetRendezVous(int id)
        {
            var rdv = await _context.RendezVous
                .Include(r => r.Patient)
                .Include(r => r.Receptionniste)
                .Include(r => r.Medecin) // <- inclure le médecin
                .FirstOrDefaultAsync(r => r.Id == id);

            if (rdv == null) return NotFound();
            return rdv;
        }

        // 🔹 GET : api/RendezVous/patient/5
        [HttpGet("patient/{patientId}")]
        public async Task<ActionResult<IEnumerable<ListRendezVousDTO>>> GetByPatient(int patientId)
        {
            var rdvs = await _context.RendezVous
                .Where(r => r.PatientId == patientId)
                .Include(r => r.Patient)
                .Include(r => r.Medecin)
                .Include(r => r.Receptionniste)
                .Select(r => new ListRendezVousDTO()
                {
                    Id = r.Id,
                    DateHeure = r.DateHeure,
                    Statut = (int?)r.Statut,
                    MedecinId = r.MedecinId,
                    MedecinNom = r.Medecin != null ? r.Medecin.Nom : "Médecin non assigné",
                    PatientId = r.PatientId,
                    PatientNom = r.Patient != null ? r.Patient.Nom : "Patient inconnu",
                    ReceptionnisteId = r.ReceptionnisteId,
                    ReceptionnisteNom = r.Receptionniste != null ? r.Receptionniste.Nom : "Réceptionniste inconnu"
                })
                .ToListAsync();

            

            return (rdvs);
        }

        // 🔹 POST : api/RendezVous
        [HttpPost]
        public async Task<IActionResult> CreateRendezVous([FromBody] RendezVous rendezVous)
        {
            if (!await _context.Patients.AnyAsync(p => p.Id == rendezVous.PatientId))
                return BadRequest("Patient invalide");

            // Vérification seulement si un réceptionnisteId est fourni

            if (!rendezVous.ReceptionnisteId.HasValue ||( rendezVous.ReceptionnisteId.HasValue &&
                !await _context.Receptionnistes.AnyAsync(r => r.Id == rendezVous.ReceptionnisteId.Value)))
                return BadRequest("Receptionniste invalide");

            if (rendezVous.MedecinId.HasValue &&
                !await _context.Medecins.AnyAsync(m => m.Id == rendezVous.MedecinId.Value))
                return BadRequest("Medecin invalide");

            _context.RendezVous.Add(rendezVous);
            await _context.SaveChangesAsync();
            return Ok(rendezVous);
        }


        // 🔹 PUT : api/RendezVous/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRendezVous(int id, [FromBody] RendezVous rdv)
        {
            var existingRdv = await _context.RendezVous.FindAsync(id);
            if (existingRdv == null)
                return NotFound();

            bool wasConfirmedBefore = existingRdv.Statut == StatutRV.Confirmé;

            // 🔹 Mise à jour conditionnelle
            if (rdv.Statut.HasValue)
                existingRdv.Statut = rdv.Statut.Value;

            if (rdv.DateHeure != default)
                existingRdv.DateHeure = rdv.DateHeure;

            if (rdv.PatientId != 0)
                existingRdv.PatientId = rdv.PatientId;

            if (rdv.ReceptionnisteId != 0)
                existingRdv.ReceptionnisteId = rdv.ReceptionnisteId;

            if (rdv.MedecinId.HasValue)
                existingRdv.MedecinId = rdv.MedecinId;

            await _context.SaveChangesAsync();

            // 🩺 Création automatique d'une consultation si confirmé et pas déjà fait
            if (existingRdv.Statut == StatutRV.Confirmé && !wasConfirmedBefore)
            {
                var consultation = new Consultation
                {
                    RendezVousId = existingRdv.Id,
                    Rapport = "",
                    MedecinId = existingRdv.MedecinId,
                    Facture = null,
                    Ordonnance = null
                };

                _context.Consultations.Add(consultation);
                await _context.SaveChangesAsync();
            }

            return Ok(existingRdv);
        }




        // 🔹 DELETE : api/RendezVous/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRendezVous(int id)
        {
            var rdv = await _context.RendezVous.FindAsync(id);
            if (rdv == null)
                return NotFound();

            _context.RendezVous.Remove(rdv);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // GET : api/RendezVous/medecin/5
        [HttpGet("medecin/{medecinId}")]
        public async Task<ActionResult<IEnumerable<ListRendezVousDTO>>> GetByMedecin(int medecinId)
        {
            var rdvs = await _context.RendezVous
                .Where(r => r.MedecinId.HasValue && r.MedecinId.Value == medecinId)
                .Include(r => r.Patient)
                .Include(r => r.Receptionniste)
                .Select(r => new ListRendezVousDTO()
                {
                    Id = r.Id,
                    DateHeure = r.DateHeure,
                    Statut = (int?)r.Statut,
                    MedecinId = r.MedecinId,
                    MedecinNom = r.Medecin != null ? r.Medecin.Nom : "Médecin non assigné",
                    PatientId = r.PatientId,
                    PatientNom = r.Patient != null ? r.Patient.Nom : "Patient inconnu",
                    ReceptionnisteId = r.ReceptionnisteId,
                    ReceptionnisteNom = r.Receptionniste != null ? r.Receptionniste.Nom : "Réceptionniste inconnu"
                })
                .ToListAsync();

            return (rdvs); // retourne toujours un tableau, même vide
        }

        // PUT : api/RendezVous/5/statut
        [HttpPut("{id}/statut")]
        public async Task<IActionResult> UpdateStatut(int id, [FromBody] int statut)
        {
            var rdv = await _context.RendezVous.FindAsync(id);
            if (rdv == null) return NotFound();

            rdv.Statut = (StatutRV)statut;  // 0 = en attente, 1 = accepté, 2 = refusé
            await _context.SaveChangesAsync();
            return Ok(rdv);
        }

    }
}
