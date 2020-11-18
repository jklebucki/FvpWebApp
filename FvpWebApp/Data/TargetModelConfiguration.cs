using FvpWebAppModels.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FvpWebApp.Data
{
    public class TargetModelConfiguration : IEntityTypeConfiguration<Target>
    {
        public void Configure(EntityTypeBuilder<Target> builder)
        {
            builder.HasData(
                new Target
                {
                    TargetId = 1,
                    Descryption = "Citronex I - Symfonia ERP",
                    DatabaseName = "fkf_test_db",
                    DatabaseAddress = "192.168.21.20",
                    DatabaseUsername = "sa",
                    DatabasePassword = "#sa2015!"
                },
                new Target
                {
                    TargetId = 2,
                    Descryption = "Citronex MOP - Symfonia ERP",
                    DatabaseName = "fkf_",
                    DatabaseAddress = "192.168.21.20",
                    DatabaseUsername = "sa",
                    DatabasePassword = "#sa2015!"
                });
        }
    }
}
