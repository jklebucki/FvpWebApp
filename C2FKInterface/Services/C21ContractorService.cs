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
            var tryCounts = 0;
            List<C21FvpContractor> c21FvpContractors = new List<C21FvpContractor>();
            var ex = new Exception();
            while (tryCounts < 10)
            {
                try
                {
                    using (var db = new SageDb("Db"))
                    {
                        if (active)
                            c21FvpContractors = await db.C21FvpContractors.Where(a => a.Active).ToListAsync();
                        else
                            c21FvpContractors = await db.C21FvpContractors.ToListAsync();
                    }
                }
                catch (Exception e)
                {
                    ex = e;
                }

                if (c21FvpContractors.Count > 0)
                    break;
            }//Problem z widokiem C21_FVP_Contractors - Nie zawsze działa dlatego jest max 10 powtórzeń.

            if (c21FvpContractors.Count == 0)
                throw ex;
            return c21FvpContractors;

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
                                " SELECT" + Environment.NewLine +
                                " cast(Elements1.[AccountNo] as int) AS FkId, Con.Id, Con.Shortcut, Con.Name, Con.NIP As VatId, Po.[Street]," + Environment.NewLine +
                                " Po.[HouseNo], Po.[ApartmentNo], Po.[Place]," + Environment.NewLine +
                                " Po.[PostCode], Po.[Country], Con.[BankingInfoGuid], Elements1.[Active]" + Environment.NewLine +
                                " FROM SSCommon.[STContractors] as Con   INNER JOIN SSCommon.[STElements] as Elements1 ON(Elements1.[Guid] = Con.[MainElement])" + Environment.NewLine +
                                " LEFT OUTER JOIN SSCommon.[STContacts] as Contacts4 ON(Contacts4.[Guid] = Con.[ContactGuid])" + Environment.NewLine +
                                " LEFT OUTER JOIN SSCommon.[STPostOfficeAddresses] as Po ON(Po.[Guid] = Contacts4.[MainPostOfficeAddress])";
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
