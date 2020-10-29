using LinqToDB.Mapping;
using System;
using System.Collections.Generic;
using System.Text;

namespace C2FKInterface.Models
{
    [Table(Name = "FROK", Schema = "FK")]
    public class C21Year
    {
        [PrimaryKey]
        public short rokId { get; set; }
        [Column(Name = "katalog")]
        public string katalog { get; set; }
        [Column(Name = "zamkniety")]
        public byte zamkniety { get; set; }
        [Column(Name = "archiw")]
        public byte archiw { get; set; }
        [Column(Name = "mcZamkniety")]
        public byte mcZamkniety { get; set; }
        [Column(Name = "bilansOtw")]
        public byte bilansOtw { get; set; }
        [Column(Name = "obrotyRozp")]
        public byte obrotyRozp { get; set; }
        [Column(Name = "rozrachunki")]
        public byte rozrachunki { get; set; }
        [Column(Name = "ksAutomat")]
        public byte ksAutomat { get; set; }
        [Column(Name = "koszty")]
        public short koszty { get; set; }
        [Column(Name = "poczatek")]
        public DateTime poczatek { get; set; }
        [Column(Name = "koniec")]
        public DateTime koniec { get; set; }
        [Column(Name = "dlugosc")]
        public short dlugosc { get; set; }
        [Column(Name = "waluty")]
        public byte waluty { get; set; }
        [Column(Name = "Gus1")]
        public double Gus1 { get; set; }
        [Column(Name = "Gus2")]
        public double Gus2 { get; set; }
        [Column(Name = "W1")]
        public double W1 { get; set; }
        [Column(Name = "W2")]
        public double W2 { get; set; }
        [Column(Name = "W3")]
        public double W3 { get; set; }
        [Column(Name = "ZZl")]
        public double ZZl { get; set; }
        [Column(Name = "ZVat")]
        public double ZVat { get; set; }
        [Column(Name = "ZR1")]
        public double ZR1 { get; set; }
        [Column(Name = "ZR2")]
        public double ZR2 { get; set; }
        [Column(Name = "ZWal")]
        public double ZWal { get; set; }
        [Column(Name = "regulaRK")]
        public short regulaRK { get; set; }
        [Column(Name = "validBO")]
        public short validBO { get; set; }
        [Column(Name = "niePrzelicz")]
        public short niePrzelicz { get; set; }
        [Column(Name = "kwotaPrzeks")]
        public double kwotaPrzeks { get; set; }
        [Column(Name = "okresOR")]
        public short okresOR { get; set; }
        [Column(Name = "programOK")]
        public short programOK { get; set; }
        [Column(Name = "miejsceRK")]
        public byte miejsceRK { get; set; }
        [Column(Name = "dokumentRK")]
        public string dokumentRK { get; set; }
        [Column(Name = "dokumentKMP")]
        public string dokumentKMP { get; set; }
        [Column(Name = "przeksRW")]
        public byte przeksRW { get; set; }
        [Column(Name = "ORnaKontach")]
        public bool ORnaKontach { get; set; }
        [Column(Name = "wlasnosc")]
        public short wlasnosc { get; set; }
        [Column(Name = "typDzial")]
        public short typDzial { get; set; }
        [Column(Name = "numeracjaDK")]
        public byte numeracjaDK { get; set; }
        [Column(Name = "statusP")]
        public short statusP { get; set; }
    }
}
