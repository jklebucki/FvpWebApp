using FvpWebAppModels.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace FvpWebApp.Data
{
    public class TaskTicketModelConfiguration : IEntityTypeConfiguration<TaskTicket>
    {
        public void Configure(EntityTypeBuilder<TaskTicket> builder)
        {
            builder.HasData(
                new TaskTicket
                {
                    TaskTicketId = 1,
                    SourceId = 1,
                    DateFrom = new DateTime(2020, 9, 1),
                    DateTo = new DateTime(2020, 9, 30),
                    TicketStatus = TicketStatus.Added,
                    TicketType = TicketType.ImportDocuments,
                    CreatedAt = new DateTime(2020, 1, 1),
                },
                new TaskTicket
                {
                    TaskTicketId = 2,
                    SourceId = 2,
                    DateFrom = new DateTime(2020, 9, 1),
                    DateTo = new DateTime(2020, 9, 30),
                    TicketStatus = TicketStatus.Added,
                    TicketType = TicketType.ImportDocuments,
                    CreatedAt = new DateTime(2020, 1, 1),
                });
        }
    }
}
