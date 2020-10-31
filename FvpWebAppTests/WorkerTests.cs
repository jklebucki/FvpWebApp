using FvpWebAppModels.Models;
using FvpWebAppWorker.Data;
using FvpWebAppWorker.Infrastructure;
using FvpWebAppWorker.Services;
using Serilog.Core;
using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace FvpWebAppTests
{
    public class WorkerTests
    {
        [Fact]
        public async void GetQueryFileTestWithCorrectPath()
        {
            var queryString = await FileUtils.GetQueryFile(Path.Combine(Directory.GetCurrentDirectory(), "TestQueryFiles", "Query.sql"));
            Assert.Equal("File content readed!", queryString);
        }

        [Fact]
        public void GetQueryFileTestWithIncorrectPath()
        {
            Assert.ThrowsAsync<Exception>(async () => await Task.FromResult(FileUtils.GetQueryFile(Path.Combine(Directory.GetCurrentDirectory(), "TestQueryFiles", "Query.sqlx"))));
        }

        [Fact]
        public void GetVatValueTest()
        {
            TargetDataService targetDataService = new TargetDataService(null, null);
            var vatValue23 = targetDataService.GetVatValue(new DocumentVat { NetAmount = 284.9300M, GrossAmount = 350.4600M });
            Assert.True(23D == vatValue23);
            var vatValue8 = targetDataService.GetVatValue(new DocumentVat { NetAmount = 8.3200M, GrossAmount = 8.9900M });
            Assert.True(8D == vatValue8);
        }
    }
}