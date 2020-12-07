using FvpWebAppModels.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FvpWebApp.Data
{
    public class TaskTicketModelConfiguration : IEntityTypeConfiguration<TaskTicket>
    {
        public void Configure(EntityTypeBuilder<TaskTicket> builder)
        {
            //    builder.HasData(
            //        new TaskTicket
            //        {
            //            TaskTicketId = 1,
            //            SourceId = 1,
            //            DateFrom = new DateTime(2020, 9, 1),
            //            DateTo = new DateTime(2020, 9, 30),
            //            TicketStatus = TicketStatus.Added,
            //            TicketType = TicketType.ImportDocuments,
            //            CreatedAt = new DateTime(2020, 1, 1),
            //        },
            //        new TaskTicket
            //        {
            //            TaskTicketId = 2,
            //            SourceId = 2,
            //            DateFrom = new DateTime(2020, 9, 1),
            //            DateTo = new DateTime(2020, 9, 30),
            //            TicketStatus = TicketStatus.Added,
            //            TicketType = TicketType.ImportDocuments,
            //            CreatedAt = new DateTime(2020, 1, 1),
            //        },
            //        new TaskTicket
            //        {
            //            TaskTicketId = 3,
            //            SourceId = 0,
            //            DateFrom = new DateTime(2020, 9, 1),
            //            DateTo = new DateTime(2020, 9, 30),
            //            TicketStatus = TicketStatus.Added,
            //            TicketType = TicketType.CheckContractors,
            //            CreatedAt = new DateTime(2020, 1, 1),
            //        },
            //        new TaskTicket
            //        {
            //            TaskTicketId = 4,
            //            SourceId = 1,
            //            DateFrom = new DateTime(2020, 9, 1),
            //            DateTo = new DateTime(2020, 9, 30),
            //            TicketStatus = TicketStatus.Added,
            //            TicketType = TicketType.MatchContractors,
            //            CreatedAt = new DateTime(2020, 1, 1),
            //        },
            //        new TaskTicket
            //        {
            //            TaskTicketId = 5,
            //            SourceId = 2,
            //            DateFrom = new DateTime(2020, 9, 1),
            //            DateTo = new DateTime(2020, 9, 30),
            //            TicketStatus = TicketStatus.Added,
            //            TicketType = TicketType.MatchContractors,
            //            CreatedAt = new DateTime(2020, 1, 1),
            //        },
            //        new TaskTicket
            //        {
            //            TaskTicketId = 6,
            //            SourceId = 1,
            //            DateFrom = new DateTime(2020, 9, 1),
            //            DateTo = new DateTime(2020, 9, 30),
            //            TicketStatus = TicketStatus.Added,
            //            TicketType = TicketType.ExportContractorsToErp,
            //            CreatedAt = new DateTime(2020, 1, 1),
            //        },
            //        new TaskTicket
            //        {
            //            TaskTicketId = 7,
            //            SourceId = 2,
            //            DateFrom = new DateTime(2020, 9, 1),
            //            DateTo = new DateTime(2020, 9, 30),
            //            TicketStatus = TicketStatus.Added,
            //            TicketType = TicketType.ExportContractorsToErp,
            //            CreatedAt = new DateTime(2020, 1, 1),
            //        });
        }
    }
}
