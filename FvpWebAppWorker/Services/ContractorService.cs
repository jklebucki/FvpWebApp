using FvpWebAppModels.Models;
using FvpWebAppWorker.Data;
using System.Collections.Generic;
using System.Linq;

namespace FvpWebAppWorker.Services
{
    public class ContractorService
    {
        private readonly WorkerAppDbContext _dbContex;
        private readonly List<Contractor> contractors;
        public ContractorService(WorkerAppDbContext dbContex)
        {
            _dbContex = dbContex;
            contractors = _dbContex.Contractors.ToList();
        }
        public ContractorServiceResponse ContractorExist(int sourceId, string contractorSourceId)
        {

            var contractor = contractors.FirstOrDefault(c => c.ContractorSourceId == contractorSourceId && c.SourceId == sourceId);
            if (contractor != null)
                return new ContractorServiceResponse
                {
                    ContractorStatus = contractor.ContractorStatus,
                    Exist = true
                };
            else
                return new ContractorServiceResponse
                {
                    ContractorStatus = ContractorStatus.NotChecked,
                    Exist = false
                };
        }
    }
}
