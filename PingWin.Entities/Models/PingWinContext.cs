using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using PingWin.Entities.Models.Mapping;

namespace PingWin.Entities.Models
{
    public partial class PingWinContext : DbContext
    {
        static PingWinContext()
        {
            Database.SetInitializer<PingWinContext>(null);
        }

        public PingWinContext()
            : base("Name=PingWinContext")
        {
        }

        public DbSet<JobRecord> JobRecords { get; set; }
        public DbSet<Log> Logs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new JobRecordMap());
            modelBuilder.Configurations.Add(new LogMap());
        }
    }
}
