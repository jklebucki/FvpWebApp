using FvpWebAppModels;
using FvpWebAppModels.Models;
using FvpWebAppWorker.Data;
using FvpWebAppWorker.Infrastructure;
using FvpWebAppWorker.Models;
using FvpWebAppWorker.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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

        public async Task TransferContractors(List<Document> documents, WorkerAppDbContext _dbContext)
        {
            try
            {
                ContractorService contractorService = new ContractorService(_dbContext);
                var documentsContractors = AggregateContractorsFromDocuments(documents);
                _logger.LogInformation($"Kontrahenci do dodania: {documentsContractors.Count}");
                foreach (var documentContractor in documentsContractors)
                {
                    var contractorServiceResponse = await contractorService.ContractorExist((int)documentContractor.SourceId, documentContractor.ContractorSourceId);
                    if (!contractorServiceResponse.Exist)
                    {
                        await _dbContext.AddAsync(documentContractor).ConfigureAwait(true);
                        await _dbContext.SaveChangesAsync().ConfigureAwait(true);
                        Console.WriteLine($"Kontrahent dodany: {documentContractor.Name} Nip : {documentContractor.VatId}");
                    }
                    else
                        Console.WriteLine($"Kontrahent istnieje: {documentContractor.Name} Nip : {documentContractor.VatId}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
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
                    var contractorToUpdate = await dbContext.Contractors.FirstOrDefaultAsync(c => c.ContractorId == contractor.ContractorId).ConfigureAwait(false);
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
            var ticket = await dbContext.TaskTickets.FirstOrDefaultAsync(f => f.TaskTicketId == ticketId);
            ticket.TicketStatus = ticketStatus;
            ticket.StatusChangedAt = DateTime.Now;
            dbContext.Update(ticket);
            await dbContext.SaveChangesAsync();
        }
    }
}