using FvpWebAppModels.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FvpWebApp.Data
{
    public class VatRegisterConfiguration : IEntityTypeConfiguration<VatRegister>
    {
        public void Configure(EntityTypeBuilder<VatRegister> modelBuilder)
        {
            modelBuilder.Property(p => p.VatValue).HasColumnType("decimal(12,4)");
            modelBuilder.HasIndex(p => new { p.TargetDocumentSettingsId, p.VatValue })
                        .HasName("IX_VatValueOnTargetDocument")
                        .IsUnique();
        }

    }
}