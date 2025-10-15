namespace CliniqueApp.Models
{
    public class Admin : Utilisateur
    {
        public ICollection<Clinique> Cliniques { get; set; }
    }
}
