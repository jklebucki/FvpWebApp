using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FvpWebAppModels.Models
{
    [Table("Documents")]
    public class Document
    {
        [Key]
        public int DocumentId { get; set; }
        public int SourceId { get; set; }
        public int ExternalId { get; set; }
        public string DocumentNumber { get; set; }
        public string DocumentSymbol { get; set; }
        public DateTime SaleDate { get; set; }
        public DateTime DocumentDate { get; set; }
        public decimal Net { get; set; }
        public decimal Gross { get; set; }
        public decimal Vat { get; set; }
        public bool DocumentValid { get; set; }
        public string DocContractorId { get; set; }
        public string DocContractorName { get; set; }
        public string DocContractorVatCode { get; set; }
        public string DocContractorCity { get; set; }
        public string DocContractorPostCode { get; set; }
        public string DocContractorCountryCode { get; set; }
        public string DocContractorStreetAndNumber { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public ICollection<DocumentVat> DocumentVats { get; set; }

    }
}