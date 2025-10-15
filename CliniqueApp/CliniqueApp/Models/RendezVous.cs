using System.Text.Json.Serialization;

namespace CliniqueApp.Models
{
    public class RendezVous
    {
        public int Id { get; set; }
        public DateTime DateHeure { get; set; }
        public StatutRV? Statut { get; set; } 

        public int PatientId { get; set; }
        

        public Patient? Patient { get; set; }

        public int? ReceptionnisteId { get; set; }
        

        public Receptionniste? Receptionniste { get; set; }
        public int? MedecinId { get; set; }
        
        public Medecin? Medecin { get; set; }
    }
}
