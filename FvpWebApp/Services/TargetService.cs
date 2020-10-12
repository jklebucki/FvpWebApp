using FvpWebApp.Data;
using FvpWebApp.Services.Interfaces;
using FvpWebAppModels.Models;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Threading.Tasks;

namespace FvpWebApp.Services
{
    public class TargetService : ITargetService
    {
        readonly ILogger _logger;
        private ApplicationDbContext _dbContext { get; set; }
        public TargetService(ApplicationDbContext dbContext, ILogger logger)
        {
            _logger = logger;
            _dbContext = dbContext;
        }
        public async Task<Target> CreateTarget(Target target)
        {
            try
            {
                await _dbContext.AddAsync<Target>(target);
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.Error(ex.Message);
                return null;
            }

            return await Task.FromResult<Target>(target);
        }
        public async Task<Target> GetTarget(int targetId)
        {
            return await Task.FromResult<Target>(new Target());
        }
        public async Task UpdateTarget(Target target)
        {
            await Task.CompletedTask;
        }
        public async Task RemoveTarget(int targetId)
        {
            await Task.CompletedTask;
        }
    }
}