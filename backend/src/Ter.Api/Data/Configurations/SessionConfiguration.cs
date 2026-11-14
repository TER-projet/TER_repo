using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ter.Api.Models.Entities;

namespace Ter.Api.Data.Configurations;

public class SessionConfiguration : IEntityTypeConfiguration<Session>
{
    public void Configure(EntityTypeBuilder<Session> builder)
    {
        builder.ToTable("session");

        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).HasColumnName("id_session").ValueGeneratedOnAdd();

        builder.Property(s => s.DateDebut)
            .HasColumnName("date_debut")
            .HasColumnType("timestamp with time zone")
            .HasDefaultValueSql("now()");

        builder.Property(s => s.DateExpiration)
            .HasColumnName("date_expiration")
            .HasColumnType("timestamp with time zone")
            .IsRequired();

        builder.Property(s => s.CompteId).HasColumnName("id_compte").IsRequired();

        builder.HasIndex(s => s.CompteId);

        builder.HasOne(s => s.Compte)
            .WithMany(u => u.Sessions)
            .HasForeignKey(s => s.CompteId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
