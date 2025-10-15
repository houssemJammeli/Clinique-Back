using CliniqueApp.Models;
using Microsoft.EntityFrameworkCore;

namespace CliniqueApp.Context
{
    public class CliniqueDbContext : DbContext
    {
        public DbSet<Utilisateur> Utilisateurs { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Medecin> Medecins { get; set; }
        public DbSet<Receptionniste> Receptionnistes { get; set; }
        public DbSet<Patient> Patients { get; set; }

        public DbSet<Clinique> Cliniques { get; set; }
        public DbSet<RendezVous> RendezVous { get; set; }
        public DbSet<Consultation> Consultations { get; set; }
        public DbSet<Facture> Factures { get; set; }
        public DbSet<Paiement> Paiements { get; set; }
        public DbSet<Ordonnance> Ordonnances { get; set; }

        public CliniqueDbContext(DbContextOptions<CliniqueDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Pas besoin de Role ou d’Enum, EF crée une colonne Discriminator
            modelBuilder.Entity<Utilisateur>()
                .HasDiscriminator<string>("Discriminator")
                .HasValue<Admin>("Admin")
                .HasValue<Medecin>("Medecin")
                .HasValue<Receptionniste>("Receptionniste")
                .HasValue<Patient>("Patient");
        }
    }
}
