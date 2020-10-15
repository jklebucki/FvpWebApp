using FvpWebAppModels;

namespace FvpWebAppWorker.Services
{
    public class ContractorServiceResponse
    {
        public ContractorStatus ContractorStatus { get; set; }
        public bool Exist { get; set; }
    }
}