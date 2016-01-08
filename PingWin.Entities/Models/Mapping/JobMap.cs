using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace PingWin.Entities.Models.Mapping
{
    public class JobMap : EntityTypeConfiguration<Job>
    {
        public JobMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Job");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Name).HasColumnName("Name");
        }
    }
}
