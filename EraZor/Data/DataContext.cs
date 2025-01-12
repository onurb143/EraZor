using EraZor.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EraZor.Data;

public class DataContext : IdentityDbContext<IdentityUser>
{
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }

    // Kortlægning af modeller til databasetabeller
    public DbSet<WipeJob> WipeJobs { get; set; }
    public DbSet<Disk> Disks { get; set; }
    public DbSet<WipeMethod> WipeMethods { get; set; }
    public DbSet<WipeReport> WipeReports { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Konfiguration for WipeJob relationer
        modelBuilder.Entity<WipeJob>()
            .HasOne(wj => wj.PerformedByUser) // Relation til IdentityUser
            .WithMany() // Ingen omvendt navigation (IdentityUser behøver ikke en liste af jobs)
            .HasForeignKey(wj => wj.PerformedByUserId) // Fremmednøgle
            .OnDelete(DeleteBehavior.Restrict); // Undgå kaskadesletning

        modelBuilder.Entity<WipeJob>()
            .HasOne(wj => wj.WipeMethod)
            .WithMany(wm => wm.WipeJobs)
            .HasForeignKey(wj => wj.WipeMethodId);

        modelBuilder.Entity<WipeJob>()
            .HasOne(wj => wj.Disk)
            .WithMany(d => d.WipeJobs)
            .HasForeignKey(wj => wj.DiskId);

        // Konfiguration for WipeReport (uden primær nøgle, baseret på en View)
        modelBuilder.Entity<WipeReport>()
            .ToView("WipeReports")
            .HasNoKey();

        // Konfiguration for Disk (unik SerialNumber)
        modelBuilder.Entity<Disk>()
            .HasIndex(d => d.SerialNumber)
            .IsUnique();

        // Seed data for WipeMethod
        modelBuilder.Entity<WipeMethod>().HasData(
        new WipeMethod { WipeMethodID = 1, Name = "Secure Erase", OverwritePass = 3, Description = "Standard DoD-sletning med 3 gennemløb. Ikke ISO-certificeret." },
        new WipeMethod { WipeMethodID = 2, Name = "Zero Fill", OverwritePass = 1, Description = "Skriver nulværdier i ét gennemløb. Ikke ISO-certificeret." },
        new WipeMethod { WipeMethodID = 3, Name = "Random Fill", OverwritePass = 1, Description = "Skriver tilfældige data i ét gennemløb. Ikke ISO-certificeret." },
        new WipeMethod { WipeMethodID = 4, Name = "Gutmann Method", OverwritePass = 35, Description = "Meget sikker metode med 35 gennemløb. Ikke ISO-certificeret." },
        new WipeMethod { WipeMethodID = 5, Name = "Random Data", OverwritePass = 3, Description = "Skriver tilfældige data i 3 gennemløb. Ikke ISO-certificeret." },
        new WipeMethod { WipeMethodID = 6, Name = "Write Zero", OverwritePass = 1, Description = "Skriver nulværdier i ét gennemløb. Ikke ISO-certificeret." },
        new WipeMethod { WipeMethodID = 7, Name = "Schneier Method", OverwritePass = 7, Description = "Sikker metode med 7 gennemløb. Ikke ISO-certificeret." },
        new WipeMethod { WipeMethodID = 8, Name = "HMG IS5 (Enhanced)", OverwritePass = 3, Description = "Sletning med 3 gennemløb efter britisk standard. Ikke ISO-certificeret." },
        new WipeMethod { WipeMethodID = 9, Name = "Peter Gutmann's Method", OverwritePass = 35, Description = "Ekstremt sikker metode med 35 gennemløb. Ikke ISO-certificeret." },
        new WipeMethod { WipeMethodID = 10, Name = "Single Pass Zeroing", OverwritePass = 1, Description = "Hurtig sletning med ét gennemløb af nulværdier. Ikke ISO-certificeret." },
        new WipeMethod { WipeMethodID = 11, Name = "DoD 5220.22-M (E)", OverwritePass = 4, Description = "Forbedret DoD-sletning med 4 gennemløb. Ikke ISO-certificeret." },
        new WipeMethod { WipeMethodID = 12, Name = "ISO/IEC 27040", OverwritePass = 1, Description = "ISO-standard med ét gennemløb af nulværdier. ISO-certificeret." }

        );
    }
}
