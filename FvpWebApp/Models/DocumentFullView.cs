using FvpWebAppModels.Models;
using System.Collections.Generic;

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
