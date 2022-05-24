using C2FKInterface.Data;
using C2FKInterface.Models;
using C2FKInterface.Services.Interfaces;
using LinqToDB;
using LinqToDB.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace C2FKInterface.Services
{
    public class C21ContractorService : IContractorService
    {
        private readonly DbConnectionSettings _dbConnectionSettings;
        public C21ContractorService(DbConnectionSettings dbConnectionSettings)
        {
            _dbConnectionSettings = dbConnectionSettings;
            DataConnection.DefaultSettings = new DbSettings(_dbConnectionSettings.ConnStr());
            DataConnection.SetConnectionString("Db", _dbConnectionSettings.ConnStr());
        }
        public async Task AddContractorsAsync(List<C21Contractor> c21Contractors)
        {
            using (var db = new SageDb("Db"))
            {
                foreach (var contractor in c21Contractors)
                {
                    try
                    {
                        await db.InsertAsync(contractor);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }
            }
        }

        public async Task<List<C21Contractor>> GetC21ContractorsAsync()
        {
            using (var db = new SageDb("Db"))
            {
                return await db.C21Contractors.ToListAsync();
            }
        }

        public async Task<List<C21FvpContractor>> GetC21FvpContractorsAsync(bool active = true)
        {
            await CreateContractorView();
            using (var db = new SageDb("Db"))
            {
                if (active)
                    return await db.C21FvpContractors.Where(a => a.Active).ToListAsync();
                else
                    return await db.C21FvpContractors.ToListAsync();
            }
        }

        public async Task<List<string>> ProceedContractorsAsync(int debug = 1)
        {
            List<string> procOutput = new List<string>();
            using (var db = new SageDb("Db"))
            {
                db.CommandTimeout = 0;
                var response = await db.QueryProcAsync<string>(
                    "[FK].[sp_C21_importKH]",
                    new DataParameter("Debug", debug, DataType.Int32)
                    ).ConfigureAwait(false);
                procOutput = response.ToList();
            }
            return procOutput;
        }

        public async Task<string> CreateContractorView()
        {
            var procOutput = "";
            using (var db = new SageDb("Db"))
            {
                db.CommandTimeout = 0;
                var response = await db.QueryToListAsync<bool>(
                    "SELECT CASE WHEN EXISTS(select * FROM sys.views where name = 'C21_FVP_Contractors') THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END"
                    ).ConfigureAwait(false);

                if (!response[0])
                {
                    var viewDef =
                                " CREATE VIEW [FK].[C21_FVP_Contractors]" + Environment.NewLine +
                                " AS" + Environment.NewLine +
                                " SELECT [fk_kontrahenci].[pozycja] as FkId" + Environment.NewLine +
                                "	,[fk_kontrahenci].[id] as Id" + Environment.NewLine +
                                "	,[fk_kontrahenci].[skrot] as Shortcut" + Environment.NewLine +
                                "	,[fk_kontrahenci].[nazwa] as Name" + Environment.NewLine +
                                "	,[fk_kontrahenci].[nip] as VatId" + Environment.NewLine +
                                "	,[fk_kontrahenci].[ulica] as [Street]" + Environment.NewLine +
                                "	,[fk_kontrahenci].[numerDomu] as [HouseNo]" + Environment.NewLine +
                                "	,[fk_kontrahenci].[numerMieszk] as [ApartmentNo]" + Environment.NewLine +
                                "	,[fk_kontrahenci].[Miejscowosc] as Place" + Environment.NewLine +
                                "	,[fk_kontrahenci].[kod] as [PostCode]" + Environment.NewLine +
                                "	,[fk_kontrahenci].[kodKraju] as [Country]" + Environment.NewLine +
                                "	,cast(null as uniqueidentifier) as [BankingInfoGuid]" + Environment.NewLine +
                                "	,isnull([fk_kontrahenci].[aktywny],0) as [Active]" + Environment.NewLine +
                                " FROM fk.[fk_kontrahenci]";
                    var createResponse = await db.QueryToListAsync<string>(viewDef).ConfigureAwait(false);
                    procOutput = "View created";
                }
                else
                    procOutput = "View exist";
            }
            return procOutput;
        }
    }
}
