using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using FvpWebAppModels.Models;
using FvpWebAppWorker.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FvpWebAppWorker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //var counter = 0;
            while (!stoppingToken.IsCancellationRequested)
            {

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
                // };

                List<Document> documents = new List<Document>();
                SBenDataService sBenDataService = new SBenDataService();
                try
                {
                    documents.AddRange(await sBenDataService.GetDocuments(new Source
                    {
                        SourceId = 1,
                        Address = "192.168.42.70",
                        DbName = "sben",
                        Username = "sben",
                        Password = "almarwinnet"
                    }).ConfigureAwait(false));
                    Console.WriteLine($"Documents count: {documents.Count}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                try
                {
                    documents.AddRange(await sBenDataService.GetDocuments(new Source
                    {
                        SourceId = 1,
                        Address = "192.168.45.70",
                        DbName = "sben",
                        Username = "sben",
                        Password = "almarwinnet"
                    }).ConfigureAwait(false));
                    Console.WriteLine($"Documents count: {documents.Count}");
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
                                Console.WriteLine($"Nazwa: {element.Name}\tNip:{element.VatCode}");
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
