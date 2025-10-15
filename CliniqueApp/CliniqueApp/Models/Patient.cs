using System;
using System.Text.Json.Serialization;

namespace CliniqueApp.Models
{
    public class Patient : Utilisateur
    {
        public DateTime DateNaissance { get; set; }
        public string Telephone { get; set; }
        [JsonIgnore]
        public ICollection<RendezVous>? RendezVous { get; set; }
    }
}
