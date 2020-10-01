using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FvpWebApp.Models
{
    [Table("Targets")]
    public class Target
    {
        [Key]
        public int TargetId { get; set; }
        public string Descryption { get; set; }
        public string DatabaseName { get; set; }
        public string DatabaseAddress { get; set; }
        public string DatabaseUsername { get; set; }
        public string DatabasePassword { get; set; }
    }
}