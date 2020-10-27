using System;
using System.Collections.Generic;
using System.Text;

namespace C2FKInterface.Models
{
    public class C21DocumentAggregate
    {
        public C21Document Document { get; set; }
        public List<C21AccountingRecord> AccountingRecords { get; set; }
        public List<C21VatRegister> VatRegisters { get; set; }
    }
}
