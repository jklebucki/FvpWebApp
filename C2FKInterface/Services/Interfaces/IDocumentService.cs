using C2FKInterface.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace C2FKInterface.Services.Interfaces
{
    interface IDocumentService
    {
        Task<int> GetNextDocumentId(int incrementValue);
        Task AddDocument(C21Document document);
        Task<List<string>> ProceedDocumentsAsync(int debug = 1);
    }
}
