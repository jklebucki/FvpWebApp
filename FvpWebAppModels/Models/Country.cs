using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FvpWebAppModels.Models
{
    [Table("Countries")]
    public class Country
    {
        [Key]
        public int CountryId { get; set; }
        public string Symbol { get; set; }
        public string NamePL { get; set; }
        public string NameENG { get; set; }
        public string Info { get; set; }
        public bool UE { get; set; }
    }
}
