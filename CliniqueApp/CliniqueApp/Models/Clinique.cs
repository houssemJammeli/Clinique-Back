namespace CliniqueApp.Models
{
    public class Clinique
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Adresse { get; set; }
        public string Telephone { get; set; }

        public int AdminId { get; set; }
        public Admin Admin { get; set; }
        public ICollection<Receptionniste> Receptionnistes { get; set; }
        public ICollection<Medecin> Medecins { get; set; }

    }
}
