using System;

namespace C2FKInterface.Models
{
    class C21AccountingRecord
    {
        public int id { get; set; }
        public int? dokId { get; set; }
        public int pozycja { get; set; }
        public short rozbicie { get; set; }
        public short strona { get; set; }
        public byte zapisRownolegly { get; set; }
        public double kwota { get; set; }
        public double wkwota { get; set; }
        public string waluta { get; set; }
        public double kurs { get; set; }
        public string opis { get; set; }
        public int synt { get; set; }
        public int poz1 { get; set; }
        public int poz2 { get; set; }
        public int poz3 { get; set; }
        public int poz4 { get; set; }
        public int poz5 { get; set; }
        public int kontoRap { get; set; }
        public DateTime? dataKPKW { get; set; }
        public string numerDok { get; set; }
        public byte TypRozrachunku { get; set; }
        public string dokRozliczany { get; set; }
        public DateTime? terminPlatnosci { get; set; }
        public string wymiar01 { get; set; }
        public string wymiar02 { get; set; }
        public string wymiar03 { get; set; }
        public string wymiar04 { get; set; }
        public string wymiar05 { get; set; }
        public string wymiar06 { get; set; }
        public string wymiar07 { get; set; }
        public string wymiar08 { get; set; }
        public string wymiar09 { get; set; }
        public string wymiar10 { get; set; }
    }
}
