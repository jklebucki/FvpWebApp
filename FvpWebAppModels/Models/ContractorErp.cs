using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FvpWebAppModels.Models
{
    [Table("ContractorsErp")]
    public class ContractorErp
    {
        [Key]
        public int ContractorErpId { get; set; }
    }
}