using System;
using System.Collections.Generic;
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
        public TicketStatus TicketStatus { get; set; }
        public TicketType TicketType { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime StatusChangedAt { get; set; }
        public ICollection<Document> Documents { get; set; }
    }
}