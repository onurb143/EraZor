using EraZor.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace EraZor.Data;

public class DataContext : IdentityDbContext<IdentityUser>
{
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }

    // Kortlægning af modeller til databasetabeller
    public DbSet<LogEntry> LogEntries { get; set; }
    public DbSet<WipeJob> WipeJobs { get; set; }
    public DbSet<Disk> Disks { get; set; }
    public DbSet<WipeMethod> WipeMethods { get; set; }
    public DbSet<WipeReport> WipeReports { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Sikrer at Identity fungerer
        base.OnModelCreating(modelBuilder);

        // Custom konfiguration for WipeReports (uden primær nøgle)
        modelBuilder.Entity<WipeReport>()
            .ToView("WipeReports")
            .HasNoKey();

        // Konfiguration for WipeJob
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

        // Disk-konfiguration (unik SerialNumber)
        modelBuilder.Entity<Disk>()
            .HasIndex(d => d.SerialNumber)
            .IsUnique();

        // Seed data for WipeMethod
        modelBuilder.Entity<WipeMethod>().HasData(
            new WipeMethod { WipeMethodID = 1, Name = "DoD 5220.22-M", OverwritePass = 3, Description = "Standard DoD wiping method with 3 passes" },
            new WipeMethod { WipeMethodID = 2, Name = "NIST 800-88 Clear", OverwritePass = 1, Description = "NIST standard for clearing data with 1 pass" },
            new WipeMethod { WipeMethodID = 3, Name = "NIST 800-88 Purge", OverwritePass = 1, Description = "NIST standard for purging data with 1 pass" },
            new WipeMethod { WipeMethodID = 4, Name = "Gutmann", OverwritePass = 35, Description = "Highly secure method with 35 overwrite passes" },
            new WipeMethod { WipeMethodID = 5, Name = "Random Data", OverwritePass = 1, Description = "Single pass of random data" },
            new WipeMethod { WipeMethodID = 6, Name = "Write Zero", OverwritePass = 1, Description = "Single pass of zeroes" },
            new WipeMethod { WipeMethodID = 7, Name = "Write One", OverwritePass = 1, Description = "Single pass of ones" },
            new WipeMethod { WipeMethodID = 8, Name = "Schneider", OverwritePass = 7, Description = "Custom 7-pass wiping method" },
            new WipeMethod { WipeMethodID = 9, Name = "Bruce Force", OverwritePass = 10, Description = "Secure 10-pass overwrite method" },
            new WipeMethod { WipeMethodID = 10, Name = "Quick Format", OverwritePass = 1, Description = "Fast format with 1 pass" },
            new WipeMethod { WipeMethodID = 11, Name = "Full Format", OverwritePass = 1, Description = "Complete format with 1 pass" }
        );
    }
}



