using FvpWebAppModels.Models;
using FvpWebAppWorker.Models.Configuration;
using Microsoft.EntityFrameworkCore;

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
        public DbSet<TaskTicket> TaskTicketsQueue { get; set; }
        public DbSet<Contractor> Contractors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new DocumentModelConfiguration());
            modelBuilder.ApplyConfiguration(new DocumentVatModelConfiguration());
        }
    }
}
