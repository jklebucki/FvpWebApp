using LinqToDB.Mapping;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace C2FKInterface.Models
{
    [Table(Name = "def_dok", Schema = "FK")]
    public class C21DocumentDefinition
    {
        [Column(Name = "rokId")]
        public short rokId { get; set; }
        [Column(Name = "dSkrot"), MaxLength(4)]
        public string dSkrot { get; set; }
        [Column(Name = "dWzor")]
        public short dWzor { get; set; }
        [PrimaryKey]
        public int id { get; set; }
        [Column(Name = "dNazwa")]
        public string dNazwa { get; set; }
        [Column(Name = "rejestr")]
        public int rejestr { get; set; }
        [Column(Name = "dForm")]
        public short dForm { get; set; }
        [Column(Name = "dZs")]
        public short dZs { get; set; }
        [Column(Name = "dAktywny")]
        public bool dAktywny { get; set; }
        [Column(Name = "autoTrans")]
        public byte autoTrans { get; set; }
        [Column(Name = "terminTrans")]
        public int terminTrans { get; set; }
        [Column(Name = "konfOkres")]
        public byte konfOkres { get; set; }
        [Column(Name = "dFR")]
        public short dFR { get; set; }
        [Column(Name = "korekta")]
        public byte korekta { get; set; }
        [Column(Name = "kontoKasy")]
        public string kontoKasy { get; set; }
        [Column(Name = "dED")]
        public byte dED { get; set; }
        [Column(Name = "VatWal")]
        public byte VatWal { get; set; }
        [Column(Name = "waluta"), MaxLength(3)]
        public string waluta { get; set; }
        [Column(Name = "dUE")]
        public byte dUE { get; set; }
        [Column(Name = "dUsl")]
        public byte dUsl { get; set; }
        [Column(Name = "VatOkres")]
        public byte VatOkres { get; set; }
        [Column(Name = "rejestr2")]
        public int rejestr2 { get; set; }
        [Column(Name = "dZero")]
        public byte dZero { get; set; }
    }
}
