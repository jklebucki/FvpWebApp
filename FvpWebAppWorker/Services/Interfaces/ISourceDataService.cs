using System.Collections.Generic;
using System.Threading.Tasks;
using FvpWebAppModels.Models;

namespace FvpWebAppWorker.Services.Interfaces
{
    public interface ISourceDataService
    {
        Task<List<Document>> GetDocuments(Source source, TaskTicket ticket);
    }
}