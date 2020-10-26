using LinqToDB.Mapping;
using System;

namespace C2FKInterface.Models
{
    [Table(Name = "C21_zapisy", Schema = "FK")]
    class C21AccountingRecord
    {
        [PrimaryKey]
        public int id { get; set; }
        [Column(Name = "dokId")]
        public int? dokId { get; set; }
        [Column(Name = "pozycja")]
        public int pozycja { get; set; }
        [Column(Name = "rozbicie")]
        public short rozbicie { get; set; }
        [Column(Name = "strona")]
        public short strona { get; set; }
        [Column(Name = "zapisRownolegly")]
        public byte zapisRownolegly { get; set; }
        [Column(Name = "kwota")]
        public double kwota { get; set; }
        [Column(Name = "wkwota")]
        public double wkwota { get; set; }
        [Column(Name = "waluta")]
        public string waluta { get; set; }
        [Column(Name = "kurs")]
        public double kurs { get; set; }
        [Column(Name = "opis")]
        public string opis { get; set; }
        [Column(Name = "synt")]
        public int synt { get; set; }
        [Column(Name = "poz1")]
        public int poz1 { get; set; }
        [Column(Name = "poz2")]
        public int poz2 { get; set; }
        [Column(Name = "poz3")]
        public int poz3 { get; set; }
        [Column(Name = "poz4")]
        public int poz4 { get; set; }
        [Column(Name = "poz5")]
        public int poz5 { get; set; }
        [Column(Name = "kontoRap")]
        public int kontoRap { get; set; }
        [Column(Name = "dataKPKW")]
        public DateTime? dataKPKW { get; set; }
        [Column(Name = "numerDok")]
        public string numerDok { get; set; }
        [Column(Name = "TypRozrachunku")]
        public byte TypRozrachunku { get; set; }
        [Column(Name = "dokRozliczany")]
        public string dokRozliczany { get; set; }
        [Column(Name = "terminPlatnosci")]
        public DateTime? terminPlatnosci { get; set; }
        [Column(Name = "wymiar01")]
        public string wymiar01 { get; set; }
        [Column(Name = "wymiar02")]
        public string wymiar02 { get; set; }
        [Column(Name = "wymiar03")]
        public string wymiar03 { get; set; }
        [Column(Name = "wymiar04")]
        public string wymiar04 { get; set; }
        [Column(Name = "wymiar05")]
        public string wymiar05 { get; set; }
        [Column(Name = "wymiar06")]
        public string wymiar06 { get; set; }
        [Column(Name = "wymiar07")]
        public string wymiar07 { get; set; }
        [Column(Name = "wymiar08")]
        public string wymiar08 { get; set; }
        [Column(Name = "wymiar09")]
        public string wymiar09 { get; set; }
        [Column(Name = "wymiar10")]
        public string wymiar10 { get; set; }
    }
}
