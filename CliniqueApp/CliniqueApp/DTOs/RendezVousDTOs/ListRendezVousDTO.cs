namespace CliniqueApp.DTOs.RendezVousDTOs
{
    public class ListRendezVousDTO
    {
        public int Id { get; set; }

        public DateTime DateHeure { get; set; }

        // 0 = EnAttente, 1 = Confirmé, 2 = Annulé
        public int? Statut { get; set; }

        public int PatientId { get; set; }
        public string? PatientNom { get; set; }

        public int? ReceptionnisteId { get; set; }
        public string? ReceptionnisteNom { get; set; }

        public int? MedecinId { get; set; }
        public string? MedecinNom { get; set; }


    }
}
