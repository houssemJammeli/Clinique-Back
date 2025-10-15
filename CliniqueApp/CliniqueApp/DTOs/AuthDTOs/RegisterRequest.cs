namespace CliniqueApp.DTOs.AuthDTOs
{
    public class RegisterRequest
    {
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; } // "Patient", "Medecin", "Admin", "Receptionniste"
    }
}
