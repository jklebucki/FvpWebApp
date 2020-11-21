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
        private List<string> _procOutput;
        public TargetDataService(ILogger logger, WorkerAppDbContext dbContext, List<string> procOutput)
        {
            _logger = logger;
            _dbContext = dbContext;
            _procOutput = procOutput;
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
            await FvpWebAppUtils.ChangeTicketStatus(_dbContext, taskTicket.TaskTicketId, TicketStatus.Pending).ConfigureAwait(false);
            C21DocumentService c21DocumentService = new C21DocumentService(GetDbSettings(target), _procOutput);
            //List<C21DocumentAggregate> c21DocumentAggregates = new List<C21DocumentAggregate>();
            var documentsToSend = await _dbContext.Documents.Where(d => d.DocumentStatus == DocumentStatus.Valid && d.SourceId == taskTicket.SourceId).ToListAsync();
            if (documentsToSend != null)
            {
                documentsToSend = documentsToSend.OrderBy(d => d.DocumentDate).ToList();
                var allDocumentVats = await _dbContext.DocumentVats.Where(v => documentsToSend.Select(i => (int?)i.DocumentId).Contains(v.DocumentId)).ToListAsync();
                var contractors = await _dbContext.Contractors.Where(c => c.SourceId == taskTicket.SourceId).ToListAsync();
                var targetDocumentSettings = await _dbContext.TargetDocumentsSettings.FirstOrDefaultAsync(t => t.SourceId == taskTicket.SourceId);
                var accountingRecords = await _dbContext.AccountingRecords.Where(a => a.SourceId == taskTicket.SourceId).ToListAsync();
                var source = await _dbContext.Sources.FirstOrDefaultAsync(s => s.SourceId == taskTicket.SourceId);
                var vatRegisters = await _dbContext.VatRegisters.Where(v => v.TargetDocumentSettingsId == targetDocumentSettings.TargetDocumentSettingsId).ToListAsync();
                Console.WriteLine($"Source: {source.Description} Start prep.: {DateTime.Now}");
                int docCounter = 0;
                int insertCounter = 0;
                foreach (var document in documentsToSend)
                {
                    var documentAggregate = await PrepareDocumentAggregate(accountingRecords, targetDocumentSettings, contractors, allDocumentVats, vatRegisters, c21DocumentService, document, source);
                    if (documentAggregate.IsPrepared)
                    {
                        //c21DocumentAggregates.Add(documentAggregate);
                        try
                        {
                            await c21DocumentService.AddDocumentAggregate(documentAggregate).ConfigureAwait(false);
                            document.DocumentStatus = DocumentStatus.SentToC2FK;
                            _dbContext.Update(document);
                            await _dbContext.SaveChangesAsync();
                            insertCounter++;
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex.Message);
                        }
                    }
                    else
                        foreach (var msg in documentAggregate.Messages)
                        {
                            _logger.LogError($"{msg.Key}: {msg.Value}");
                        }

                    docCounter++;
                    if (insertCounter >= 300 || docCounter == documentsToSend.Count)
                    {
                        c21DocumentService.ProceedDocumentsAsync(docCounter, taskTicket.TaskTicketId);
                        Console.WriteLine($"Added documents: {insertCounter} End: {DateTime.Now}");
                        insertCounter = 0;
                    }


                }
                Console.WriteLine($"Source: {source.Description} End prep.: {DateTime.Now} Documents: {docCounter}");
            }
        }

        public async Task<C21DocumentAggregate> PrepareDocumentAggregate(
            List<AccountingRecord> accountingRecords,
            TargetDocumentSettings targetDocumentSettings,
            List<Contractor> contractors,
            List<DocumentVat> allDocumentVats,
            List<VatRegister> vatRegisters,
            C21DocumentService c21DocumentService,
            Document document, Source source)
        {

            C21DocumentAggregate c21DocumentAggregate = new C21DocumentAggregate();

            var documentVats = allDocumentVats.Where(v => v.DocumentId == document.DocumentId).ToList();
            if (accountingRecords == null)
            {
                c21DocumentAggregate.IsPrepared = false;
                c21DocumentAggregate.Messages.Add(new KeyValuePair<string, string>("Zapisy księgowe", "Brak konfiguracji zapisów księgowych"));
                return c21DocumentAggregate;
            }
            if (targetDocumentSettings == null)
            {
                c21DocumentAggregate.IsPrepared = false;
                c21DocumentAggregate.Messages.Add(new KeyValuePair<string, string>("Konfiguracja dokumentu", $"Nie skonfigurowano typu dokumentu dla tego źródła danych: {source.Description}"));
                return c21DocumentAggregate;
            }

            var c21documentId = 1000;//await c21DocumentService.GetNextDocumentId(1000);
            var year = await c21DocumentService.GetYearId(document.SaleDate);
            var docTypDef = await c21DocumentService.GetDocumentDefinition(targetDocumentSettings.DocumentShortcut, year.rokId);
            var vatRegisterDef = await c21DocumentService.GetVatRegistersDefs(docTypDef.rejestr);
            var contractor = contractors.FirstOrDefault(c => c.ContractorId == document.ContractorId);
            if (year != null && contractor != null && vatRegisterDef != null)
            {
                c21DocumentAggregate.Document = new C21Document
                {
                    id = c21documentId,
                    rokId = year.rokId,
                    skrot = targetDocumentSettings.DocumentShortcut,
                    kontrahent = null,//contractor.ContractorErpPosition != null ? contractor.ContractorErpPosition : null,
                    nazwa = document.DocumentNumber,
                    tresc = document.DocumentNumber,
                    datawpr = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
                    datadok = document.DocumentDate,
                    dataOper = document.SaleDate,
                    kwota = Convert.ToDouble(document.Gross),
                    sygnatura = string.Empty,
                    kontoplatnosci = string.Empty,
                    atrJpkV7 = document.JpkV7,
                    DaneKh = 1, //contractor.ContractorErpPosition == null ? 1 : 0,
                    kh_nazwa = contractor.Name,
                    kh_ulica = contractor.Street.Replace("UL. ", "").Replace("ul. ", ""),
                    kh_dom = contractor.EstateNumber,
                    kh_lokal = contractor.QuartersNumber,
                    kh_nip = contractor.VatId,
                    kh_kodPocztowy = contractor.PostalCode,
                    kh_miejscowosc = contractor.City,
                    kh_kraj = contractor.CountryCode
                };
                var nextAccountingRecordId = 1000;
                foreach (var accountingRecord in accountingRecords)
                {
                    c21DocumentAggregate.AccountingRecords.Add(new C21AccountingRecord
                    {
                        id = nextAccountingRecordId,
                        dokId = c21documentId,
                        pozycja = 0,
                        rozbicie = (short)(accountingRecord.RecordOrder - 1),
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
                var nextVatRegisterId = 1000;
                foreach (var documentVat in documentVats)
                {
                    var vatRegister = GetVatRegisterIdForVat(GetVatValue(documentVat), vatRegisters);
                    c21DocumentAggregate.VatRegisters.Add(new C21VatRegister
                    {
                        id = nextVatRegisterId,
                        dokId = c21documentId,
                        rejId = vatRegister > 0 ? vatRegister : vatRegisterDef.id,
                        okres = document.SaleDate,
                        Oczek = 0,
                        abc = vatRegisterDef.defAbc,
                        nienaliczany = 0,
                        stawka = GetVatValue(documentVat),
                        netto = Convert.ToDouble(documentVat.NetAmount),
                        brutto = Convert.ToDouble(documentVat.GrossAmount),
                        vat = Convert.ToDouble(documentVat.VatAmount),
                        atrJpkV7 = documentVat.VatTags
                    });
                    nextVatRegisterId++;
                }
            }
            c21DocumentAggregate.IsPrepared = true;
            return c21DocumentAggregate;
        }

        public int GetVatRegisterIdForVat(double vatValue, List<VatRegister> vatRegisters)
        {
            if (vatRegisters == null)
                return -1;
            var vatId = vatRegisters.FirstOrDefault(v => Convert.ToDouble(v.VatValue) == vatValue);
            if (vatId != null)
                return vatId.ErpVatRegisterId;
            else
                return -1;
        }


        public double GetVatValue(DocumentVat documentVat)
        {
            if (new string[] { "F", "NP" }.Contains(documentVat.VatCode))
                //return Convert.ToDouble(Math.Round(((documentVat.GrossAmount / documentVat.NetAmount) - 1) * 100));
                return -2;
            if (documentVat.VatCode == "E")
                return -1;
            return Convert.ToDouble(documentVat.VatValue);
        }

        private double GetRecordAmmount(Document document, AccountingRecord accountingRecord)
        {
            var ammountType = !string.IsNullOrEmpty(accountingRecord.Debit) ? accountingRecord.Debit : accountingRecord.Credit;
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
