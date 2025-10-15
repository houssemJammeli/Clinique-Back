namespace CliniqueApp.Models
{
    public class Receptionniste : Utilisateur
    {
        public decimal Salaire { get; set; }



        public ICollection<RendezVous> RendezVous { get; set; }
        public Clinique Clinique { get; set; }
        public int? CliniqueId { get; set; }
    }
}
