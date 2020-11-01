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
