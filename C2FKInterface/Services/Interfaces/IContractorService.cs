using C2FKInterface.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace C2FKInterface.Services.Interfaces
{
    interface IContractorService
    {
        Task<List<C21Contractor>> GetC21ContractorsAsync();
        Task<List<C21Contractor>> GetC21ContractorsAsync(string vatId);
        Task AddContractorsAsync(List<C21Contractor> c21Contractors);
    }
}
