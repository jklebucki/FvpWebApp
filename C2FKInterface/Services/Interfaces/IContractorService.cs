using C2FKInterface.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace C2FKInterface.Services.Interfaces
{
    interface IContractorService
    {
        Task<List<C21Contractor>> GetC21ContractorsAsync();
        Task AddContractorsAsync(List<C21Contractor> c21Contractors);
        Task<List<C21FvpContractor>> GetC21FvpContractorsAsync(bool active = true);
        Task<List<string>> ProceedContractorsAsync(int debug = 1);
    }
}
