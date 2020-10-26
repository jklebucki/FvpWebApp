using LinqToDB.Mapping;
using System;

namespace C2FKInterface.Models
{
    [Table(Name = "C21_rejVat", Schema = "FK")]
    class C21VatRegister
    {
        [PrimaryKey]
        public int id { get; set; }
        [Column(Name = "dokId")]
        public int dokId { get; set; }
        [Column(Name = "rejId")]
        public int? rejId { get; set; }
        [Column(Name = "okres")]
        public DateTime? okres { get; set; }
        [Column(Name = "Oczek")]
        public int Oczek { get; set; }
        [Column(Name = "abc")]
        public int abc { get; set; }
        [Column(Name = "nienaliczany")]
        public short nienaliczany { get; set; }
        [Column(Name = "stawka")]
        public double stawka { get; set; }
        [Column(Name = "brutto")]
        public double brutto { get; set; }
        [Column(Name = "netto")]
        public double netto { get; set; }
        [Column(Name = "vat")]
        public double vat { get; set; }
        [Column(Name = "znacznik")]
        public string znacznik { get; set; }
        [Column(Name = "ue")]
        public byte ue { get; set; }
        [Column(Name = "usluga")]
        public byte usluga { get; set; }
        [Column(Name = "bruttoWaluta")]
        public double bruttoWaluta { get; set; }
        [Column(Name = "nettoWaluta")]
        public double nettoWaluta { get; set; }
        [Column(Name = "vatWaluta")]
        public double vatWaluta { get; set; }
        [Column(Name = "atrJpkV7")]
        public string atrJpkV7 { get; set; }
    }
}
