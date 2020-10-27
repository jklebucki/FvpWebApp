using C2FKInterface.Data;
using C2FKInterface.Services;
using System;
using Xunit;

namespace FvpWebAppTests
{
    public class C21DocumentServiceTest
    {

        [Fact]
        public async void C21ContractorServiceProceedContractorTest()
        {
            DbConnectionSettings dbConnectionSettings = new DbConnectionSettings("192.168.21.20", "sa", "#sa2015!", "fkf_test_db");
            C21DocumentService documentService = new C21DocumentService(dbConnectionSettings);

            var output = await documentService.GetNextDocumentId(10);

            Console.WriteLine($"C21ContractorServiceGetNextDocumentIdTest - Next ID: {output}");
            Assert.True(output > 0);
        }
    }
}
