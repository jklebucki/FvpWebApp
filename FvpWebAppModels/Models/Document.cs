using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FvpWebAppModels.Models
{
    [Table("Documents")]
    public class Document
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key]
        public int DocumentId { get; set; }
        public int ExternalId { get; set; }
        public int? SourceId { get; set; }
        public int? ContractorId { get; set; }
        public int TaskTicketId { get; set; }
        public string DocumentNumber { get; set; }
        public string DocumentSymbol { get; set; }
        public DateTime SaleDate { get; set; }
        public DateTime DocumentDate { get; set; }
        public decimal Net { get; set; }
        public decimal Gross { get; set; }
        public decimal Vat { get; set; }
        public string JpkV7 { get; set; }
        public DocumentStatus DocumentStatus { get; set; }
        public string DocContractorId { get; set; }
        public string DocContractorName { get; set; }
        public string DocContractorVatId { get; set; }
        public string DocContractorCity { get; set; }
        public string DocContractorPostCode { get; set; }
        public string DocContractorCountryCode { get; set; }
        public string DocContractorStreetAndNumber { get; set; }
        public int DocContractorFirm { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public ICollection<DocumentVat> DocumentVats { get; set; }

    }
}