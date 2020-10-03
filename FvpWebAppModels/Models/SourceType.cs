using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FvpWebAppModels.Models
{
    [Table("SourceTypes")]
    public class SourceType
    {
        [Key]
        public int SourceTypeId { get; set; }
        public string Descryption { get; set; }
    }
}