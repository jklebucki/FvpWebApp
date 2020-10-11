using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FvpWebAppModels.Models
{
    [Table("TaskTicketsQueue")]
    public class TaskTicket
    {

        [Key]
        public int TaskTicketId { get; set; }
        public int SourceId { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public bool Done { get; set; }
    }
}