namespace EraZor.Data;
using EraZor.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }

    public DbSet<LogEntry> LogEntries { get; set; }
    public DbSet<WipeJob> WipeJobs { get; set; }
    public DbSet<Disk> Disks { get; set; }
    public DbSet<WipeMethod> WipeMethods { get; set; }
    public DbSet<IdentityUser> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Relation mellem WipeJob og WipeMethod
        modelBuilder.Entity<WipeJob>()
            .HasOne(wj => wj.WipeMethod)
            .WithMany(wm => wm.WipeJobs)
            .HasForeignKey(wj => wj.WipeMethodId);

        // Relation mellem WipeJob og LogEntry
        modelBuilder.Entity<WipeJob>()
            .HasMany(wj => wj.LogEntries)
            .WithOne(le => le.WipeJob)
            .HasForeignKey(le => le.WipeJobId);

        // Relation mellem WipeJob og User
        modelBuilder.Entity<WipeJob>()
            .HasOne(w => w.User)
            .WithMany() // Ingen navigation i IdentityUser
            .HasForeignKey(w => w.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Relation mellem WipeJob og Disk
        modelBuilder.Entity<WipeJob>()
            .HasOne(wj => wj.Disk)
            .WithMany(d => d.WipeJobs)
            .HasForeignKey(wj => wj.DiskId);
    }
}