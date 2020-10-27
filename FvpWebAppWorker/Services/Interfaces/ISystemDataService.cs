using FvpWebAppModels.Models;
using FvpWebAppWorker.Data;
using FvpWebAppWorker.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FvpWebAppWorker.Services.Interfaces
{
    public interface ISystemDataService
    {
        Task TransferDocuments(List<Document> documents, TaskTicket taskTicket, WorkerAppDbContext dbContext);
        Task TransferContractors(List<Document> documents, WorkerAppDbContext dbContext);
        Task<List<Contractor>> GetContractorByVatId(string vatId, WorkerAppDbContext dbContext);
        Task<ApiResponseContractor> CheckContractorByGusApi(string vatId);
        Task<ApiResponseContractor> CheckContractorByViesApi(Contractor contractor);
    }
}