using FvpWebAppModels.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FvpWebApp.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new DocumentModelConfiguration());
            modelBuilder.ApplyConfiguration(new DocumentVatModelConfiguration());
            modelBuilder.ApplyConfiguration(new SourceModelConfiguration());
            modelBuilder.ApplyConfiguration(new TaskTicketModelConfiguration());
            modelBuilder.ApplyConfiguration(new TargetModelConfiguration());
        }
    }
}
