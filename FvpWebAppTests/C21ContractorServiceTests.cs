
using C2FKInterface.Data;
using C2FKInterface.Models;
using C2FKInterface.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace FvpWebAppTests
{
    public class C21ContractorServiceTests
    {
        [Fact]
        public async void C21ContractorServiceAddContractorTest()
        {
            DbConnectionSettings dbConnectionSettings = new DbConnectionSettings("192.168.21.20", "sa", "#sa2015!", "fkf_test_db");

            C21ContractorService contractorService = new C21ContractorService(dbConnectionSettings);
            var erpContractors = await contractorService.GetC21FvpContractorsAsync(true);
            int? erpContractorId = null;
            if (erpContractors != null)
            {
                var erpContractor = erpContractors.FirstOrDefault(v => v.VatId == "6912503886TEST");
                if (erpContractor != null)
                    erpContractorId = erpContractor.Id;
            }

            C21Contractor contractor = new C21Contractor
            {
                id = erpContractorId,
                skrot = "FVP-TestKh",
                nazwa = $"FVP - Kontrahent testowy {string.Format("{0:HH:mm:ss}", DateTime.Now)}",
                ulica = "ul. Okrzei",
                numerDomu = "13",
                kod = "59-220",
                nip = "6912503886TEST",
                Kraj = "PL",
                aktywny = true,
                statusUE = 0,
                externalId = null,
                limit = false,
                limitKwota = 0.0,
                negoc = 0
            };

            await contractorService.AddContractorsAsync(new List<C21Contractor> { contractor });
            var contractorReaded = await contractorService.GetC21ContractorsAsync();
            Console.WriteLine($"C21ContractorServiceAddContractorTest - contractor read : {contractorReaded.FirstOrDefault().nazwa}");
            Assert.True(contractorReaded.Count > 0);
        }

        [Fact]
        public async void C21ContractorServiceGetFvpContractorTest()
        {
            DbConnectionSettings dbConnectionSettings = new DbConnectionSettings("192.168.21.20", "sa", "#sa2015!", "fkf_test_db");
            C21ContractorService contractorService = new C21ContractorService(dbConnectionSettings);

            var memBefore = GC.GetTotalMemory(false);
            var fvpContractors = await contractorService.GetC21FvpContractorsAsync(true).ConfigureAwait(false);
            var memDiff = ((GC.GetTotalMemory(false) - memBefore) / 1024) / 1024;
            Console.WriteLine($"C21ContractorServiceGetFvpContractorTest - contractors read : {fvpContractors.Count}");
            Console.WriteLine($"C21ContractorServiceGetFvpContractorTest - size in MB : {memDiff}");
            Assert.True(fvpContractors.Count > 0);
        }

        [Fact]
        public async void C21ContractorServiceProceedContractorTest()
        {
            DbConnectionSettings dbConnectionSettings = new DbConnectionSettings("192.168.21.20", "sa", "#sa2015!", "fkf_test_db");
            C21ContractorService contractorService = new C21ContractorService(dbConnectionSettings);

            var output = await contractorService.ProceedContractorsAsync();

            Console.WriteLine($"C21ContractorServiceProceedContractorTest - output records : {output.Count}");
            Assert.True(output.Count > 0);
        }

        [Fact]
        public async void C21ContractorServiceCreateViewTest()
        {
            DbConnectionSettings dbConnectionSettings = new DbConnectionSettings("192.168.21.20", "sa", "#sa2015!", "fkf_test_db");
            C21ContractorService contractorService = new C21ContractorService(dbConnectionSettings);

            var output = await contractorService.CreateContractorView();

            Console.WriteLine($"C21ContractorServiceCreateViewTest - status : {output}");
            Assert.True(output.Length > 0);
        }
    }
}
