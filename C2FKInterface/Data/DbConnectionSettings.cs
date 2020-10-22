using System;
using System.Data.SqlClient;

namespace C2FKInterface.Data
{
    public class DbConnectionSettings
    {
        private string _databaseAddress { get; set; }
        private string _userName { get; set; }
        private string _userPassword { get; set; }
        private string _databaseName { get; set; }

        public DbConnectionSettings(string databaseAddress, string userName, string userPassword, string databaseName)
        {
            _databaseAddress = databaseAddress ?? throw new ArgumentNullException(nameof(databaseAddress));
            _userName = userName ?? throw new ArgumentNullException(nameof(userName));
            _userPassword = userPassword ?? throw new ArgumentNullException(nameof(userPassword));
            _databaseName = databaseName ?? throw new ArgumentNullException(nameof(databaseName));
        }

        public string ConnStr()
        {
            var cs = new SqlConnectionStringBuilder();
            cs.DataSource = _databaseAddress;
            cs.UserID = _userName;
            cs.Password = _userPassword;
            cs.InitialCatalog = _databaseName;
            return cs.ConnectionString;
        }
    }

}
