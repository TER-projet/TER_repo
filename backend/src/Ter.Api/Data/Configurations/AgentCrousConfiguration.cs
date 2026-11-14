using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ter.Api.Models.Entities;

namespace Ter.Api.Data.Configurations;

public class AgentCrousConfiguration : IEntityTypeConfiguration<AgentCrous>
{
    public void Configure(EntityTypeBuilder<AgentCrous> builder)
    {
        builder.ToTable("agent_crous");

        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id).HasColumnName("id_agent_crous").ValueGeneratedOnAdd();

        builder.Property(a => a.Nom).HasColumnName("nom").IsRequired().HasMaxLength(100);
        builder.Property(a => a.Prenom).HasColumnName("prenom").IsRequired().HasMaxLength(100);
        builder.Property(a => a.CompteId).HasColumnName("id_compte").IsRequired();

        builder.HasIndex(a => a.CompteId).IsUnique();

        builder.HasOne(a => a.Compte)
            .WithOne(u => u.AgentCrous)
            .HasForeignKey<AgentCrous>(a => a.CompteId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
