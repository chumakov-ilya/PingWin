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
            this.Property(t => t.ShortData)
                .HasMaxLength(256);

            // Table & Column Mappings
            this.ToTable("Log");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.ShortData).HasColumnName("ShortData");
            this.Property(t => t.FullData).HasColumnName("FullData");
            this.Property(t => t.StackTrace).HasColumnName("StackTrace");
            this.Property(t => t.JobRecordId).HasColumnName("JobRecordId");
            this.Property(t => t.DateTime).HasColumnName("DateTime");

            // Relationships
            this.HasRequired(t => t.JobRecord)
                .WithMany(t => t.Logs)
                .HasForeignKey(d => d.JobRecordId);

        }
    }
}
