using C2FKInterface.Data;
using C2FKInterface.Models;
using C2FKInterface.Services.Interfaces;
using LinqToDB;
using LinqToDB.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace C2FKInterface.Services
{
    public class C21DocumentService : IDocumentService
    {
        private readonly DbConnectionSettings _dbConnectionSettings;
        private List<C21Year> years;
        private List<C21VatRegisterDef> c21VatRegisterDefs;
        private List<C21DocumentDefinition> c21DocumentDefinitions;
        public List<string> _procOutput { get; protected set; }
        public C21DocumentService(DbConnectionSettings dbConnectionSettings, List<string> procOutput)
        {
            _dbConnectionSettings = dbConnectionSettings;
            DataConnection.DefaultSettings = new DbSettings(_dbConnectionSettings.ConnStr());
            _procOutput = procOutput;
        }

        public async Task<int> GetNextDocumentId(int incrementValue)
        {
            using (var db = new SageDb("Db"))
            {
                var maxId = await db.C21Documents.MaxAsync(d => (int?)d.id);
                incrementValue += maxId != null ? (int)maxId : 0;
            }
            return incrementValue;
        }

        public async Task<int> GetNextAccountRecordtId(int incrementValue)
        {
            using (var db = new SageDb("Db"))
            {
                var maxId = await db.C21AccountingRecords.MaxAsync(d => (int?)d.id);
                incrementValue += maxId != null ? (int)maxId : 0;
            }
            return incrementValue;
        }

        public async Task<int> GetNextVatRegistertId(int incrementValue)
        {
            using (var db = new SageDb("Db"))
            {
                var maxId = await db.C21VatRegisters.MaxAsync(d => (int?)d.id);
                incrementValue += maxId != null ? (int)maxId : 0;
            }
            return incrementValue;
        }

        public async Task<C21Year> GetYearId(DateTime documentSaleDate)
        {
            if (years == null)
                using (var db = new SageDb("Db"))
                {
                    years = await db.C21Years.ToListAsync();
                }
            return years.FirstOrDefault(y => documentSaleDate >= y.poczatek && documentSaleDate <= y.koniec);
        }

        public async Task<C21DocumentDefinition> GetDocumentDefinition(string documentShortcut, short yearId)
        {
            if (c21DocumentDefinitions == null)
                using (var db = new SageDb("Db"))
                {
                    c21DocumentDefinitions = await db.C21DocumentDefinitions.ToListAsync();
                }
            return c21DocumentDefinitions.FirstOrDefault(d => d.rokId == yearId && d.dSkrot == documentShortcut);
        }

        public async Task<C21VatRegisterDef> GetVatRegistersDefs(int vatRegisterId)
        {
            if (c21VatRegisterDefs == null)
                using (var db = new SageDb("Db"))
                {
                    c21VatRegisterDefs = await db.C21VatRegisterDefs.ToListAsync();
                }
            return c21VatRegisterDefs.FirstOrDefault(v => v.id == vatRegisterId);
        }

        public async Task<List<C21VatRegisterDef>> GetAllVatRegistersDefs()
        {
            if (c21VatRegisterDefs == null)
                using (var db = new SageDb("Db"))
                {
                    c21VatRegisterDefs = await db.C21VatRegisterDefs.ToListAsync();
                }
            return c21VatRegisterDefs;
        }

        public async Task AddDocumentAggregate(C21DocumentAggregate documentAggregate)
        {

            documentAggregate.RenumberDocumentId(await GetNextDocumentId(50), await GetNextAccountRecordtId(50), await GetNextVatRegistertId(50));
            if (documentAggregate != null)
                using (var db = new SageDb("Db"))
                {
                    await db.BeginTransactionAsync();
                    try
                    {
                        await db.InsertAsync(documentAggregate.Document);
                        await db.BulkCopyAsync(documentAggregate.VatRegisters);
                        await db.BulkCopyAsync(documentAggregate.AccountingRecords);

                        await db.CommitTransactionAsync();
                    }
                    catch (Exception ex)
                    {
                        await db.RollbackTransactionAsync();
                        throw ex;
                    }
                }
        }

        public async Task ClearC2FK()
        {
            using (var db = new SageDb("Db"))
            {
                var docsToClear = await db.C21Documents.Where(x => x.status == -2).ToListAsync();
                if (docsToClear != null && docsToClear.Count > 0)
                {
                    await db.BeginTransactionAsync();
                    try
                    {
                        await db.C21Documents.Where(d => d.status == -2).DeleteAsync();
                        await db.C21AccountingRecords.Where(r => docsToClear.Select(d => (int?)d.id).ToList().Contains(r.dokId)).DeleteAsync();
                        await db.C21VatRegisters.Where(v => docsToClear.Select(d => (int?)d.id).ToList().Contains(v.dokId)).DeleteAsync();
                        await db.CommitTransactionAsync();
                    }
                    catch
                    {
                        await db.RollbackTransactionAsync();
                    }
                }
            }
        }

        public async void ProceedDocumentsAsync(int pack, int ticketId, int debug = 1)
        {
            //List<string> procOutput = new List<string>();
            Console.WriteLine($"Task ticket id:{ticketId} Packiet size:{pack} Packiet Log lines: {_procOutput.Count} Start time: {DateTime.Now}");
            using (var db = new SageDb("Db"))
            {
                db.CommandTimeout = 0;
                var response = await db.QueryProcAsync<string>(
                    "[FK].[sp_C21_importDK]",
                    new DataParameter("Debug", debug, DataType.Int32)
                    ).ConfigureAwait(false);
                _procOutput.AddRange(response.ToList());
            }
            Console.WriteLine($"Task ticket id:{ticketId} Packiet size:{pack} Packiet Log lines: {_procOutput.Count} End time: {DateTime.Now}");
        }
    }
}
