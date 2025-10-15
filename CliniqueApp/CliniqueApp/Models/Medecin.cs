namespace CliniqueApp.Models
{
    public class Medecin : Utilisateur
    {
        public decimal Salaire { get; set; }
        public string Specialite { get; set; }

        public ICollection<Consultation> Consultations { get; set; } 

        public Clinique Clinique { get; set; }
        public int? CliniqueId { get; set; }
    }
}
