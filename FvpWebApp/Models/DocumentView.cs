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
        public string DocumentNumber { get; set; }
        public string DocumentSymbol { get; set; }
        public string JpkV7 { get; set; }
        public DateTime SaleDate { get; set; }
        public DateTime DocumentDate { get; set; }
        public decimal Net { get; set; }
        public decimal Gross { get; set; }
        public decimal Vat { get; set; }
        public DocumentStatus DocumentStatus { get; set; }
        public int ContractorId { get; set; }
        public string ContractorName { get; set; }
        public string ContractorVatId { get; set; }
        public string ContractorCountryCode { get; set; }
        public ContractorStatus ContractorStatus { get; set; }
        public DocumentView()
        {

        }
        public DocumentView(Document document, Source source, Contractor contractor)
        {
            DocumentId = document.DocumentId;
            DocumentDate = document.DocumentDate;
            SourceDescription = source.Code;
            SaleDate = document.SaleDate;
            DocumentNumber = document.DocumentNumber;
            DocumentStatus = document.DocumentStatus;
            DocumentSymbol = document.DocumentSymbol;
            JpkV7 = document.JpkV7;
            ContractorId = contractor.ContractorId;
            ContractorName = contractor.Name;
            ContractorVatId = contractor.VatId;
            ContractorCountryCode = contractor.CountryCode;
            ContractorStatus = contractor.ContractorStatus;
            Net = document.Net;
            Vat = document.Vat;
            Gross = document.Gross;
        }
    }
}
