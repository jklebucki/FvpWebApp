using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FvpWebAppModels.Models
{
    [Table("TargetDocumentsSettings")]
    public class TargetDocumentSettings
    {
        [Key]
        public int TargetDocumentSettingsId { get; set; }
        public int SourceId { get; set; }
        public string DocumentShortcut { get; set; }
        public int VatRegisterId { get; set; }
    }
}
