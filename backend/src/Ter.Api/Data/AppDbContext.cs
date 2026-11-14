using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Ter.Api.Models.Entities;

namespace Ter.Api.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options)
    : IdentityDbContext<AppUser, IdentityRole<Guid>, Guid>(options)
{
    // Ajoutez ici un DbSet par entité (une ligne par entité).
    public DbSet<Etudiant> Etudiants => Set<Etudiant>();
    public DbSet<Personnel> Personnels => Set<Personnel>();
    public DbSet<AgentCrous> AgentsCrous => Set<AgentCrous>();
    public DbSet<Session> Sessions => Set<Session>();
    public DbSet<Administrateur> Administrateurs => Set<Administrateur>();
    public DbSet<Administrer> Administrer => Set<Administrer>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // OBLIGATOIRE en premier : configure les tables d'Identity
        base.OnModelCreating(modelBuilder);

        // Noms de tables conformes aux conventions du projet (minuscules, snake_case)
        modelBuilder.Entity<AppUser>().ToTable("comptes");
        modelBuilder.Entity<IdentityRole<Guid>>().ToTable("roles");
        modelBuilder.Entity<IdentityUserRole<Guid>>().ToTable("compte_roles");
        modelBuilder.Entity<IdentityUserClaim<Guid>>().ToTable("compte_claims");
        modelBuilder.Entity<IdentityUserLogin<Guid>>().ToTable("compte_logins");
        modelBuilder.Entity<IdentityUserToken<Guid>>().ToTable("compte_tokens");
        modelBuilder.Entity<IdentityRoleClaim<Guid>>().ToTable("role_claims");

        // Charge automatiquement toutes les classes IEntityTypeConfiguration<T>
        // (dossier Data/Configurations) : rien à enregistrer à la main.
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}