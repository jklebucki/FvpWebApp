using LinqToDB.Mapping;
using System;

namespace C2FKInterface.Models
{

    [Table(Name = "C21_kontrahenci", Schema = "FK")]
    public class C21Contractor
    {
        [PrimaryKey]
        public int? id { get; set; }
        [Column(Name = "skrot")]
        public string skrot { get; set; }
        [Column(Name = "nazwa")]
        public string nazwa { get; set; }
        [Column(Name = "Kraj")]
        public string Kraj { get; set; }
        [Column(Name = "Miejscowosc")]
        public string Miejscowosc { get; set; }
        [Column(Name = "gmina")]
        public string gmina { get; set; }
        [Column(Name = "ulica")]
        public string ulica { get; set; }
        [Column(Name = "numerDomu")]
        public string numerDomu { get; set; }
        [Column(Name = "numerMieszk")]
        public string numerMieszk { get; set; }
        [Column(Name = "kod")]
        public string kod { get; set; }
        [Column(Name = "poczta")]
        public string poczta { get; set; }
        [Column(Name = "Telefon1")]
        public string Telefon1 { get; set; }
        [Column(Name = "Telefon2")]
        public string Telefon2 { get; set; }
        [Column(Name = "Telefax")]
        public string Telefax { get; set; }
        [Column(Name = "Telex")]
        public string Telex { get; set; }
        [Column(Name = "email")]
        public string email { get; set; }
        [Column(Name = "PlatnikVAT"), Nullable]
        public byte? PlatnikVAT { get; set; }
        [Column(Name = "naglowek")]
        public string naglowek { get; set; }
        [Column(Name = "nazwisko")]
        public string nazwisko { get; set; }
        [Column(Name = "imie")]
        public string imie { get; set; }
        [Column(Name = "pracownik")]
        public int? pracownik { get; set; }
        [Column(Name = "typ"), Nullable]
        public byte? typ { get; set; }
        [Column(Name = "nip")]
        public string nip { get; set; }
        [Column(Name = "pesel")]
        public string pesel { get; set; }
        [Column(Name = "Regon")]
        public string Regon { get; set; }
        [Column(Name = "zaufanie"), Nullable]
        public short? zaufanie { get; set; }
        [Column(Name = "Uwagi")]
        public string Uwagi { get; set; }
        [Column(Name = "externalId")]
        public string externalId { get; set; }
        [Column(Name = "sygnaturaW")]
        public string sygnaturaW { get; set; }
        [Column(Name = "sygnaturaM")]
        public string sygnaturaM { get; set; }
        [Column(Name = "datawpr"), Nullable]
        public DateTime? datawpr { get; set; }
        [Column(Name = "datamod"), Nullable]
        public DateTime? datamod { get; set; }
        [Column(Name = "www")]
        public string www { get; set; }
        [Column(Name = "rachunekId")]
        public int? rachunekId { get; set; }
        [Column(Name = "idKraj"), Nullable]
        public int? idKraj { get; set; }
        [Column(Name = "statusUE")]
        public short statusUE { get; set; }
        [Column(Name = "znacznik")]
        public string znacznik { get; set; }
        [Column(Name = "limit")]
        public bool limit { get; set; }
        [Column(Name = "limitKwota")]
        public double limitKwota { get; set; }
        [Column(Name = "limitWaluta")]
        public string limitWaluta { get; set; }
        [Column(Name = "typPPFnal"), Nullable]
        public short? typPPFnal { get; set; }
        [Column(Name = "wskPPFnal"), Nullable]
        public double? wskPPFnal { get; set; }
        [Column(Name = "typPPFzob"), Nullable]
        public short? typPPFzob { get; set; }
        [Column(Name = "wskPPFzob"), Nullable]
        public double? wskPPFzob { get; set; }
        [Column(Name = "pozycja"), Nullable]
        public int? pozycja { get; set; }
        [Column(Name = "rodzaj"), Nullable]
        public int? rodzaj { get; set; }
        [Column(Name = "katalog"), Nullable]
        public int? katalog { get; set; }
        [Column(Name = "aktywny")]
        public bool aktywny { get; set; }
        [Column(Name = "negoc")]
        public short negoc { get; set; }
        [Column(Name = "rejon")]
        public string rejon { get; set; }
        [Column(Name = "flag"), Nullable]
        public short? flag { get; set; }
        [Column(Name = "super"), Nullable]
        public int? super { get; set; }
        [Column(Name = "status"), Nullable]
        public int? status { get; set; }
        [Column(Name = "mpp")]
        public int mpp { get; set; }
        [Column(Name = "LinkedUnit")]
        public bool LinkedUnit { get; set; }
    }
}
