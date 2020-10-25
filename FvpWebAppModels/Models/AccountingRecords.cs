using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FvpWebAppModels.Models
{
    [Table("AccountingRecords")]
    class AccountingRecords
    {
        [Key]
        public int AccountingRecordsId { get; set; }
        public string Account { get; set; }
        public string Debit { get; set; }
        public string Credit { get; set; }
        public char Sign { get; set; }
        public int SourceId { get; set; }
    }
}
