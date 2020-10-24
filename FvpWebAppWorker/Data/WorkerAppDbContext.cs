using FvpWebAppModels.Models;
using FvpWebAppWorker.Models.Configuration;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace FvpWebAppWorker.Data
{
    public class WorkerAppDbContext : DbContext
    {
        public WorkerAppDbContext(DbContextOptions<WorkerAppDbContext> options)
            : base(options)
        {
        }
        public DbSet<Document> Documents { get; set; }
        public DbSet<DocumentVat> DocumentVats { get; set; }
        public DbSet<Source> Sources { get; set; }
        public DbSet<SourceType> SourceTypes { get; set; }
        public DbSet<Target> Targets { get; set; }
        public DbSet<TaskTicket> TaskTickets { get; set; }
        public DbSet<Contractor> Contractors { get; set; }
        public DbSet<Country> Countries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new DocumentModelConfiguration());
            modelBuilder.ApplyConfiguration(new DocumentVatModelConfiguration());
        }
        public void DetachAllEntities()
        {
            var changedEntriesCopy = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added ||
                            e.State == EntityState.Modified ||
                            e.State == EntityState.Deleted)
                .ToList();

            foreach (var entry in changedEntriesCopy)
                entry.State = EntityState.Detached;
        }
    }
}
