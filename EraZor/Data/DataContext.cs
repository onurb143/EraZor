namespace EraZor.Data;
using EraZor.Models;
using Microsoft.EntityFrameworkCore;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }

    public DbSet<LogEntry> LogEntries { get; set; }
    public DbSet<WipeJob> WipeJobs { get; set; }
    public DbSet<Disk> Disks { get; set; }
    public DbSet<WipeMethod> WipeMethods { get; set; }
    public DbSet<WipeReport> WipeReports { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<WipeReport>()
            .ToView("WipeReports")
            .HasNoKey();

        modelBuilder.Entity<WipeJob>()
            .HasOne(wj => wj.WipeMethod)
            .WithMany(wm => wm.WipeJobs)
            .HasForeignKey(wj => wj.WipeMethodId);

        modelBuilder.Entity<WipeJob>()
            .HasOne(wj => wj.Disk)
            .WithMany(d => d.WipeJobs)
            .HasForeignKey(wj => wj.DiskId);

        modelBuilder.Entity<WipeJob>()
            .HasMany(wj => wj.LogEntries)
            .WithOne(le => le.WipeJob)
            .HasForeignKey(le => le.WipeJobId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Disk>()
            .HasIndex(d => d.SerialNumber)
            .IsUnique();

        modelBuilder.Entity<WipeMethod>().HasData(
            new WipeMethod { WipeMethodID = 1, Name = "DoD 5220.22-M", OverwritePass = 3 },
            new WipeMethod { WipeMethodID = 2, Name = "NIST 800-88 Clear", OverwritePass = 1 },
            new WipeMethod { WipeMethodID = 3, Name = "NIST 800-88 Purge", OverwritePass = 1 },
            new WipeMethod { WipeMethodID = 4, Name = "Gutmann", OverwritePass = 35 },
            new WipeMethod { WipeMethodID = 5, Name = "Random Data", OverwritePass = 1 },
            new WipeMethod { WipeMethodID = 6, Name = "Write Zero", OverwritePass = 1 },
            new WipeMethod { WipeMethodID = 7, Name = "Write One", OverwritePass = 1 },
            new WipeMethod { WipeMethodID = 8, Name = "Schneider", OverwritePass = 7 },
            new WipeMethod { WipeMethodID = 9, Name = "Bruce Force", OverwritePass = 10 },
            new WipeMethod { WipeMethodID = 10, Name = "Quick Format", OverwritePass = 1 },
            new WipeMethod { WipeMethodID = 11, Name = "Full Format", OverwritePass = 1 }
        );
    }
}
