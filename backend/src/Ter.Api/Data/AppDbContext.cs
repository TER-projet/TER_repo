using Microsoft.EntityFrameworkCore;

namespace Ter.Api.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    // Ajoutez ici un DbSet par entité (une ligne par entité). Exemple :
    // public DbSet<Article> Articles => Set<Article>();
    //
    // En cas de conflit Git sur ce fichier, gardez simplement TOUTES les lignes des deux côtés.

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Charge automatiquement toutes les classes IEntityTypeConfiguration<T>
        // (dossier Data/Configurations) : rien à enregistrer à la main.
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}