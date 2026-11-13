using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ter.Api.Models.Entities;

namespace Ter.Api.Data.Configurations;

public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        builder.Property(u => u.DateCreation).HasColumnName("date_creation");

        builder.Property(u => u.Statut)
            .HasColumnName("statut_compte")
            .HasConversion<string>()  
            .HasMaxLength(20);
    }
}