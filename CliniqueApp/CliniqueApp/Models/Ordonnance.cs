namespace CliniqueApp.Models
{
    public class Ordonnance
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Contenu { get; set; }

        public int ConsultationId { get; set; }
        public Consultation? Consultation { get; set; }
    }
}
