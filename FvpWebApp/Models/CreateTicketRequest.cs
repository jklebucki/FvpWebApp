using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FvpWebApp.Models
{
    public class CreateTicketRequest
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int SourceId { get; set; }
    }
}
