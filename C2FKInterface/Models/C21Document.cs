using LinqToDB.Mapping;
using System;

namespace C2FKInterface.Models
{
    [Table(Name = "C21_dokumenty", Schema = "FK")]
    public class C21Document
    {
        [PrimaryKey]
        public int id { get; set; }
        [Column(Name = "rokId")]
        public short rokId { get; set; }
        [Column(Name = "skrot")]
        public string skrot { get; set; }
        [Column(Name = "numer")]
        public int? numer { get; set; }
        [Column(Name = "kontrahent")]
        public int? kontrahent { get; set; }
        [Column(Name = "nazwa")]
        public string nazwa { get; set; }
        [Column(Name = "tresc")]
        public string tresc { get; set; }
        [Column(Name = "datawpr")]
        public DateTime datawpr { get; set; }
        [Column(Name = "datadok")]
        public DateTime? datadok { get; set; }
        [Column(Name = "dataOper")]
        public DateTime? dataOper { get; set; }
        [Column(Name = "datawpl")]
        public DateTime? datawpl { get; set; }
        [Column(Name = "kwota")]
        public double kwota { get; set; }
        [Column(Name = "wkwota")]
        public double wkwota { get; set; }
        [Column(Name = "waluta")]
        public string waluta { get; set; }
        [Column(Name = "kurs")]
        public double kurs { get; set; }
        [Column(Name = "nazwaKor")]
        public string nazwaKor { get; set; }
        [Column(Name = "dataKor")]
        public DateTime? dataKor { get; set; }
        [Column(Name = "sygnatura")]
        public string sygnatura { get; set; }
        [Column(Name = "saldoPoczRK")]
        public double? saldoPoczRK { get; set; }
        [Column(Name = "saldoZapRK")]
        public double? saldoZapRK { get; set; }
        [Column(Name = "kwotaPozaRej")]
        public double? kwotaPozaRej { get; set; }
        [Column(Name = "TypRozrachunku")]
        public byte TypRozrachunku { get; set; }
        [Column(Name = "dokRozliczany")]
        public string dokRozliczany { get; set; }
        [Column(Name = "terminPlatnosci")]
        public DateTime? terminPlatnosci { get; set; }
        [Column(Name = "status")]
        public int? status { get; set; }
        [Column(Name = "errorInfo")]
        public string errorInfo { get; set; }
        [Column(Name = "mppFlags")]
        public short? mppFlags { get; set; }
        [Column(Name = "kontoplatnosci")]
        public string kontoplatnosci { get; set; }
        [Column(Name = "atrJpkV7")]
        public string atrJpkV7 { get; set; }
    }
}
