using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FvpWebAppModels.Models
{
    [Table("Contractors")]
    public class Contractor
    {
        [Key]
        public int ContractorId { get; set; }
        public int? ContractorErpId { get; set; }
        public int? ContractorErpPosition { get; set; }
        public int? SourceId { get; set; }
        public int? GusContractorEntriesCount { get; set; }
        public string ContractorSourceId { get; set; }
        public string Name { get; set; }
        public string Street { get; set; }
        public string EstateNumber { get; set; }
        public string QuartersNumber { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Province { get; set; }
        public string VatId { get; set; }
        public string Regon { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string CountryCode { get; set; }
        public Firm Firm { get; set; }
        public ContractorStatus ContractorStatus { get; set; }
        public DateTime? CheckDate { get; set; }
        public ICollection<Document> Documents { get; set; }
    }
}
