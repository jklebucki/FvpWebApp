using LinqToDB.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace C2FKInterface.Data
{
    public class DbSettings : ILinqToDBSettings
    {
        private string _connString;
        public DbSettings(string connString)
        {
            _connString = connString;
        }
        public IEnumerable<IDataProviderSettings> DataProviders => Enumerable.Empty<IDataProviderSettings>();

        public string DefaultConfiguration => "SqlServer";
        public string DefaultDataProvider => "SqlServer";

        public IEnumerable<IConnectionStringSettings> ConnectionStrings
        {
            get
            {
                yield return
                    new ConnectionStringSettings
                    {
                        Name = "Db",
                        ProviderName = "SqlServer",
                        ConnectionString = _connString
                    };
            }
        }
    }
}
