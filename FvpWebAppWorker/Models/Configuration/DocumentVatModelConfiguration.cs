using FvpWebAppModels.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FvpWebAppWorker.Models.Configuration
{
    public class DocumentVatModelConfiguration : IEntityTypeConfiguration<DocumentVat>
    {
        public void Configure(EntityTypeBuilder<DocumentVat> modelBuilder)
        {
            modelBuilder.Property(p => p.VatAmount).HasColumnType("decimal(12,4)");
            modelBuilder.Property(p => p.VatValue).HasColumnType("decimal(4,2)");
            modelBuilder.Property(p => p.GrossAmount).HasColumnType("decimal(12,4)");
            modelBuilder.Property(p => p.NetAmount).HasColumnType("decimal(12,4)");
        }
    }
}