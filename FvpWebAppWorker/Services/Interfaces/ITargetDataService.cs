using FvpWebAppModels.Models;
using FvpWebAppWorker.Data;
using FvpWebAppWorker.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FvpWebAppWorker.Services.Interfaces
{
    public interface ITargetDataService
    {
        Task TransferDocuments(List<Document> documents, TaskTicket taskTicket, WorkerAppDbContext dbContext);
        Task TransferContractors(List<Document> documents, Target target);
        Task<Contractor> GetContractorByVatId(string vatId);
        Task<ApiResponseContractor> CheckContractorByGusApi(string vatId);
        Task<ApiResponseContractor> CheckContractorByViesApi(string countryCode, string vatId);
    }
}