namespace CliniqueApp.Models
{
    public class Paiement
    {
        public int Id { get; set; }
        public decimal Montant { get; set; }
        public DateTime Date { get; set; }

        public int FactureId { get; set; }
        public Facture? Facture { get; set; }
    }
}
