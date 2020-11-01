using FvpWebAppModels.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FvpWebApp.Data
{
    public class TargetDocumentSettingsConfiguration : IEntityTypeConfiguration<TargetDocumentSettings>
    {
        public void Configure(EntityTypeBuilder<TargetDocumentSettings> builder)
        {
            builder.HasData(
                new TargetDocumentSettings
                {
                    TargetDocumentSettingsId = 1,
                    SourceId = 1,
                    DocumentShortcut = "FDZG",
                    VatRegisterId = 1
                },
                new TargetDocumentSettings
                {
                    TargetDocumentSettingsId = 2,
                    SourceId = 2,
                    DocumentShortcut = "FDD2",
                    VatRegisterId = 1
                }
            );
        }
    }
}
