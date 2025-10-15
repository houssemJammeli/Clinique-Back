using System.Text.RegularExpressions;

namespace CliniqueApp.Models
{
    public class Consultation 
    {
        public int Id { get; set; }

        public int RendezVousId { get; set; }
        public RendezVous RendezVous { get; set; }
        public int? MedecinId { get; set; }
        public Medecin? Medecin { get; set; }

        public string Rapport { get; set; }
        public Facture? Facture { get; set; }
        public Ordonnance? Ordonnance { get; set; }
    }
}
