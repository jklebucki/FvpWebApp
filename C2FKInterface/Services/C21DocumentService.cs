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
        public C21DocumentService(DbConnectionSettings dbConnectionSettings)
        {
            _dbConnectionSettings = dbConnectionSettings;
            DataConnection.DefaultSettings = new DbSettings(_dbConnectionSettings.ConnStr());
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
        public async Task AddDocumentAggregate(C21DocumentAggregate documentAggregate)
        {
            if (documentAggregate != null)
                using (var db = new SageDb("Db"))
                {
                    await db.BeginTransactionAsync();
                    try
                    {
                        await db.InsertAsync(documentAggregate.Document);
                        await db.InsertAsync(documentAggregate.AccountingRecords);
                        await db.InsertAsync(documentAggregate.VatRegisters);

                        await db.CommitTransactionAsync();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        await db.RollbackTransactionAsync();
                    }
                }
        }

        public async Task<List<C21VatRegisterDef>> GetVarRegistersDefs()
        {
            List<C21VatRegisterDef> c21VatRegisterDefs;
            using (var db = new SageDb("Db"))
            {
                c21VatRegisterDefs = await db.C21VatRegisterDefs.ToListAsync();
            }
            return c21VatRegisterDefs;
        }

        public async Task<List<C21Year>> GetYears()
        {
            List<C21Year> c21Years;
            using (var db = new SageDb("Db"))
            {
                c21Years = await db.C21Years.ToListAsync();
            }
            return c21Years;
        }

        public async Task<List<string>> ProceedDocumentsAsync(int debug = 1)
        {
            List<string> procOutput = new List<string>();
            using (var db = new SageDb("Db"))
            {
                var response = await db.QueryProcAsync<string>(
                    "[FK].[sp_C21_importDK]",
                    new DataParameter("Debug", debug, DataType.Int32)
                    ).ConfigureAwait(false);
                procOutput = response.ToList();
            }
            return procOutput;
        }
    }
}
