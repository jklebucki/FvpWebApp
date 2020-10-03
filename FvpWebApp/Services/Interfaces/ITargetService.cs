using System.Threading.Tasks;
using FvpWebAppModels.Models;

namespace FvpWebApp.Services.Interfaces
{
    public interface ITargetService
    {
        Task<Target> CreateTarget(Target target);
        Task<Target> GetTarget(int targetId);
        Task UpdateTarget(Target target);
        Task RemoveTarget(int targetId);

    }
}