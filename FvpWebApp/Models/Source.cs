using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FvpWebApp.Models
{
    [Table("Sources")]
    public class Source
    {
        [Key]
        public int SourceId { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public string Type { get; set; }
        public string Address { get; set; }
        public string DbName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

    }
}