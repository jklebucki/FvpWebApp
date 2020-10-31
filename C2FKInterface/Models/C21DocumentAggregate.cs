using System.Collections.Generic;
using System.Diagnostics;

namespace C2FKInterface.Models
{
    public class C21DocumentAggregate
    {
        public bool IsPrepared { get; set; }
        public List<KeyValuePair<string, string>> Messages { get; set; }
        public C21Document Document { get; set; }
        public List<C21AccountingRecord> AccountingRecords { get; set; }
        public List<C21VatRegister> VatRegisters { get; set; }
        public C21DocumentAggregate()
        {
            IsPrepared = true;
            Messages = new List<KeyValuePair<string, string>>();
            AccountingRecords = new List<C21AccountingRecord>();
            VatRegisters = new List<C21VatRegister>();
        }
    }
}
