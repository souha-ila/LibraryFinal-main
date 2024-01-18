using Microsoft.EntityFrameworkCore;

namespace library.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Livre> Livres { get; set; }
        public DbSet<Abonne> Abonnes { get; set; }
        public DbSet<Emprunt> Emprunts { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Blog> Blogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure Livre entity
            modelBuilder.Entity<Livre>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_Livre");
                entity.ToTable("Livres");
                entity.Property(e => e.Titre).HasMaxLength(50).IsUnicode(false);
                entity.Property(e => e.Auteur).HasMaxLength(50).IsUnicode(false);
                entity.Property(e => e.Resume).HasMaxLength(50).IsUnicode(false);
                entity.Property(e => e.EstEmprunte).HasMaxLength(50).IsUnicode(false);
            });

            // Configure Abonne entity
            modelBuilder.Entity<Abonne>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_Abonne");
                entity.ToTable("Abonnes");
                entity.Property(e => e.Nom).HasMaxLength(50).IsUnicode(false);
                entity.Property(e => e.Prenom).HasMaxLength(50).IsUnicode(false);
            });

            // Configure Emprunt entity
            modelBuilder.Entity<Emprunt>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_Emprunt");
                entity.ToTable("Emprunts");
                entity.Property(e => e.LivreId).HasMaxLength(50).IsUnicode(false);
                entity.Property(e => e.DateRetour).HasMaxLength(50).IsUnicode(false);
                entity.Property(e => e.DateEmprunt).HasMaxLength(50).IsUnicode(false);
                entity.Property(e => e.AbonneId).HasMaxLength(50).IsUnicode(false);
            });

            // Add additional configurations for other entities (Message, Blog, etc.) if needed

            // Call the base OnModelCreating method
            base.OnModelCreating(modelBuilder);
        }
    }
}
