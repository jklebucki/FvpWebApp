using FvpWebAppModels.Models;
using FvpWebAppWorker.Data;
using FvpWebAppWorker.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
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
                IServiceScope scope = _provider.CreateScope();
                var _dbContext = scope.ServiceProvider.GetRequiredService<WorkerAppDbContext>();
                // await Task.Run(() =>
                // {
                //     _logger.LogInformation($"Worker running at: {DateTimeOffset.Now} counter: {counter}");
                //     for (int i = 0; i < 10; i++)
                //     {
                //         Console.Write("A");
                //     }
                //     counter++;
                // }).ConfigureAwait(true);
                // //await Task.Delay(1000, stoppingToken).ConfigureAwait(false);
                // if (counter > 10)
                // {
                //     await Task.Delay(1000, stoppingToken).ConfigureAwait(false);
                //     await base.StopAsync(stoppingToken).ConfigureAwait(false);
                //     Environment.Exit(0);

                var sources = _dbContext.Sources.ToList();

                List<Document> documents = new List<Document>();
                var dateFrom = new DateTime(2020, 9, 1);
                var dateTo = new DateTime(2020, 9, 30);
                SBenDataService sBenDataService = new SBenDataService();
                try
                {
                    var documentsResponse = await sBenDataService.GetDocuments(
                        sources[0],
                        new TaskTicket
                        {
                            SourceId = sources[0].SourceId,
                            DateFrom = dateFrom,
                            DateTo = dateTo
                        }
                        ).ConfigureAwait(false);
                    documents.AddRange(documentsResponse);
                    Console.WriteLine($"Documents count: {documentsResponse.Count}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                try
                {
                    var documentsResponse = await sBenDataService.GetDocuments(
                        sources[1],
                        new TaskTicket
                        {
                            SourceId = sources[1].SourceId,
                            DateFrom = dateFrom,
                            DateTo = dateTo
                        }
                        ).ConfigureAwait(false);
                    documents.AddRange(documentsResponse);
                    Console.WriteLine($"Documents count: {documentsResponse.Count}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                Console.WriteLine($"Documents: {documents.Count}");
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
                        g.DocContractorVatCode
                    })
                    .Select(c => new
                    {
                        c.Key.SourceId,
                        c.Key.DocContractorId,
                        c.Key.DocContractorName,
                        c.Key.DocContractorStreetAndNumber,
                        c.Key.DocContractorCity,
                        c.Key.DocContractorCountryCode,
                        c.Key.DocContractorPostCode,
                        c.Key.DocContractorVatCode
                    }).ToList();
                Console.WriteLine($"Kontrahenci do sprawdzenia: {documentsContractors.Count}");
                try
                {
                    foreach (var item in documentsContractors)
                    {
                        var vatId = new String(item.DocContractorVatCode.Where(Char.IsDigit).ToArray());
                        var response = await targetDataService.CheckContractorByGusApi(vatId);
                        if (response.ApiStatus == Models.ApiStatus.Valid)
                        {
                            foreach (var element in response.Contractors)
                            {
                                Console.WriteLine($"Nazwa: {element.Name}\tNip:{element.VatId}");
                            }
                            checkedContractors.AddRange(response.Contractors);
                        }
                        else if (response.ApiStatus == Models.ApiStatus.NotValid)
                        {
                            Console.WriteLine($"Kontrahent niepoprawny: {item.DocContractorName} Nip : {item.DocContractorVatCode}");
                        }
                        else if (response.ApiStatus == Models.ApiStatus.Error)
                        {
                            Console.WriteLine($"Błąd sprawdzania kontrahenta: {item.DocContractorName} Nip : {item.DocContractorVatCode}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }


            }
        }
    }
}
