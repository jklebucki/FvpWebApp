using FvpWebAppModels.Models;
using FvpWebAppWorker.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FvpWebAppWorker.Services.Interfaces
{
    public interface ISystemDataService
    {
        Task TransferDocuments(List<Document> documents, TaskTicket taskTicket);
        Task TransferContractors(List<Document> documents);
        Task<List<Contractor>> GetContractorByVatId(string vatId);
        Task<ApiResponseContractor> CheckContractorByGusApi(string vatId);
        Task<ApiResponseContractor> CheckContractorByViesApi(Contractor contractor);
    }
}