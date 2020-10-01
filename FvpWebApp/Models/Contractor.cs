using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BIWebApi.Models
{
    [Table("Contractors")]
    public class Contractor
    {
        [Key]
        public int ContractorId { get; set; }
        public int ContractorErpId { get; set; }
        public int ContractorSourceId { get; set; }
        public string Name { get; set; }
        public string VatCode { get; set; }
        public string City { get; set; }
        public string PostCode { get; set; }
        public string CountryCode { get; set; }
        public string StreetAndNumber { get; set; }
        public bool ContractorValid { get; set; }
        public DateTime? CheckDate { get; set; }

    }
}
