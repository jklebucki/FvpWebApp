using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FvpWebApp.Models
{
    [Table("SourceTypes")]
    public class SourceType
    {
        [Key]
        public int SourceTypeId { get; set; }
        public string Descryption { get; set; }
    }
}