using FvpWebApp.Models;
using System.Threading.Tasks;

namespace FvpWebApp.Services.Interfaces
{
    public interface ISourceService
    {
        Task<WebAppMessage> UpdateSource(SourceAggregate sourceAggregate);
        Task<WebAppMessage> ClearContractors(int sourceId);
        //I can't do separate methods because everything has to be done in one transaction
        //Task<WebAppMessage> AddSource(SourceAggregate sourceAggregate);
        //Task<WebAppMessage> AddAccountingRecords(int sourceId, List<AccountingRecord> accountingRecords);
        //Task<WebAppMessage> AddTargetDocumenSettings(TargetDocumentSettings targetDocumentSettings);
        //Task<WebAppMessage> AddVatRegisters(int targetDocumentSettingsId, List<VatRegister> vatRegisters);
    }
}
