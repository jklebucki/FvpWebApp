using System.Collections.Generic;
using System.Threading.Tasks;
using FvpWebAppModels.Models;

namespace FvpWebAppWorker.Services.Interfaces
{
    public interface ITargetDataService
    {
        Task TransferDocuments(List<Document> documents, Target target);
    }
}