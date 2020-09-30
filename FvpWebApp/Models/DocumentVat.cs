using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FvpWebApp.Models
{
    [Table("DocumentVats")]
    public class DocumentVat
    {
        [Key]
        public int DocumentVatId { get; set; }
        public string VatCode { get; set; }
        public decimal VatValue { get; set; }
        public decimal VatAmount { get; set; }
    }
}