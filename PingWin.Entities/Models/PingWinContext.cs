using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using PingWin.Entities.Models.Mapping;

namespace PingWin.Entities.Models
{
    public partial class PingWinContext : DbContext, IPingWinContext
    {
        static PingWinContext()
        {
            Database.SetInitializer<PingWinContext>(null);
        }

        public PingWinContext()
            : base("Name=PingWinContext")
        {
        }

        public IDbSet<JobRecord> JobRecords { get; set; }
        public IDbSet<Log> Logs { get; set; }
        public IDbSet<SystemLog> SystemLogs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new JobRecordMap());
            modelBuilder.Configurations.Add(new LogMap());
            modelBuilder.Configurations.Add(new SystemLogMap());
        }
    }
}
