using C2FKInterface.Data;
using C2FKInterface.Models;
using C2FKInterface.Services;
using FvpWebAppModels.Models;
using FvpWebAppWorker.Data;
using FvpWebAppWorker.Infrastructure;
using FvpWebAppWorker.Services.Interfaces;
using LinqToDB;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FvpWebAppWorker.Services
{
    public class TargetDataService : ITargetDataService
    {
        private readonly ILogger _logger;
        private readonly WorkerAppDbContext _dbContext;
        public TargetDataService(ILogger logger, WorkerAppDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }
        public async Task ExportContractorsToErp(TaskTicket taskTicket, Target target)
        {
            var countries = await _dbContext.Countries.ToListAsync().ConfigureAwait(false);
            await FvpWebAppUtils.ChangeTicketStatus(_dbContext, taskTicket.TaskTicketId, TicketStatus.Pending);
            var allSourcesFromTarget = await _dbContext.Sources.Where(s => s.TargetId == target.TargetId).Select(i => i.SourceId).ToListAsync();
            var notMatchedContractors = await _dbContext.Contractors.Where(
                c => c.ContractorErpId == null &&
                c.GusContractorEntriesCount == 1 &&
                c.ContractorStatus == ContractorStatus.Valid &&
                allSourcesFromTarget.Contains((int)c.SourceId) &&
                c.ContractorErpPosition == null).ToListAsync().ConfigureAwait(false);
            var c21Contractors = notMatchedContractors.GroupBy(c =>
                new
                {
                    VatId = string.IsNullOrEmpty(c.VatId) ? "" : c.VatId,
                    Regon = string.IsNullOrEmpty(c.Regon) ? "" : c.Regon,
                    Name = string.IsNullOrEmpty(c.Name) ? "" : c.Name,
                    Street = string.IsNullOrEmpty(c.Street) ? "" : c.Street,
                    EstateNumber = string.IsNullOrEmpty(c.EstateNumber) ? "" : c.EstateNumber,
                    QuartersNumber = string.IsNullOrEmpty(c.QuartersNumber) ? "" : c.QuartersNumber,
                    City = string.IsNullOrEmpty(c.City) ? "" : c.City,
                    CountryCode = string.IsNullOrEmpty(c.CountryCode) ? "" : c.CountryCode,
                    PostalCode = string.IsNullOrEmpty(c.PostalCode) ? "" : c.PostalCode,
                    Province = string.IsNullOrEmpty(c.Province) ? "" : c.Province,
                    Phone = string.IsNullOrEmpty(c.Phone) ? "" : c.Phone,
                    Email = string.IsNullOrEmpty(c.Email) ? "" : c.Email,
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
            SystemDataService systemDataService = new SystemDataService(_logger, _dbContext);
            await systemDataService.MatchContractors(taskTicket, target);
        }

        public async Task InsertDocumentsToTarget(TaskTicket taskTicket, Target target)
        {
            C21DocumentService c21DocumentService = new C21DocumentService(GetDbSettings(target));
            List<C21DocumentAggregate> c21DocumentAggregates = new List<C21DocumentAggregate>();
            var documentsToSend = await _dbContext.Documents.Where(d => d.DocumentStatus == DocumentStatus.Valid && d.SourceId == taskTicket.SourceId).ToListAsync();
            if (documentsToSend != null)
            {
                foreach (var document in documentsToSend)
                {
                    var documentAggregate = await PrepareDocumentAggregate(c21DocumentService, document, taskTicket, target);
                    if (documentAggregate.IsPrepared)
                        c21DocumentAggregates.Add(documentAggregate);
                    else
                        foreach (var msg in documentAggregate.Messages)
                        {
                            _logger.LogError($"Błąd w: {msg.Key}\tKomunikat: {msg.Value}");
                        }
                }
            }
            ///TODO - insert documents to C21 tables
        }

        public async Task<C21DocumentAggregate> PrepareDocumentAggregate(C21DocumentService c21DocumentService, Document document, TaskTicket taskTicket, Target target)
        {

            C21DocumentAggregate c21DocumentAggregate = new C21DocumentAggregate();

            var documentsVatsToSend = await _dbContext.DocumentVats.Where(d => d.DocumentId == document.DocumentId).ToListAsync();
            var accountingRecords = await _dbContext.AccountingRecords.Where(a => a.SourceId == taskTicket.SourceId).ToListAsync();
            var documentVats = await _dbContext.DocumentVats.Where(v => v.DocumentId == document.DocumentId).ToListAsync();
            if (accountingRecords == null)
            {
                c21DocumentAggregate.IsPrepared = false;
                c21DocumentAggregate.Messages.Add(new KeyValuePair<string, string>("Zapisy księgowe", "Brak konfiguracji zapisów księgowych"));
                return c21DocumentAggregate;
            }
            var targetDocumentSettings = await _dbContext.TargetDocumentsSettings.FirstOrDefaultAsync(s => s.SourceId == taskTicket.SourceId);
            if (targetDocumentSettings == null)
            {
                c21DocumentAggregate.IsPrepared = false;
                c21DocumentAggregate.Messages.Add(new KeyValuePair<string, string>("Konfiguracja dokumentu", "Brak dokumentów do wysłania"));
                return c21DocumentAggregate;
            }

            var c21documentId = await c21DocumentService.GetNextDocumentId(1000);
            var year = await c21DocumentService.GetYearId(document.SaleDate);
            var vatRegisterDef = await c21DocumentService.GetVarRegistersDefs(targetDocumentSettings.VatRegisterId);
            var contractor = await _dbContext.Contractors.FirstOrDefaultAsync(c => c.ContractorId == document.ContractorId);
            if (year != null && contractor != null && vatRegisterDef != null)
            {
                c21DocumentAggregate.Document = new C21Document
                {
                    id = c21documentId,
                    rokId = year.rokId,
                    skrot = targetDocumentSettings.DocumentShortcut,
                    kontrahent = contractor.ContractorErpId,
                    nazwa = document.DocumentNumber,
                    tresc = document.DocumentNumber,
                    datawpr = document.DocumentDate,
                    datadok = document.DocumentDate,
                    dataOper = document.SaleDate,
                    kwota = Convert.ToDouble(document.Gross),
                    atrJpkV7 = document.JpkV7,
                };
                var nextAccountingRecordId = await c21DocumentService.GetNextAccountRecordtId(1000);
                foreach (var accountingRecord in accountingRecords)
                {
                    c21DocumentAggregate.AccountingRecords.Add(new C21AccountingRecord
                    {
                        id = nextAccountingRecordId,
                        dokId = c21documentId,
                        pozycja = accountingRecord.RecordOrder,
                        strona = !string.IsNullOrEmpty(accountingRecord.Debit) ? (short)0 : (short)1,
                        kwota = GetRecordAmmount(document, accountingRecord),
                        opis = document.DocumentNumber,
                        synt = GetAccountPart(accountingRecord.Account, 0),
                        poz1 = GetAccountPart(accountingRecord.Account, 1),
                        poz2 = GetAccountPart(accountingRecord.Account, 2),
                        poz3 = GetAccountPart(accountingRecord.Account, 3),
                        poz4 = GetAccountPart(accountingRecord.Account, 4),
                        poz5 = GetAccountPart(accountingRecord.Account, 5),

                    });
                    nextAccountingRecordId++;
                }
                var nextVatRegisterId = await c21DocumentService.GetNextVatRegistertId(1000);
                foreach (var documentVat in documentVats)
                {
                    c21DocumentAggregate.VatRegisters.Add(new C21VatRegister
                    {
                        id = nextVatRegisterId,
                        dokId = c21documentId,
                        rejId = vatRegisterDef.id,
                        okres = document.SaleDate,
                        Oczek = 0,
                        abc = vatRegisterDef.defAbc,
                        nienaliczany = 0,
                        stawka = Convert.ToDouble(documentVat.VatValue)
                    });
                    nextVatRegisterId++;
                }
            }

            ///TODO - complete aggregate
            return c21DocumentAggregate;
        }

        public double GetVatValue(DocumentVat documentVat)
        {
            return Convert.ToDouble(Math.Round(((documentVat.GrossAmount / documentVat.NetAmount) - 1) * 100));
        }

        private double GetRecordAmmount(Document document, AccountingRecord accountingRecord)
        {
            var ammountType = string.IsNullOrEmpty(accountingRecord.Debit) ? accountingRecord.Debit : accountingRecord.Credit;
            double ammount = 0;
            switch (ammountType)
            {
                case "netto":
                    ammount = Convert.ToDouble(document.Net);
                    break;
                case "brutto":
                    ammount = Convert.ToDouble(document.Gross);
                    break;
                case "vat":
                    ammount = Convert.ToDouble(document.Vat);
                    break;
            }
            return ammount;
        }

        private int GetAccountPart(string account, int accountSection)
        {
            var accountParts = account.Split("-");
            int accountSectionNumber = 0;
            try
            {
                accountSectionNumber = int.Parse(accountParts[accountSection]);
            }
            catch
            {
                //ignore
            }
            return accountSectionNumber;
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
