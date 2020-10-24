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
            using (var db = new SageDb("Db"))
            {
                if (active)
                    return await db.C21FvpContractors.Where(a => a.Active).ToListAsync();
                else
                    return await db.C21FvpContractors.ToListAsync();
            }
        }

        public async Task<List<C21Contractor>> GetC21ContractorsAsync(string vatId)
        {
            using (var db = new SageDb("Db"))
            {
                return await db.C21Contractors.ToListAsync();
            }
        }

        public async Task<List<string>> ProceedContractorsAsync(int debug = 1)
        {
            List<string> procOutput = new List<string>();
            using (var db = new SageDb("Db"))
            {
                var response = await db.QueryProcAsync<string>(
                    "[FK].[sp_C21_importKH]",
                    new DataParameter("Debug", debug, DataType.Int32)
                    ).ConfigureAwait(false);
                procOutput = response.ToList();
            }
            return procOutput;
        }
    }
}
