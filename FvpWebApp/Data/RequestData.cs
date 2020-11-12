using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FvpWebApp.Data
{
    public class RequestData
    {
        public int ContractorId { get; set; }
        public int DocumentId { get; set; }
        public bool ChangeAllDocuments { get; set; }
    }
}
