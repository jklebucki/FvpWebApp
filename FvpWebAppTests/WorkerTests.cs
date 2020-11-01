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
            var vatValue23 = targetDataService.GetVatValue(new DocumentVat { VatCode = "A", VatValue = 23 });
            Assert.True(23 == vatValue23);
            var vatValue8 = targetDataService.GetVatValue(new DocumentVat { VatCode = "B", VatValue = 8 });
            Assert.True(8 == vatValue8);
            var vatValue5 = targetDataService.GetVatValue(new DocumentVat { VatCode = "C", VatValue = 5 });
            Assert.True(5 == vatValue5);
            var vatValueZw = targetDataService.GetVatValue(new DocumentVat { VatCode = "E", VatValue = 0 });
            Assert.True(-1 == vatValueZw);
            var vatValueNPF = targetDataService.GetVatValue(new DocumentVat { VatCode = "F", VatValue = 0 });
            Assert.True(-2 == vatValueNPF);
            var vatValueNP = targetDataService.GetVatValue(new DocumentVat { VatCode = "NP", VatValue = 0 });
            Assert.True(-2 == vatValueNP);
        }
    }
}