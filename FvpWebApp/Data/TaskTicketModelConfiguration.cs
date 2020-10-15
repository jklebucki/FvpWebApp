using FvpWebAppModels.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
                    CreatedAt = DateTime.Now
                },
                new TaskTicket
                {
                    TaskTicketId = 2,
                    SourceId = 2,
                    DateFrom = new DateTime(2020, 9, 1),
                    DateTo = new DateTime(2020, 9, 30),
                    TicketStatus = TicketStatus.Added,
                    CreatedAt = DateTime.Now
                });
        }
    }
}
