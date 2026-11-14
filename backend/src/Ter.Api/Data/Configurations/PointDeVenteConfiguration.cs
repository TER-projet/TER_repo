
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ter.Api.Models.Entities;

namespace Ter.Api.Data.Configurations;

public class PointDeVenteConfiguration : IEntityTypeConfiguration<PointDeVente>
{
    public void Configure(EntityTypeBuilder<PointDeVente> builder)
    {
        builder.ToTable("points_de_vente");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasColumnName("id_point_de_vente");

        builder.Property(p => p.Nom)
            .HasColumnName("nom")
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(p => p.Adresse)
            .HasColumnName("adresse")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(p => p.Latitude)
            .HasColumnName("latitude")
            .HasPrecision(9, 6);

        builder.Property(p => p.Longitude)
            .HasColumnName("longitude")
            .HasPrecision(9, 6);

        builder.Property(p => p.Telephone)
            .HasColumnName("telephone")
            .HasMaxLength(30);

        builder.Property(p => p.CapaciteAccueil)
            .HasColumnName("capacite_accueil");

        builder.Property(p => p.Statut)
            .HasColumnName("statut")
            .HasMaxLength(20)
            .IsRequired();
    }
}
