using FvpWebAppModels;
using FvpWebAppModels.Models;
using FvpWebAppWorker.Data;
using FvpWebAppWorker.Infrastructure;
using FvpWebAppWorker.Models;
using FvpWebAppWorker.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FvpWebAppWorker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IServiceProvider _provider;
        public Worker(ILogger<Worker> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _provider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (IServiceScope scope = _provider.CreateScope())
                {
                    using (var _dbContext = scope.ServiceProvider.GetRequiredService<WorkerAppDbContext>())
                    {
                        var taskTicket = await _dbContext.TaskTickets.FirstOrDefaultAsync(s => s.TicketStatus == TicketStatus.Added).ConfigureAwait(false);
                        if (taskTicket != null)
                        {
                            var source = await _dbContext.Sources.FirstOrDefaultAsync(i => i.SourceId == taskTicket.SourceId).ConfigureAwait(false);

                            List<Document> documents = new List<Document>();
                            if (source != null)
                                try
                                {
                                    switch (source.Type)
                                    {
                                        case "oracle_sben_dp":
                                            documents = await ProceedSbenOracleDpDocuments(_dbContext, source, taskTicket).ConfigureAwait(false);
                                            break;
                                        default:
                                            await TargetDataService.ChangeTicketStatus(_dbContext, taskTicket.TaskTicketId, TicketStatus.Failed).ConfigureAwait(false);
                                            break;
                                    }

                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                            else
                                await TargetDataService.ChangeTicketStatus(_dbContext, taskTicket.TaskTicketId, TicketStatus.Failed).ConfigureAwait(false);
                            Console.WriteLine($"Documents: {documents.Count}");

                            try
                            {
                                if (documents.Count > 0)
                                    await ProceedContractors(_dbContext, documents).ConfigureAwait(false);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                    };
                }
                await Task.Delay(5000).ConfigureAwait(false);
            }
        }

        private static async Task<List<Document>> ProceedSbenOracleDpDocuments(WorkerAppDbContext _dbContext, Source source, TaskTicket taskTicket)
        {
            SBenDataService sBenDataService = new SBenDataService();
            var documentsResponse = await sBenDataService.GetDocuments(source, taskTicket).ConfigureAwait(false);
            if (documentsResponse != null && documentsResponse.Count > 0)
            {
                TargetDataService targetDataService = new TargetDataService();
                await targetDataService.TransferDocuments(documentsResponse, taskTicket, _dbContext);
            }
            
            return documentsResponse;
        }

        private static async Task ProceedContractors(WorkerAppDbContext _dbContext, List<Document> documents)
        {
            List<Contractor> checkedContractors = new List<Contractor>();
            TargetDataService targetDataService = new TargetDataService();
            var documentsContractors = documents
                .GroupBy(g => new
                {
                    g.SourceId,
                    g.DocContractorId,
                    g.DocContractorName,
                    g.DocContractorStreetAndNumber,
                    g.DocContractorCity,
                    g.DocContractorCountryCode,
                    g.DocContractorPostCode,
                    g.DocContractorVatId,
                    g.DocContractorFirm,
                })
                .Select(c => new Contractor
                {
                    SourceId = c.Key.SourceId,
                    ContractorSourceId = c.Key.DocContractorId,
                    Name = c.Key.DocContractorName,
                    Street = c.Key.DocContractorStreetAndNumber,
                    City = c.Key.DocContractorCity,
                    CountryCode = c.Key.DocContractorCountryCode,
                    PostalCode = c.Key.DocContractorPostCode,
                    VatId = c.Key.DocContractorVatId,
                    ContractorStatus = ContractorStatus.NotChecked,
                    Firm = (Firm)c.Key.DocContractorFirm
                }).ToList();
            Console.WriteLine($"Kontrahenci do sprawdzenia: {documentsContractors.Count}");
            try
            {
                ContractorService contractorService = new ContractorService(_dbContext);
                foreach (var item in documentsContractors)
                {
                    //tutaj skończyłem
                    var contractorServiceResponse = await contractorService.ContractorExist((int)item.SourceId, item.ContractorSourceId);
                    if (!contractorServiceResponse.Exist)
                    {
                        var vatId = new String(item.VatId.Where(Char.IsDigit).ToArray());
                        var response = await targetDataService.CheckContractorByGusApi(vatId);
                        if (response.ApiStatus == Models.ApiStatus.Valid)
                        {
                            foreach (var element in response.Contractors)
                            {
                                Console.WriteLine($"Nazwa: {element.Name}\tNip:{element.VatId}");
                            }
                            await AddContractor(_dbContext, item, response);
                        }
                        else if (response.ApiStatus == Models.ApiStatus.NotValid)
                        {
                            response.Contractors.Add(item);
                            await AddContractor(_dbContext, item, response);
                            Console.WriteLine($"Kontrahent niepoprawny: {item.Name} Nip : {item.VatId}");
                        }
                        else if (response.ApiStatus == Models.ApiStatus.Error)
                        {
                            response.Contractors.Add(item);
                            await AddContractor(_dbContext, item, response);
                            Console.WriteLine($"Błąd sprawdzania kontrahenta: {item.Name} Nip : {item.VatId}");
                        }
                    }
                    else
                        Console.WriteLine($"Kontrahent istnieje: {item.Name} Nip : {item.VatId}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static async Task AddContractor(WorkerAppDbContext _dbContext, Contractor item, ApiResponseContractor response)
        {
            response.Contractors.ForEach(c =>
            {
                c.SourceId = item.SourceId;
                c.GusContractorEntriesCount = response.Contractors.Count;
                c.ContractorSourceId = item.ContractorSourceId;
                c.Firm = item.Firm;
            });
            await _dbContext.AddRangeAsync(response.Contractors);
            await _dbContext.SaveChangesAsync();
        }
    }
}
