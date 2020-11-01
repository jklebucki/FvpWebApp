using C2FKInterface.Models;
using C2FKInterface.Services;
using FvpWebAppModels.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FvpWebAppWorker.Services.Interfaces
{
    interface ITargetDataService
    {
        Task InsertDocumentsToTarget(TaskTicket taskTicket, Target target);
        Task<C21DocumentAggregate> PrepareDocumentAggregate(
            List<AccountingRecord> accountingRecords,
            TargetDocumentSettings targetDocumentSettings,
            List<Contractor> contractors,
            List<DocumentVat> documentVats,
            C21DocumentService c21DocumentService,
            Document document, Source source);
        Task ExportContractorsToErp(TaskTicket taskTicket, Target target);

    }
}
