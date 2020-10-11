using System;
using System.IO;
using System.Threading.Tasks;
using FvpWebAppWorker.Infrastructure;
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
    }
}