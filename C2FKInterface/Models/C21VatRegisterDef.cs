using LinqToDB.Mapping;

namespace C2FKInterface.Models
{
    [Table(Name = "fk_defRej", Schema = "FK")]
    public class C21VatRegisterDef
    {
        [PrimaryKey]
        public int id { get; set; }
        [Column(Name = "rNazwa")]
        public string rNazwa { get; set; }
        [Column(Name = "rZs")]
        public short rZs { get; set; }
        [Column(Name = "rFr")]
        public short rFr { get; set; }
        [Column(Name = "srTrwale")]
        public short srTrwale { get; set; }
        [Column(Name = "zablok")]
        public short zablok { get; set; }
        [Column(Name = "defAbc")]
        public short defAbc { get; set; }
        [Column(Name = "defStawka")]
        public double defStawka { get; set; }
        [Column(Name = "warunkowy")]
        public bool warunkowy { get; set; }
        [Column(Name = "okresWarunkowy")]
        public short? okresWarunkowy { get; set; }
        [Column(Name = "okresPodst")]
        public short? okresPodst { get; set; }
        [Column(Name = "wykazuj")]
        public short wykazuj { get; set; }
        [Column(Name = "Naliczany")]
        public int? Naliczany { get; set; }
    }
}
