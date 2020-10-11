using System.Collections.Generic;
using FvpWebAppModels.Models;

namespace FvpWebAppWorker.Models
{
    public class ApiResponseContractor
    {
        public List<Contractor> Contractors { get; set; }
        public ApiStatus ApiStatus { get; set; }
        public string Message { get; set; }
    }
}