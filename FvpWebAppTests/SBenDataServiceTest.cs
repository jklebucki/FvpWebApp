using FvpWebAppWorker.Data;
using FvpWebAppWorker.Infrastructure;
using FvpWebAppWorker.Services;
using Serilog.Core;
using System;
using System.Data;
using System.IO;
using System.Threading.Tasks;
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

    }
}