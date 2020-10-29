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

        public async Task<short?> GetYearId(DateTime documentSaleDate)
        {
            short? yearId = null;
            using (var db = new SageDb("Db"))
            {
                var year = await db.C21Years.FirstOrDefaultAsync(y => documentSaleDate >= y.poczatek && documentSaleDate <= y.koniec);
                if (year != null)
                    yearId = year.rokId;
            }
            return yearId;
        }

        public async Task<C21VatRegisterDef> GetVarRegistersDefs(int vatRegisterId)
        {
            if (c21VatRegisterDefs == null)
                using (var db = new SageDb("Db"))
                {
                    c21VatRegisterDefs = await db.C21VatRegisterDefs.ToListAsync();
                }

            return c21VatRegisterDefs.FirstOrDefault(v => v.id == vatRegisterId);
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
