
using C2FKInterface.Data;
using C2FKInterface.Models;
using C2FKInterface.Services;
using System;
using System.Collections.Generic;
using System.Data.Common;
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

            ContractorService contractorService = new ContractorService(dbConnectionSettings);
            C21Contractor contractor = new C21Contractor
            {
                id = null,
                skrot = "FVP-TestKh",
                nazwa = "FVP - Kontrahent testowy",
                ulica = "ul. Okrzei",
                numerDomu = "13",
                kod = "59-220",
                nip = "6912503886",
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
            Console.WriteLine($"C21ContractorServiceAddContractorTest - contractorReaded : {contractorReaded.FirstOrDefault().nazwa}");
            Assert.True(contractorReaded.Count > 0);
        }

        [Fact]
        public async void C21ContractorServiceGetFvpContractorTest()
        {
            DbConnectionSettings dbConnectionSettings = new DbConnectionSettings("192.168.21.20", "sa", "#sa2015!", "fkf_test_db");
            ContractorService contractorService = new ContractorService(dbConnectionSettings);

            var memBefore = GC.GetTotalMemory(false);
            var fvpContractors = await contractorService.GetC21FvpContractorsAsync(true).ConfigureAwait(false);
            var memDiff = ((GC.GetTotalMemory(false) - memBefore) / 1024) / 1024;
            Console.WriteLine($"C21ContractorServiceGetFvpContractorTest - contractorsReaded : {fvpContractors.Count}");
            Console.WriteLine($"C21ContractorServiceGetFvpContractorTest - size in MB : {memDiff}");
            Assert.True(fvpContractors.Count > 0);
        }
    }
}
