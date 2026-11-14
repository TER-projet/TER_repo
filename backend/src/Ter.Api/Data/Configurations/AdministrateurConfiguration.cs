using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ter.Api.Models.Entities;

namespace Ter.Api.Data.Configurations;

public class AdministrateurConfiguration : IEntityTypeConfiguration<Administrateur>
{
    public void Configure(EntityTypeBuilder<Administrateur> builder)
    {
        builder.ToTable("administrateur");

        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id).HasColumnName("id_administrateur").ValueGeneratedOnAdd();

        builder.Property(a => a.Nom).HasColumnName("nom").IsRequired().HasMaxLength(100);
        builder.Property(a => a.Prenom).HasColumnName("prenom").IsRequired().HasMaxLength(100);
        builder.Property(a => a.CompteId).HasColumnName("id_compte").IsRequired();

        builder.HasIndex(a => a.CompteId).IsUnique();

        // Profil administrateur <-> compte de connexion (one-to-one, FK côté profil)
        builder.HasOne(a => a.Compte)
            .WithOne(u => u.Administrateur)
            .HasForeignKey<Administrateur>(a => a.CompteId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
