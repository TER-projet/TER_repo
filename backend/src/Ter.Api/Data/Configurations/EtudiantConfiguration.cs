using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ter.Api.Models.Entities;

namespace Ter.Api.Data.Configurations;

public class EtudiantConfiguration : IEntityTypeConfiguration<Etudiant>
{
    public void Configure(EntityTypeBuilder<Etudiant> builder)
    {
        builder.ToTable("etudiant");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("id_etudiant").ValueGeneratedOnAdd();

        builder.Property(e => e.Nom).HasColumnName("nom").IsRequired().HasMaxLength(100);
        builder.Property(e => e.Prenom).HasColumnName("prenom").IsRequired().HasMaxLength(100);
        builder.Property(e => e.EstBoursier).HasColumnName("est_boursier").HasDefaultValue(false);
        builder.Property(e => e.Classe).HasColumnName("classe").HasMaxLength(100);
        builder.Property(e => e.CompteId).HasColumnName("id_compte").IsRequired();

        builder.HasIndex(e => e.CompteId).IsUnique();

        builder.HasOne(e => e.Compte)
            .WithOne(u => u.Etudiant)
            .HasForeignKey<Etudiant>(e => e.CompteId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
