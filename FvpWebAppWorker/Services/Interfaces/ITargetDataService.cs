using C2FKInterface.Models;
using FvpWebAppModels.Models;
using FvpWebAppWorker.Data;
using System.Threading.Tasks;

namespace FvpWebAppWorker.Services.Interfaces
{
    interface ITargetDataService
    {
        Task InsertDocumentsToTarget(TaskTicket taskTicket, Target target);
        Task<C21DocumentAggregate> PrepareDocumentAggregate(TaskTicket taskTicket, Target target);
        Task ExportContractorsToErp(TaskTicket taskTicket, Target target);

    }
}
