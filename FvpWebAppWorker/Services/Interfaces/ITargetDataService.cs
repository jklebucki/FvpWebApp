using C2FKInterface.Models;
using FvpWebAppModels.Models;
using FvpWebAppWorker.Data;
using System.Threading.Tasks;

namespace FvpWebAppWorker.Services.Interfaces
{
    interface ITargetDataService
    {
        Task InsertDocumentsToTarget(WorkerAppDbContext workerAppDbContext, TaskTicket taskTicket, Target target);
        Task<C21DocumentAggregate> PrepareDocumentAggregate(WorkerAppDbContext workerAppDbContext, TaskTicket taskTicket, Target target);
        Task ExportContractorsToErp(WorkerAppDbContext dbContext, TaskTicket taskTicket, Target target);

    }
}
