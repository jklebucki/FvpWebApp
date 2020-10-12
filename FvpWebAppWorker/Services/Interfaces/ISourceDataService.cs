using FvpWebAppModels.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FvpWebAppWorker.Services.Interfaces
{
    public interface ISourceDataService
    {
        Task<List<Document>> GetDocuments(Source source, TaskTicket ticket);
    }
}