using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using TfsMigration.Infrastructure;
using TfsMigrator.Data;

namespace TfsMigrator.Infrastructure
{
    public class DataContext : DbContext
    {
        public DataContext(AppSettings appSettings) : base(new DbContextOptionsBuilder().UseSqlServer(appSettings.DatabaseConnection).Options)
        {
            Database.EnsureCreated();
        }

        public DbSet<WorkItemMigration> WorkItemMigrations { get; set; }

        public DbSet<Attachment> Attachments { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WorkItemMigration>().HasKey(x => x.OriginalId);

            modelBuilder.Entity<WorkItemMigration>().Property(x => x.OriginalId).ValueGeneratedNever();
            
            base.OnModelCreating(modelBuilder);
        }
    }
}