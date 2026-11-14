using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ter.Api.Models.Entities;

namespace Ter.Api.Data.Configurations;

public class AdministrerConfiguration : IEntityTypeConfiguration<Administrer>
{
    public void Configure(EntityTypeBuilder<Administrer> builder)
    {
        builder.ToTable("administrer");

        builder.HasKey(a => new { a.AdministrateurId, a.CompteId });

        builder.Property(a => a.AdministrateurId).HasColumnName("id_administrateur");
        builder.Property(a => a.CompteId).HasColumnName("id_compte");

        builder.HasIndex(a => a.CompteId);

        // Administrateur -> comptes qu'il gère
        builder.HasOne(a => a.Administrateur)
            .WithMany(adm => adm.ComptesAdministres)
            .HasForeignKey(a => a.AdministrateurId)
            .OnDelete(DeleteBehavior.Cascade);

        // Compte -> administrateurs qui le gèrent
        builder.HasOne(a => a.Compte)
            .WithMany(u => u.AdministresPar)
            .HasForeignKey(a => a.CompteId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
