using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using C2FKInterface.Data;
using C2FKInterface.Models;
using LinqToDB;
using LinqToDB.Data;

namespace C2FKInterface.Services
{
    public class FKDocumentsService
    {
        private readonly DbConnectionSettings _dbConnectionSettings;

        public FKDocumentsService(DbConnectionSettings dbConnectionSettings)
        {
            _dbConnectionSettings = dbConnectionSettings;
            DataConnection.DefaultSettings = new DbSettings(_dbConnectionSettings.ConnStr());
            DataConnection.SetConnectionString("Db", _dbConnectionSettings.ConnStr());
        }
    }
}