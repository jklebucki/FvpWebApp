using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FvpWebApp.Models
{
    [Table("Documents")]
    public class Document
    {
        [Key]
        public int DocumentId { get; set; }
        public int SourceId { get; set; }
        public int ExternalId { get; set; }
        public int ContractorId { get; set; }
        public string DocumentNumber { get; set; }
        public string DocumentSymbol { get; set; }
        public DateTime SaleDate { get; set; }
        public DateTime DocumentDate { get; set; }
        public decimal Net { get; set; }
        public decimal Gross { get; set; }
        public decimal Vat { get; set; }
        public bool Valid { get; set; }

    }
}