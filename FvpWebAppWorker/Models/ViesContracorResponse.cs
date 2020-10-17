using System;

namespace FvpWebAppWorker.Models
{
    public class ViesContractorResponse
    {
        public string CountryCode { get; set; }
        public string VatNumber { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string CompanyType { get; set; }
        public DateTime CheckDate { get; set; }
        public bool Status { get; set; }
        public string Identifer { get; set; }
    }
}