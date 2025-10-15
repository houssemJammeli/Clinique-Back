namespace CliniqueApp.DTOs.AuthDTOs
{
    public class AuthResponse
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public string Role { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
    }
}
