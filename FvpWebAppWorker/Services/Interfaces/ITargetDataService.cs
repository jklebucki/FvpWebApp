using C2FKInterface.Models;
using C2FKInterface.Services;
using FvpWebAppModels.Models;
using System.Threading.Tasks;

namespace FvpWebAppWorker.Services.Interfaces
{
    interface ITargetDataService
    {
        Task InsertDocumentsToTarget(TaskTicket taskTicket, Target target);
        Task<C21DocumentAggregate> PrepareDocumentAggregate(C21DocumentService c21DocumentService, Document document, TaskTicket taskTicket, Target target);
        Task ExportContractorsToErp(TaskTicket taskTicket, Target target);

    }
}
