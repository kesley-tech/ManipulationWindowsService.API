using ManipulationWindowsService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ManipulationWindowsService.API.Domain.Persistence
{
    public class WinServiceDbContext : DbContext
    {
        public WinServiceDbContext(DbContextOptions<WinServiceDbContext> options) : base(options) 
        {
           
        }

        public DbSet<WinService> WinServiceList { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<WinService>(e =>
            {
                e.HasKey(key => key.Id);

                e.Property(data => data.Name)
                 .IsRequired()
                 .HasMaxLength(100)
                 .HasColumnType("varchar(100)");

                e.Property(data => data.Description)
                 .HasMaxLength(200)
                 .HasColumnType("varchar(200)");

                e.Property(data => data.StartupType)
                 .IsRequired()
                 .HasMaxLength(50)
                 .HasColumnType("varchar(50)");

                e.Property(data => data.Status)
                 .IsRequired()
                 .HasMaxLength(50)
                 .HasColumnType("varchar(50)");

                e.Property(data => data.LogOnAs)
                 .HasMaxLength(200)
                 .HasColumnType("varchar(200)");

                e.Property(data => data.IsDeleted)
                 .HasColumnType("bit");
            });

        }
    }
}
