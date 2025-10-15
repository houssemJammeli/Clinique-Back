namespace CliniqueApp.Models
{
    public class Facture
    {
        public int Id { get; set; }
        public decimal Montant { get; set; }
        public StatutP StatutPaiement { get; set; } 

        public int ConsultationId { get; set; }
        public Consultation? Consultation { get; set; }

        public Paiement? Paiement { get; set; }
    }
}
