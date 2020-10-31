using C2FKInterface.Data;
using C2FKInterface.Services;
using System;
using Xunit;

namespace FvpWebAppTests
{
    public class C21DocumentServiceTest
    {

        [Fact]
        public async void C21GetNextDocumentIdTest()
        {
            DbConnectionSettings dbConnectionSettings = new DbConnectionSettings("192.168.21.20", "sa", "#sa2015!", "fkf_test_db");
            C21DocumentService documentService = new C21DocumentService(dbConnectionSettings);

            var output = await documentService.GetNextDocumentId(10);

            Console.WriteLine($"C21GetNextDocumentIdTest - Next ID: {output}");
            Assert.True(output > 0);
        }

        [Fact]
        public async void C21GetVatRegisterDefsTest()
        {
            DbConnectionSettings dbConnectionSettings = new DbConnectionSettings("192.168.21.20", "sa", "#sa2015!", "fkf_test_db");
            C21DocumentService documentService = new C21DocumentService(dbConnectionSettings);

            var output = await documentService.GetVarRegistersDefs(1);

            Console.WriteLine($"C21GetVatRegisterDefsTest - Vat register name: {output.rNazwa}");
            Assert.True(output != null);
            var outputNull = await documentService.GetVarRegistersDefs(9999);
            Assert.True(outputNull == null);
        }

        [Fact]
        public async void C21GetYearsTest()
        {
            DbConnectionSettings dbConnectionSettings = new DbConnectionSettings("192.168.21.20", "sa", "#sa2015!", "fkf_test_db");
            C21DocumentService documentService = new C21DocumentService(dbConnectionSettings);

            var output = await documentService.GetYearId(new DateTime(2020, 5, 20));

            Console.WriteLine($"C21GetYearsTest - Year id: {output.rokId}");
            Assert.True(output != null);
            var outputNull = await documentService.GetYearId(new DateTime(2099, 5, 20));
            Assert.True(outputNull == null);
        }

    }
}
