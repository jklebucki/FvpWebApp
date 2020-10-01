using FvpWebApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FvpWebApp.Data
{
    public class DocumentModelConfiguration : IEntityTypeConfiguration<Document>
    {
        public void Configure(EntityTypeBuilder<Document> modelBuilder)
        {
            modelBuilder.Property(p => p.Gross).HasColumnType("decimal(12,4)");
            modelBuilder.Property(p => p.Net).HasColumnType("decimal(12,4)");
            modelBuilder.Property(p => p.Vat).HasColumnType("decimal(12,4)");
        }

    }
}