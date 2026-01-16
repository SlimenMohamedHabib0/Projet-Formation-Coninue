using FormationContinue.Models;
using Microsoft.EntityFrameworkCore;

namespace FormationContinue.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(u => u.Email).IsUnique();
                entity.Property(u => u.FullName).HasMaxLength(64).IsRequired();
                entity.Property(u => u.Email).HasMaxLength(120).IsRequired();
                entity.Property(u => u.Role).HasMaxLength(20).IsRequired();
                entity.Property(u => u.PasswordHash).HasMaxLength(400).IsRequired();
            });
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasIndex(u => u.Libelle).IsUnique();
                entity.Property(u => u.Libelle).HasMaxLength(100).IsRequired();
               
            });

        }
    }
}
