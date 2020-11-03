using C2FKInterface.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace C2FKInterface.Services.Interfaces
{
    interface IDocumentService
    {
        Task<int> GetNextDocumentId(int incrementValue);
        Task AddDocumentAggregate(C21DocumentAggregate documentAggregate);
        void ProceedDocumentsAsync(int pack, int ticketId, int debug = 1);
    }
}
