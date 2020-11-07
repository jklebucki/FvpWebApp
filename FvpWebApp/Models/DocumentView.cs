using FvpWebAppModels.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FvpWebApp.Models
{
    public class DocumentView
    {
        public int DocumentId { get; set; }
        public string SourceDescription { get; set; }
        [Display(Name = "Numer dokumentu")]
        public string DocumentNumber { get; set; }
        public string DocumentSymbol { get; set; }
        public string JpkV7 { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime SaleDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime DocumentDate { get; set; }
        public decimal Net { get; set; }
        public decimal Gross { get; set; }
        public decimal Vat { get; set; }
        public DocumentStatus DocumentStatus { get; set; }
        public string ContractorName { get; set; }
        public string ContractorVatId { get; set; }
        public string ContractorCountryCode { get; set; }
        public ContractorStatus ContractorStatus { get; set; }
    }
}
