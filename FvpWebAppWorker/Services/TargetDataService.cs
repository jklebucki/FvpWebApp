using C2FKInterface.Data;
using C2FKInterface.Models;
using C2FKInterface.Services;
using FvpWebAppModels;
using FvpWebAppModels.Models;
using FvpWebAppWorker.Data;
using FvpWebAppWorker.Infrastructure;
using FvpWebAppWorker.Services.Interfaces;
using LinqToDB;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FvpWebAppWorker.Services
{
    class TargetDataService : ITargetDataService
    {
        private readonly ILogger _logger;
        public TargetDataService(ILogger logger)
        {
            _logger = logger;
        }
        public async Task ExportContractorsToErp(WorkerAppDbContext dbContext, TaskTicket taskTicket, Target target)
        {
            var countries = await dbContext.Countries.ToListAsync().ConfigureAwait(false);
            await FvpWebAppUtils.ChangeTicketStatus(dbContext, taskTicket.TaskTicketId, TicketStatus.Pending);
            var allSourcesFromTarget = await dbContext.Sources.Where(s => s.TargetId == target.TargetId).Select(i => i.SourceId).ToListAsync();
            var notMatchedContractors = await dbContext.Contractors.Where(
                c => c.ContractorErpId == null &&
                c.GusContractorEntriesCount == 1 &&
                c.ContractorStatus == ContractorStatus.Valid &&
                allSourcesFromTarget.Contains((int)c.SourceId) &&
                c.ContractorErpPosition == null).ToListAsync().ConfigureAwait(false);
            var c21Contractors = notMatchedContractors.GroupBy(c =>
                new
                {
                    c.VatId,
                    c.Regon,
                    c.Name,
                    c.Street,
                    c.EstateNumber,
                    c.QuartersNumber,
                    c.City,
                    c.CountryCode,
                    c.PostalCode,
                    c.Province,
                    c.Phone,
                    c.Email,
                }).Select(c =>
                new C21Contractor
                {
                    nip = c.Key.VatId,
                    Regon = c.Key.Regon,
                    nazwa = FvpWebAppUtils.TruncateToLength(c.Key.Name, 100),
                    ulica = FvpWebAppUtils.TruncateToLength(c.Key.Street.Replace("ul. ", "").Replace("UL. ", ""), 35),
                    numerDomu = c.Key.EstateNumber,
                    numerMieszk = c.Key.QuartersNumber,
                    Miejscowosc = FvpWebAppUtils.TruncateToLength(c.Key.City, 40),
                    rejon = c.Key.Province.ToLower(),
                    Kraj = c.Key.CountryCode,
                    kod = c.Key.PostalCode,
                    Telefon1 = c.Key.Phone,
                    email = c.Key.Email,
                    aktywny = true,
                    skrot = $"FVP-1-{c.Key.VatId}",
                    statusUE = FvpWebAppUtils.CheckUeCountry(countries, c.Key.CountryCode),
                }).ToList();

            C21ContractorService c21ContractorService = new C21ContractorService(GetDbSettings(target));
            await c21ContractorService.AddContractorsAsync(c21Contractors);
            var output = await c21ContractorService.ProceedContractorsAsync();
            string outputData = "";
            foreach (var item in output)
            {
                outputData += "; " + item;
            }
            Console.WriteLine(outputData);
            SystemDataService systemDataService = new SystemDataService(_logger);
            await systemDataService.MatchContractors(dbContext, taskTicket, target);
        }

        public async Task InsertDocumentsToTarget(WorkerAppDbContext dbContext, TaskTicket taskTicket, Target target)
        {
            C21DocumentService c21DocumentService = new C21DocumentService(GetDbSettings(target));
            var documentsToInsert = await dbContext.Documents.ToListAsync();
        }

        public Task<C21DocumentAggregate> PrepareDocumentAggregate(WorkerAppDbContext workerAppDbContext, TaskTicket taskTicket, Target target)
        {
            throw new NotImplementedException();
        }

        private DbConnectionSettings GetDbSettings(Target target)
        {
            return new DbConnectionSettings(
                    target.DatabaseAddress,
                    target.DatabaseUsername,
                    target.DatabasePassword,
                    target.DatabaseName);
        }
    }
}
