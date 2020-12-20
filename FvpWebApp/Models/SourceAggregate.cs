using System.Collections.Generic;
using FvpWebAppModels.Models;

namespace FvpWebApp.Models
{
    public class SourceAggregate
    {
        public Source Source { get; set; }
        public TargetDocumentSettings TargetDocumentSettings { get; set; }
    }
}