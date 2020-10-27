using C2FKInterface.Data;
using C2FKInterface.Models;
using C2FKInterface.Services.Interfaces;
using LinqToDB;
using LinqToDB.Data;
using System;
using System.Collections.Generic;
using System.Text;
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
        public Task AddDocumentAggregate(C21DocumentAggregate documentAggregate)
        {
            throw new NotImplementedException();
        }
        public Task<List<string>> ProceedDocumentsAsync(int debug = 1)
        {
            throw new NotImplementedException();
        }
    }
}
