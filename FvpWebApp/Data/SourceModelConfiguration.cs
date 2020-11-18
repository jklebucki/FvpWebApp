using FvpWebAppModels.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FvpWebApp.Data
{
    public class SourceModelConfiguration : IEntityTypeConfiguration<Source>
    {
        public void Configure(EntityTypeBuilder<Source> modelBuilder)
        {
            modelBuilder.HasData(
                new Source
                {
                    SourceId = 1,
                    Description = "Dyskont Paliwowy DP1",
                    Code = "DP1",
                    Type = "oracle_sben_dp",
                    Address = "192.168.42.70",
                    DbName = "sben",
                    Username = "sben",
                    Password = "almarwinnet",
                    TargetId = 1

                },
                new Source
                {
                    SourceId = 2,
                    Description = "Dyskont Paliwowy DP2",
                    Code = "DP2",
                    Type = "oracle_sben_dp",
                    Address = "192.168.45.70",
                    DbName = "sben",
                    Username = "sben",
                    Password = "almarwinnet",
                    TargetId = 1
                },
                new Source
                {
                    SourceId = 3,
                    Description = "Stacja Paliw BP Mostki",
                    Code = "BPMOSTKI",
                    Type = "bp_flat_file",
                    Address = @"I:\DaneBP\MOSTKI",
                    DbName = "",
                    Username = "",
                    Password = "",
                    TargetId = 1
                }
           );
        }
    }
}