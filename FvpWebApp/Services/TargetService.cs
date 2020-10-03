using System.Threading.Tasks;
using FvpWebApp.Data;
using FvpWebAppModels.Models;
using FvpWebApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Serilog;

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
            throw new System.NotImplementedException();
        }
        public async Task UpdateTarget(Target target)
        {
            throw new System.NotImplementedException();
        }
        public async Task RemoveTarget(int targetId)
        {
            throw new System.NotImplementedException();
        }
    }
}