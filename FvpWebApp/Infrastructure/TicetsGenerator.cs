using FvpWebApp.Models;
using FvpWebAppModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FvpWebApp.Infrastructure
{
    public class TicetsGenerator
    {
        public static List<TaskTicket> ImportTickets(CreateTicketRequest request)
        {
            var createdAt = DateTime.Now;
            return new List<TaskTicket> {
                new TaskTicket
                {
                    SourceId = request.SourceId,
                    DateFrom = DatesFromMonth.DateFrom(request),
                    DateTo = DatesFromMonth.DateTo(request),
                    TicketStatus = TicketStatus.Added,
                    TicketType = TicketType.ImportDocuments,
                    CreatedAt = createdAt,
                    StatusChangedAt = createdAt
                },
                new TaskTicket
                {
                    SourceId = request.SourceId,
                    DateFrom = DatesFromMonth.DateFrom(request),
                    DateTo = DatesFromMonth.DateTo(request),
                    TicketStatus = TicketStatus.Added,
                    TicketType = TicketType.ImportContractors,
                    CreatedAt = createdAt,
                    StatusChangedAt = createdAt
                },
                new TaskTicket
                {
                    SourceId = request.SourceId,
                    DateFrom = DatesFromMonth.DateFrom(request),
                    DateTo = DatesFromMonth.DateTo(request),
                    TicketStatus = TicketStatus.Added,
                    TicketType = TicketType.CheckContractors,
                    CreatedAt = createdAt,
                    StatusChangedAt = createdAt
                },
                new TaskTicket
                {
                    SourceId = request.SourceId,
                    DateFrom = DatesFromMonth.DateFrom(request),
                    DateTo = DatesFromMonth.DateTo(request),
                    TicketStatus = TicketStatus.Added,
                    TicketType = TicketType.MatchContractors,
                    CreatedAt = createdAt,
                    StatusChangedAt = createdAt
                }
            };
        }
        public static List<TaskTicket> ExportTickets(CreateTicketRequest request)
        {
            var createdAt = DateTime.Now;
            return new List<TaskTicket> {
                new TaskTicket
                {
                    SourceId = request.SourceId,
                    DateFrom = DatesFromMonth.DateFrom(request),
                    DateTo = DatesFromMonth.DateTo(request),
                    TicketStatus = TicketStatus.Added,
                    TicketType = TicketType.ExportContractorsToErp,
                    CreatedAt = createdAt,
                    StatusChangedAt = createdAt
                },
                new TaskTicket
                {
                    SourceId = request.SourceId,
                    DateFrom = DatesFromMonth.DateFrom(request),
                    DateTo = DatesFromMonth.DateTo(request),
                    TicketStatus = TicketStatus.Added,
                    TicketType = TicketType.ExportDocumentsToErp,
                    CreatedAt = createdAt,
                    StatusChangedAt = createdAt
                }
            };
        }
    }
}
