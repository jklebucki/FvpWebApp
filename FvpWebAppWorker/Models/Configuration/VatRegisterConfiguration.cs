using FvpWebAppModels.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FvpWebAppWorker.Models.Configuration
{
    public class VatRegisterConfiguration : IEntityTypeConfiguration<VatRegister>
    {
        public void Configure(EntityTypeBuilder<VatRegister> modelBuilder)
        {
            modelBuilder.Property(p => p.VatValue).HasColumnType("decimal(12,4)");
            modelBuilder.HasIndex(p => p.VatValue)
                        .HasName("IX_VatValue")
                        .IsUnique();
        }

    }
}