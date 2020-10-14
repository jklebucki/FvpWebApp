using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FvpWebAppModels.Models
{
    [Table("TaskTickets")]
    public class TaskTicket
    {
        [Key]
        public int TaskTicketId { get; set; }
        public int SourceId { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public TikcketStatus TikcketStatus { get; set; }
    }
}