using FvpWebAppWorker.Services;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;
using Xunit;

namespace FvpWebAppTests
{
    public class SBenDataServiceTest
    {
        private DataRow _dataRow { get; set; }
        public SBenDataServiceTest()
        {
            DataTable table = new DataTable();
            DataColumn column;

            column = new DataColumn();
            column.DataType = Type.GetType("System.Int16");
            column.ColumnName = "CZYZPAR";
            table.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.Int16");
            column.ColumnName = "POWIAZANA";
            table.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.Int16");
            column.ColumnName = "SPLITPAYMENT";
            table.Columns.Add(column);

            _dataRow = table.NewRow();
            _dataRow["CZYZPAR"] = 1;
            _dataRow["POWIAZANA"] = 1;
            _dataRow["SPLITPAYMENT"] = 1;
        }

        [Fact]
        public void JpkV7DocumentTagsTest()
        {
            SBenDataService sBenDataService = new SBenDataService();
            sBenDataService.JpkV7DocumentTags(_dataRow);

            var tags = sBenDataService.JpkV7DocumentTags(_dataRow);
            Assert.Equal("FP,TP,MPP", tags);
        }

        [Fact]
        public void DatabaseConnectionTest()
        {
            bool isConnection = false;
            using (OracleConnection conn = new OracleConnection(
                "Data Source=" + "192.168.164.70"
                + "/xepdb1;User ID=" + "sben"
                + ";Password=" + "almarwinnet"
                + ";Pooling=False;Connection Timeout=60;"))
            {
                conn.Open();
                var connInfo = conn.GetSessionInfo();
                if (connInfo != null)
                {
                    isConnection = true;
                }
                conn.Close();
            }
            Assert.True(isConnection);
        }

    }
}