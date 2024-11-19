namespace EraZor.Data;
using EraZor.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;


public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }

    public DbSet<LogEntry> LogEntries { get; set; }
    public DbSet<WipeJob> WipeJobs { get; set; }
    public DbSet<Disk> Disks { get; set; }
    public DbSet<WipeMethod> WipeMethods { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<LogEntry>()
            .HasKey(le => le.LogID);

        modelBuilder.Entity<WipeMethod>()
            .HasKey(wm => wm.MethodID);

    }


}
