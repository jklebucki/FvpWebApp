using FvpWebAppModels.Models;
using FvpWebAppWorker.Data;
using FvpWebAppWorker.Infrastructure;
using FvpWebAppWorker.Models;
using FvpWebAppWorker.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FvpWebAppWorker.Services
{
    public class TargetDataService : ITargetDataService
    {
        private readonly ApiService _apiService;
        public TargetDataService()
        {
            _apiService = new ApiService();
        }

        public async Task<ApiResponseContractor> CheckContractorByGusApi(string vatId)
        {

            var apiToken = await _apiService.ApiLogin();
            if (apiToken.Token == null)
                return new ApiResponseContractor
                {
                    ApiStatus = ApiStatus.Error,
                    Contractors = new List<Contractor>(),
                };
            var gusContractors = await _apiService.GetGusDataAsync(vatId, apiToken);
            if (gusContractors.Count > 0)
                return new ApiResponseContractor
                {
                    ApiStatus = ApiStatus.Valid,
                    Contractors = GusContractorsToContractors(gusContractors),
                };
            return new ApiResponseContractor
            {
                ApiStatus = ApiStatus.NotValid,
                Contractors = new List<Contractor>(),
            };
        }

        private List<Contractor> GusContractorsToContractors(List<GusContractor> gusContractors
        )
        {
            List<Contractor> contractors = new List<Contractor>();
            try
            {
                foreach (var item in gusContractors)
                {
                    contractors.Add(new Contractor
                    {
                        Name = System.Net.WebUtility.HtmlDecode(item.Name),
                        VatId = item.VatNumber,
                        City = item.City,
                        PostalCode = item.PostalCode,
                        CountryCode = "PL",
                        Street = item.Street,
                        EstateNumber = item.EstateNumber,
                        QuartersNumber = item.QuartersNumber,
                        Province = item.Province,
                        Regon = item.Regon,
                        Phone = item.Phone,
                        Email = item.Email,
                        ContractorStatus = FvpWebAppModels.ContractorStatus.Valid,
                        CheckDate = DateTime.Now,
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return contractors;
        }

        public Task<ApiResponseContractor> CheckContractorByViesApi(string countryCode, string vatId)
        {
            throw new System.NotImplementedException();
        }

        public Task<Contractor> GetContractorByVatId(string vatId)
        {
            throw new System.NotImplementedException();
        }

        public Task TransferContractors(List<Document> documents, Target target)
        {
            throw new System.NotImplementedException();
        }

        public async Task TransferDocuments(List<Document> documents, TaskTicket taskTicket, WorkerAppDbContext dbContext)
        {
            await ChangeTicketStatus(dbContext, taskTicket.TaskTicketId, TicketStatus.Pending);
            try
            {
                var dividedDocuments = FvpWebAppUtils.DividedRows(documents, 100);
                foreach (var documentPart in dividedDocuments)
                {
                    await dbContext.AddRangeAsync(documentPart);
                    await dbContext.SaveChangesAsync();
                }

                await ChangeTicketStatus(dbContext, taskTicket.TaskTicketId, TicketStatus.Done).ConfigureAwait(false);
                Console.WriteLine($"Documents count: {documents.Count}");
            }
            catch (Exception ex)
            {
                await ChangeTicketStatus(dbContext, taskTicket.TaskTicketId, TicketStatus.Failed).ConfigureAwait(false);
                Console.WriteLine(ex.Message);
            }
        }

        public static async Task ChangeTicketStatus(WorkerAppDbContext dbContext, int ticketId, TicketStatus ticketStatus)
        {
            var ticket = await dbContext.TaskTickets.FirstOrDefaultAsync(f => f.TaskTicketId == ticketId);
            ticket.TicketStatus = ticketStatus;
            ticket.StatusChangedAt = DateTime.Now;
            dbContext.Update(ticket);
            await dbContext.SaveChangesAsync();
        }
    }
}