using FvpWebAppModels.Models;
using FvpWebAppWorker.Data;
using LinqToDB;
using System.Collections.Generic;
using System.Linq;
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
        public async Task<List<Contractor>> GetContractorsAsync()
        {
            return await _dbContex.Contractors.Where(c => c.ContractorErpId != null && c.ContractorErpId > 0).ToListAsync();
        }

        public async Task SetContractorErpIdByContractorId(int contractorId, int contractorErpId)
        {
            var contractor = await _dbContex.Contractors.FirstOrDefaultAsync(c => c.ContractorId == contractorId);
            if (contractor != null)
            {
                contractor.ContractorErpId = contractorErpId;
                _dbContex.Update(contractor);
                await _dbContex.SaveChangesAsync().ConfigureAwait(false);
            }
        }
        public async Task SetContractorErpIdByVatId(string vatId, int contractorErpId)
        {
            var contractors = await _dbContex.Contractors.Where(c => c.VatId == vatId).ToListAsync();
            if (contractors != null && contractors.Count > 0)
            {
                contractors.ForEach(c => c.ContractorErpId = contractorErpId);
                _dbContex.UpdateRange(contractors);
                await _dbContex.SaveChangesAsync().ConfigureAwait(false);
            }
        }
    }
}
