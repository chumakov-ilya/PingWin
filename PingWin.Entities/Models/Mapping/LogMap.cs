using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace PingWin.Entities.Models.Mapping
{
    public class LogMap : EntityTypeConfiguration<Log>
    {
        public LogMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Message)
                .HasMaxLength(256);

            // Table & Column Mappings
            this.ToTable("Log");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Result).HasColumnName("Result");
            this.Property(t => t.Message).HasColumnName("Message");
            this.Property(t => t.Details).HasColumnName("Details");
            this.Property(t => t.JobId).HasColumnName("JobId");
            this.Property(t => t.DateTime).HasColumnName("DateTime");

            // Relationships
            this.HasRequired(t => t.JobRecord)
                .WithMany(t => t.Logs)
                .HasForeignKey(d => d.JobId);
		}
    }
}
