using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FvpWebAppModels.Models
{
    [Table("AccountingRecords")]
    public class AccountingRecord
    {
        [Key]
        public int AccountingRecordId { get; set; }
        public int SourceId { get; set; }
        public int RecordOrder { get; set; }
        public string Account { get; set; }
        public string DebitCredit { get; set; }
        public char Sign { get; set; }
    }
}
