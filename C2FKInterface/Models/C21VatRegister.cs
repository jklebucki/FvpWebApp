using System;

namespace C2FKInterface.Models
{
    class C21VatRegister
    {
        public int id { get; set; }
        public int dokId { get; set; }
        public int? rejId { get; set; }
        public DateTime? okres { get; set; }
        public int Oczek { get; set; }
        public int abc { get; set; }
        public short nienaliczany { get; set; }
        public double stawka { get; set; }
        public double brutto { get; set; }
        public double netto { get; set; }
        public double vat { get; set; }
        public string znacznik { get; set; }
        public byte ue { get; set; }
        public byte usluga { get; set; }
        public double bruttoWaluta { get; set; }
        public double nettoWaluta { get; set; }
        public double vatWaluta { get; set; }
        public string atrJpkV7 { get; set; }
    }
}
