using FvpWebApp.Data;
using FvpWebApp.Infrastructure;
using FvpWebApp.Models;
using FvpWebAppModels.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FvpWebApp.Services
{
    public class DocumentsImportService
    {
        private ApplicationDbContext _dbContext { get; set; }
        public DocumentsImportService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<ServiceResponse> InsertDocumentsAsync(List<Document> documents, CreateTicketRequest createTicketRequest)
        {
            var serviceResponse = new ServiceResponse { Valid = true, Message = "OK" };
            var importTickets = TicketsGenerator.ImportTickets(createTicketRequest);
            try
            {
                using (var transaction = await _dbContext.Database.BeginTransactionAsync().ConfigureAwait(false))
                {
                    var ticketsIds = new List<int>();
                    foreach (var ticket in importTickets)
                    {
                        await _dbContext.AddAsync(ticket);
                        await _dbContext.SaveChangesAsync();
                        ticketsIds.Add(ticket.TaskTicketId);
                    }
                    int ticketId = ticketsIds[0];
                    documents.ForEach(d => d.TaskTicketId = ticketId);
                    await _dbContext.AddRangeAsync(documents);
                    await _dbContext.SaveChangesAsync();
                    await transaction.CommitAsync().ConfigureAwait(false);
                };
            }
            catch (Exception ex)
            {
                serviceResponse.Valid = false;
                serviceResponse.Message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }
            return serviceResponse;
        }
    }
}
