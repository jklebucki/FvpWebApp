using FvpWebAppModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FvpWebApp.Models
{
    public class DocumentFullView
    {
        public DocumentFullView()
        {
            Contractors = new List<Contractor>();
        }

        public Document Document { get; set; }
        public List<Contractor> Contractors { get; set; }

    }
}
