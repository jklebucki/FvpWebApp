using C2FKInterface.Models;
using LinqToDB;

namespace C2FKInterface.Data
{
    class SageDb : LinqToDB.Data.DataConnection
    {
        public SageDb(string connectionString) : base(connectionString) { }

        public ITable<C21AccountingRecord> C21AccountingRecords => GetTable<C21AccountingRecord>();
        public ITable<C21Contractor> C21Contractors => GetTable<C21Contractor>();
        public ITable<C21VatRegister> C21VatRegisters => GetTable<C21VatRegister>();
        public ITable<C21FvpContractor> C21FvpContractors => GetTable<C21FvpContractor>();
        public ITable<C21Document> C21Documents => GetTable<C21Document>();
        public ITable<C21Year> C21Years => GetTable<C21Year>();
        public ITable<C21VatRegisterDef> C21VatRegisterDefs => GetTable<C21VatRegisterDef>();
        public ITable<C21DocumentDefinition> C21DocumentDefinitions => GetTable<C21DocumentDefinition>();
    }
}
