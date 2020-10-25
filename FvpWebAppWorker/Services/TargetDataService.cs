using C2FKInterface.Data;
using C2FKInterface.Models;
using C2FKInterface.Services;
using FvpWebAppModels;
using FvpWebAppModels.Models;
using FvpWebAppWorker.Data;
using FvpWebAppWorker.Infrastructure;
using FvpWebAppWorker.Models;
using FvpWebAppWorker.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FvpWebAppWorker.Services
{
    public class TargetDataService : ITargetDataService
    {
        private readonly ApiService _apiService;
        private ILogger _logger;
        public TargetDataService(ILogger logger)
        {
            _apiService = new ApiService();
            _logger = logger;

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
                        ContractorStatus = ContractorStatus.Valid,
                        CheckDate = DateTime.Now,
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return contractors;
        }

        public async Task<ApiResponseContractor> CheckContractorByViesApi(Contractor contractor)
        {
            var request = ContractorViesRequest(contractor);
            var apiToken = await _apiService.ApiLogin();
            var response = await _apiService.GetViesDataAsync(request, apiToken);
            var contractors = new List<Contractor>();
            contractor.CheckDate = DateTime.Now;
            if (response != null && response.Status)
            {
                contractor.ContractorStatus = ContractorStatus.Valid;
                contractors.Add(contractor);
            }

            if (response != null)
                return new ApiResponseContractor
                {
                    ApiStatus = response.Status ? ApiStatus.Valid : ApiStatus.NotValid,
                    Contractors = contractors
                };
            else
                return new ApiResponseContractor
                {
                    ApiStatus = ApiStatus.Error,
                    Contractors = new List<Contractor>()
                };

        }

        private ViesSimpleRequest ContractorViesRequest(Contractor contractor)
        {
            var suffix = FvpWebAppUtils.GetDigitsFromString(contractor.VatId);
            var prefix = contractor.CountryCode;
            return new ViesSimpleRequest
            {
                ContractorPrefix = prefix,
                ContractorSuffix = suffix
            };
        }

        public async Task<List<Contractor>> GetContractorByVatId(string vatId, WorkerAppDbContext dbContext)
        {
            var contractors = await dbContext.Contractors.Where(c => c.VatId == vatId).ToListAsync();
            if (contractors != null && contractors.Count > 0)
                return contractors;
            else
                return new List<Contractor>();
        }

        public async Task TransferContractors(List<Document> documents, WorkerAppDbContext dbContext)
        {

            ContractorService contractorService = new ContractorService(dbContext);
            var documentsContractors = AggregateContractorsFromDocuments(documents);
            List<Contractor> newContractors = new List<Contractor>();
            foreach (var documentContractor in documentsContractors)
            {
                var contractorServiceResponse = contractorService.ContractorExist((int)documentContractor.SourceId, documentContractor.ContractorSourceId);
                var isAddedToNewContractors = newContractors.FirstOrDefault(c => c.SourceId == documentContractor.SourceId && c.ContractorSourceId == documentContractor.ContractorSourceId);
                if (!contractorServiceResponse.Exist && isAddedToNewContractors == null)
                {
                    newContractors.Add(documentContractor);
                }
            }

            try
            {
                await dbContext.AddRangeAsync(newContractors).ConfigureAwait(true);
                await dbContext.SaveChangesAsync().ConfigureAwait(true);
                _logger.LogInformation($"Dodano : {newContractors.Count} nowych kontrahentów.");
            }
            catch (Exception ex)
            {
                dbContext.DetachAllEntities();
                _logger.LogError(ex.Message);
            }
            //await UpdateContractorsOnDocuments(dbContext);
        }

        public async Task CheckContractors(WorkerAppDbContext dbContext, int taskTicketId)
        {
            await ChangeTicketStatus(dbContext, taskTicketId, TicketStatus.Pending).ConfigureAwait(false);
            try
            {
                var token = await _apiService.ApiLogin().ConfigureAwait(false);
                var ueCountries = (await dbContext.Countries.ToListAsync().ConfigureAwait(false)).Where(u => u.UE).Select(c => c.Symbol).ToList();

                ContractorService contractorService = new ContractorService(dbContext);
                var documentsContractors = await dbContext.Contractors.Where(s => s.ContractorStatus == ContractorStatus.NotChecked).ToListAsync().ConfigureAwait(false);
                _logger.LogInformation($"Kontrahenci do sprawdzenia: {documentsContractors.Count}");
                foreach (var documentContractor in documentsContractors)
                {

                    if (documentContractor.CountryCode == "PL" || documentContractor.Firm == Firm.FirmaPolska)
                    {
                        var response = await CheckContractorByGusApi(FvpWebAppUtils.GetDigitsFromString(documentContractor.VatId));
                        await ClassificateContractor(dbContext, documentContractor, response);
                    }
                    else if (ueCountries.Contains(documentContractor.CountryCode))
                    {
                        var response = await CheckContractorByViesApi(documentContractor);
                        await ClassificateContractor(dbContext, documentContractor, response);
                    }
                    else
                    {
                        var response = new ApiResponseContractor
                        {
                            ApiStatus = ApiStatus.NotSupportedByApi,
                            Contractors = new List<Contractor>()
                        };
                        await ClassificateContractor(dbContext, documentContractor, response);
                    }

                }
                await UpdateContractorsOnDocuments(dbContext);
                await ChangeTicketStatus(dbContext, taskTicketId, TicketStatus.Done).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                await ChangeTicketStatus(dbContext, taskTicketId, TicketStatus.Failed).ConfigureAwait(false);
                _logger.LogError(ex.Message);
            }
        }

        private async Task ClassificateContractor(WorkerAppDbContext _dbContext, Contractor documentContractor, ApiResponseContractor response)
        {
            if (response.ApiStatus == ApiStatus.Valid)
            {
                await UpdateContractor(_dbContext, documentContractor, response);
                Console.WriteLine($"Kontrahent poprawny: {documentContractor.Name} Nip : {documentContractor.VatId}");
            }
            else if (response.ApiStatus == ApiStatus.NotValid)
            {
                documentContractor.ContractorStatus = ContractorStatus.Invalid;
                response.Contractors.Add(documentContractor);
                await UpdateContractor(_dbContext, documentContractor, response);
                Console.WriteLine($"Kontrahent niepoprawny: {documentContractor.Name} Nip : {documentContractor.VatId}");
            }
            else if (response.ApiStatus == ApiStatus.NotSupportedByApi)
            {
                documentContractor.ContractorStatus = ContractorStatus.Valid;
                response.Contractors.Add(documentContractor);
                await UpdateContractor(_dbContext, documentContractor, response);
                Console.WriteLine($"Nazwa: {documentContractor.Name}\tNip:{documentContractor.VatId}");
            }
            else if (response.ApiStatus == ApiStatus.Error)
            {
                documentContractor.ContractorStatus = ContractorStatus.NotChecked;
                response.Contractors.Add(documentContractor);
                await UpdateContractor(_dbContext, documentContractor, response);
                Console.WriteLine($"B³¹d sprawdzania kontrahenta: {documentContractor.Name} Nip : {documentContractor.VatId}");
            }
        }

        private List<Contractor> AggregateContractorsFromDocuments(List<Document> documents)
        {
            return documents
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
        }

        private async Task UpdateContractor(WorkerAppDbContext dbContext, Contractor contractor, ApiResponseContractor response)
        {
            for (int i = 0; i < response.Contractors.Count; i++)
            {
                if (i == 0)
                {
                    var contractorToUpdate = await dbContext.Contractors.FirstOrDefaultAsync(c => c.ContractorId == contractor.ContractorId && c.SourceId == contractor.SourceId).ConfigureAwait(false);
                    contractorToUpdate = CopyContractorData(contractorToUpdate, response.Contractors[i]);
                    contractorToUpdate.GusContractorEntriesCount = 1;
                    try
                    {
                        dbContext.Update(contractorToUpdate);
                        await dbContext.SaveChangesAsync();
                    }
                    catch (DbUpdateException ex)
                    {
                        _logger.LogError(ex.Message);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.Message);
                    }
                }
                else
                {
                    response.Contractors[i].SourceId = contractor.SourceId;
                    response.Contractors[i].GusContractorEntriesCount = i + 1;
                    response.Contractors[i].ContractorSourceId = contractor.ContractorSourceId;
                    response.Contractors[i].Firm = contractor.Firm;
                    await AddContractor(dbContext, response.Contractors[i]);
                }
            }
        }

        private Contractor CopyContractorData(Contractor destContractor, Contractor sourceContractor)
        {
            destContractor.Name = sourceContractor.Name;
            destContractor.Street = sourceContractor.Street;
            destContractor.EstateNumber = sourceContractor.EstateNumber;
            destContractor.QuartersNumber = sourceContractor.QuartersNumber;
            destContractor.City = sourceContractor.City;
            destContractor.PostalCode = sourceContractor.PostalCode;
            destContractor.Province = sourceContractor.Province;
            destContractor.VatId = sourceContractor.VatId;
            destContractor.Regon = sourceContractor.Regon;
            destContractor.Phone = sourceContractor.Phone;
            destContractor.Email = sourceContractor.Email;
            destContractor.CountryCode = sourceContractor.CountryCode;
            destContractor.ContractorStatus = sourceContractor.ContractorStatus;
            destContractor.CheckDate = sourceContractor.CheckDate;
            return destContractor;
        }

        private async Task AddContractor(WorkerAppDbContext dbContext, Contractor contractor)
        {
            try
            {
                await dbContext.AddAsync(contractor);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                Console.WriteLine($"B³¹d dodania kontrahenta (API): {contractor.Name} Nip : {contractor.VatId}");
            }
        }

        private async Task UpdateContractorsOnDocuments(WorkerAppDbContext dbContext)
        {
            var contractors = await dbContext.Contractors.ToListAsync().ConfigureAwait(false);
            if (contractors != null && contractors.Count > 0)
                try
                {
                    var documents = await dbContext.Documents.Where(c => c.ContractorId == null || c.ContractorId == 0).ToListAsync().ConfigureAwait(false);
                    if (documents != null && documents.Count > 0)
                    {
                        foreach (var document in documents)
                        {
                            var matchedContractors = contractors.Where(c => c.ContractorSourceId == document.DocContractorId && c.SourceId == document.SourceId).ToList();
                            if (matchedContractors != null && matchedContractors.Count > 0)
                                document.ContractorId = matchedContractors[0].ContractorId;
                            if (matchedContractors.Count > 1)
                                document.DocumentStatus = DocumentStatus.ManyContractors;
                        }
                        dbContext.UpdateRange(contractors);
                        await dbContext.SaveChangesAsync();
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    Console.WriteLine("B³¹d aktualiazcji kontrahentów na dokumentach");
                }
        }

        public async Task TransferDocuments(List<Document> documents, TaskTicket taskTicket, WorkerAppDbContext dbContext)
        {
            await ChangeTicketStatus(dbContext, taskTicket.TaskTicketId, TicketStatus.Pending);
            try
            {
                var dividedDocuments = FvpWebAppUtils.DividedRows(documents, 100);
                foreach (var documentPart in dividedDocuments)
                {
                    await dbContext.AddRangeAsync(documentPart).ConfigureAwait(false);
                    await dbContext.SaveChangesAsync().ConfigureAwait(false);
                }

                await ChangeTicketStatus(dbContext, taskTicket.TaskTicketId, TicketStatus.Done).ConfigureAwait(false);
                _logger.LogInformation($"Documents count: {documents.Count}");
            }
            catch (Exception ex)
            {
                await ChangeTicketStatus(dbContext, taskTicket.TaskTicketId, TicketStatus.Failed).ConfigureAwait(false);
                _logger.LogError(ex.Message);
            }
        }


        public static async Task ChangeTicketStatus(WorkerAppDbContext dbContext, int ticketId, TicketStatus ticketStatus)
        {
            dbContext.DetachAllEntities();
            var ticket = await dbContext.TaskTickets.FirstOrDefaultAsync(f => f.TaskTicketId == ticketId);
            ticket.TicketStatus = ticketStatus;
            ticket.StatusChangedAt = DateTime.Now;
            dbContext.Update(ticket);
            await dbContext.SaveChangesAsync();
        }

        public async Task MatchContractors(WorkerAppDbContext dbContext, TaskTicket taskTicket, Target target)
        {
            await ChangeTicketStatus(dbContext, taskTicket.TaskTicketId, TicketStatus.Pending);
            var c21ConnectionSettings = new DbConnectionSettings(
                target.DatabaseAddress,
                target.DatabaseUsername,
                target.DatabasePassword,
                target.DatabaseName);
            C21ContractorService contractorService = new C21ContractorService(c21ConnectionSettings);
            var erpContractors = await contractorService.GetC21FvpContractorsAsync(false).ConfigureAwait(false);
            erpContractors.Where(c => c.Country == "PL" || string.IsNullOrEmpty(c.Country)).ToList().ForEach(c => c.VatId = FvpWebAppUtils.GetDigitsFromString(c.VatId));
            var allSourcesFromTarget = await dbContext.Sources.Where(s => s.TargetId == target.TargetId).Select(i => i.SourceId).ToListAsync();
            var contractors = await dbContext.Contractors.Where(
                c => c.ContractorErpId == null &&
                allSourcesFromTarget.Contains((int)c.SourceId) &&// == taskTicket.SourceId && //only contractors from specific source
                c.ContractorStatus == ContractorStatus.Valid).ToListAsync().ConfigureAwait(false);
            if (contractors != null && contractors.Count > 0)
            {
                Console.WriteLine("Matching contractors...");
                foreach (var contractor in contractors)
                {
                    string vatId = contractor.VatId;
                    if (contractor.CountryCode == "PL")
                        vatId = FvpWebAppUtils.GetDigitsFromString(contractor.VatId);
                    var erpcontractor = erpContractors.FirstOrDefault(c => c.VatId == vatId && c.Active && c.Name == contractor.Name);
                    if (erpcontractor == null)
                        erpcontractor = erpContractors.FirstOrDefault(c => c.VatId == vatId && c.Active);
                    if (erpcontractor == null)
                        erpcontractor = erpContractors.FirstOrDefault(c => c.VatId == vatId && !c.Active && c.Name == contractor.Name);
                    if (erpcontractor == null)
                        erpcontractor = erpContractors.FirstOrDefault(c => c.VatId == vatId && !c.Active);
                    if (erpcontractor != null)
                    {
                        contractor.ContractorErpId = erpcontractor.Id;
                        contractor.ContractorErpPosition = erpcontractor.FkId;
                    }
                }
                try
                {
                    dbContext.UpdateRange(contractors);
                    await dbContext.SaveChangesAsync();
                    Console.WriteLine("Contactors matched!");
                }
                catch (Exception ex)
                {
                    await ChangeTicketStatus(dbContext, taskTicket.TaskTicketId, TicketStatus.Failed);
                    _logger.LogError(ex.Message);
                }
            }
        }

        public async Task ExportContractorsToErp(WorkerAppDbContext dbContext, TaskTicket taskTicket, Target target)
        {
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
                    Kraj = c.Key.CountryCode,
                    kod = c.Key.PostalCode,
                    //= c.Key.Province,
                    Telefon1 = c.Key.Phone,
                    email = c.Key.Email,
                    aktywny = true,
                    skrot = $"FVP-1-{c.Key.VatId}"
                }).ToList();

            var c21ConnectionSettings = new DbConnectionSettings(
                target.DatabaseAddress,
                target.DatabaseUsername,
                target.DatabasePassword,
                target.DatabaseName);
            C21ContractorService c21ContractorService = new C21ContractorService(c21ConnectionSettings);
            await c21ContractorService.AddContractorsAsync(c21Contractors);
            var output = await c21ContractorService.ProceedContractorsAsync();
            string outputData = "";
            foreach (var item in output)
            {
                outputData += "; " + item;
            }
            Console.WriteLine(outputData);
            await MatchContractors(dbContext, taskTicket, target);
        }
    }
}