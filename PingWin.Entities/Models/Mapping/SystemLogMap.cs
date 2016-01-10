using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace PingWin.Entities.Models.Mapping
{
    public class SystemLogMap : EntityTypeConfiguration<SystemLog>
    {
        public SystemLogMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Message)
                .HasMaxLength(256);

            // Table & Column Mappings
            this.ToTable("SystemLog");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.DateTime).HasColumnName("DateTime");
            this.Property(t => t.Message).HasColumnName("Message");
            this.Property(t => t.FullData).HasColumnName("FullData");
            this.Property(t => t.StackTrace).HasColumnName("StackTrace");
        }
    }
}
