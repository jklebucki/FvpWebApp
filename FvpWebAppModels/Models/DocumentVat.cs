using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FvpWebAppModels.Models
{
    [Table("DocumentVats")]
    public class DocumentVat
    {
        [Key]
        public int DocumentVatId { get; set; }
        public string VatCode { get; set; }
        public decimal VatValue { get; set; }
        public decimal VatAmount { get; set; }
        public decimal NetAmount { get; set; }
        public decimal GrossAmount { get; set; }
        public string VatTags { get; set; }
    }
}