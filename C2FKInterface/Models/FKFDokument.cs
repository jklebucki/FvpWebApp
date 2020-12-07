using LinqToDB.Mapping;
using System;

namespace C2FKInterface.Models
{
    [Table(Name = "dokumenty", Schema = "FK")]
    public class FKFDokument
    {
        [Column(Name = "rokId")]
        public short rokId { get; set; }
        [Column(Name = "zrodlo")]
        public int zrodlo { get; set; }
        [Column(Name = "id")]
        public int id { get; set; }
        [Column(Name = "skrot")]
        public string skrot { get; set; }
        [Column(Name = "tabela")]
        public int? tabela { get; set; }
        [Column(Name = "waluta")]
        public string waluta { get; set; }
        [Column(Name = "okresDK")]
        public short okresDK { get; set; }
        [Column(Name = "kontrahent")]
        public int? kontrahent { get; set; }
        [Column(Name = "kontrahentStalyId")]
        public int? kontrahentStalyId { get; set; }
        [Column(Name = "kontrahentJednId")]
        public int? kontrahentJednId { get; set; }
        [Column(Name = "nazwa")]
        public string nazwa { get; set; }
        [Column(Name = "numer")]
        public int numer { get; set; }
        [Column(Name = "wzorzec")]
        public short wzorzec { get; set; }
        [Column(Name = "idLog")]
        public int? idLog { get; set; }
        [Column(Name = "errors")]
        public int errors { get; set; }
        [Column(Name = "tresc")]
        public string tresc { get; set; }
        [Column(Name = "datawpr")]
        public DateTime datawpr { get; set; }
        [Column(Name = "datadok")]
        public DateTime? datadok { get; set; }
        [Column(Name = "dataokr")]
        public DateTime? dataokr { get; set; }
        [Column(Name = "dataOper")]
        public DateTime? dataOper { get; set; }
        [Column(Name = "datawpl")]
        public DateTime? datawpl { get; set; }
        [Column(Name = "idRozrach")]
        public int idRozrach { get; set; }
        [Column(Name = "kwota")]
        public double kwota { get; set; }
        [Column(Name = "wkwota")]
        public double wkwota { get; set; }
        [Column(Name = "kwotaZRow")]
        public double kwotaZRow { get; set; }
        [Column(Name = "typkursu")]
        public int typkursu { get; set; }
        [Column(Name = "nazwaKor")]
        public string nazwaKor { get; set; }
        [Column(Name = "dataKor")]
        public DateTime? dataKor { get; set; }
        [Column(Name = "sygnatura")]
        public string sygnatura { get; set; }
        [Column(Name = "nip")]
        public string nip { get; set; }
        [Column(Name = "automat")]
        public short automat { get; set; }
        [Column(Name = "flag")]
        public short flag { get; set; }
        [Column(Name = "numerDK")]
        public int numerDK { get; set; }
        [Column(Name = "saldoPoczRK")]
        public double saldoPoczRK { get; set; }
        [Column(Name = "saldoZapRK")]
        public double saldoZapRK { get; set; }
        [Column(Name = "przeksKR")]
        public short przeksKR { get; set; }
        [Column(Name = "kwotaPozaRej")]
        public double kwotaPozaRej { get; set; }
        [Column(Name = "kurs")]
        public double kurs { get; set; }
        [Column(Name = "kursEuro")]
        public double kursEuro { get; set; }
        [Column(Name = "numerKsiegowania")]
        public int? numerKsiegowania { get; set; }
        [Column(Name = "kontrahentTT")]
        public int? kontrahentTT { get; set; }
        [Column(Name = "uniqueZrodloNotKsiegi")]
        public Guid? uniqueZrodloNotKsiegi { get; set; }
        [Column(Name = "uniqueZrodloWzorce")]
        public Guid? uniqueZrodloWzorce { get; set; }
        [Column(Name = "uniqueKontrahent")]
        public Guid? uniqueKontrahent { get; set; }
        [Column(Name = "spid")]
        public short? spid { get; set; }
        [Column(Name = "flagi")]
        public int flagi { get; set; }
        [Column(Name = "kwotaDZPB")]
        public double? kwotaDZPB { get; set; }
        [Column(Name = "znacznik")]
        public string znacznik { get; set; }
        [Column(Name = "templateId")]
        public int templateId { get; set; }
        [Column(Name = "uid")]
        public Guid uid { get; set; }
        [Column(Name = "okres")]
        public short? okres { get; set; }
        [Column(Name = "eStatus")]
        public short eStatus { get; set; }
        [Column(Name = "SDstatus")]
        public short SDstatus { get; set; }
        [Column(Name = "danekh")]
        public int? danekh { get; set; }
        [Column(Name = "rodzajDK")]
        public short? rodzajDK { get; set; }
        [Column(Name = "mppFlags")]
        public short? mppFlags { get; set; }
        [Column(Name = "atrJpkV7")]
        public string atrJpkV7 { get; set; }

    }
}