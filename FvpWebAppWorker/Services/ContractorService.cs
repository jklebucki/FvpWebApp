using FvpWebAppWorker.Data;
using LinqToDB;
using System.Threading.Tasks;

namespace FvpWebAppWorker.Services
{
    public class ContractorService
    {
        private readonly WorkerAppDbContext _dbContex;
        public ContractorService(WorkerAppDbContext dbContex)
        {
            _dbContex = dbContex;
        }
        public async Task<ContractorServiceResponse> ContractorExist(int sourceId, string contractorSourceId)
        {
            var contractor = await _dbContex.Contractors.FirstOrDefaultAsync(c =>
            c.ContractorSourceId == contractorSourceId && c.SourceId == sourceId);
            if (contractor != null)
                return new ContractorServiceResponse
                {
                    ContractorStatus = contractor.ContractorStatus,
                    Exist = true
                };
            else
                return new ContractorServiceResponse
                {
                    ContractorStatus = FvpWebAppModels.ContractorStatus.NotChecked,
                    Exist = false
                };
        }
    }
}
