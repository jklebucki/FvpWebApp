using LinqToDB.Mapping;
using System;

namespace C2FKInterface.Models
{
    [Table(Name = "C21_FVP_Contractors", Schema = "FK")]
    public class C21FvpContractor
    {
        [Column(Name = "FkId")]
        public int FkId { get; set; }
        [PrimaryKey]
        public int Id { get; set; }
        [Column(Name = "Name")]
        public string Name { get; set; }
        [Column(Name = "VatId")]
        public string VatId { get; set; }
        [Column(Name = "Street")]
        public string Street { get; set; }
        [Column(Name = "HouseNo")]
        public string HouseNo { get; set; }
        [Column(Name = "ApartmentNo")]
        public string ApartmentNo { get; set; }
        [Column(Name = "Place")]
        public string Place { get; set; }
        [Column(Name = "PostCode")]
        public string PostCode { get; set; }
        [Column(Name = "Country")]
        public string Country { get; set; }
        [Column(Name = "BankingInfoGuid")]
        public Guid? BankingInfoGuid { get; set; }
        [Column(Name = "Active")]
        public bool Active { get; set; }
    }
}

