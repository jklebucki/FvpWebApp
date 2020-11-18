using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FvpWebApp.Models
{
    public class CreateTicetsResponse
    {
        public bool TicketsCreated { get; set; }
        public string Message { get; set; }
    }
}
