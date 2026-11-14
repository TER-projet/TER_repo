using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ter.Api.Models.Entities;

namespace Ter.Api.Data.Configurations;

public class PersonnelConfiguration : IEntityTypeConfiguration<Personnel>
{
    public void Configure(EntityTypeBuilder<Personnel> builder)
    {
        builder.ToTable("personnel");

        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).HasColumnName("id_personnel").ValueGeneratedOnAdd();

        builder.Property(p => p.Nom).HasColumnName("nom").IsRequired().HasMaxLength(100);
        builder.Property(p => p.Prenom).HasColumnName("prenom").IsRequired().HasMaxLength(100);
        builder.Property(p => p.Fonction).HasColumnName("fonction").HasMaxLength(100);
        builder.Property(p => p.CompteId).HasColumnName("id_compte").IsRequired();

        builder.HasIndex(p => p.CompteId).IsUnique();

        builder.HasOne(p => p.Compte)
            .WithOne(u => u.Personnel)
            .HasForeignKey<Personnel>(p => p.CompteId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
