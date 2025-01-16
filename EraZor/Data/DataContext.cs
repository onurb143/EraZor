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
    public virtual DbSet<Disk> Disks { get; set; }
    public virtual DbSet<WipeMethod> WipeMethods { get; set; }

    // WipeReport er et view, derfor er det også kortlagt som en DbSet
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
            .ToView("WipeReports") // Angiver, at WipeReport er et view
            .HasNoKey(); // Angiver, at view’et ikke har en primær nøgle

        // Konfiguration for Disk (unik SerialNumber)
        modelBuilder.Entity<Disk>()
            .HasIndex(d => d.SerialNumber)
            .IsUnique();

        // Seed data for WipeMethod
        modelBuilder.Entity<WipeMethod>().HasData(
            new WipeMethod { WipeMethodID = 1, Name = "Secure Erase", OverwritePass = 1, Description = "Sikker metode, der udføres på hardware-niveau via SSD-controlleren. Ideel til SSD'er og NVMe. Ikke ISO-certificeret." },
            new WipeMethod { WipeMethodID = 2, Name = "Zero Fill", OverwritePass = 1, Description = "Overskriver med nulværdier i ét gennemløb. Velegnet til HDD'er, mindre egnet til SSD'er pga. wear leveling. Ikke ISO-certificeret." },
            new WipeMethod { WipeMethodID = 3, Name = "Random Fill", OverwritePass = 1, Description = "Overskriver med tilfældige data i ét gennemløb. Velegnet til HDD'er, mindre egnet til SSD'er. Ikke ISO-certificeret." },
            new WipeMethod { WipeMethodID = 4, Name = "Gutmann Method", OverwritePass = 35, Description = "Avanceret metode med 35 gennemløb designet til ældre HDD'er. Ikke egnet til moderne HDD'er, SSD'er eller NVMe. Ikke ISO-certificeret." },
            new WipeMethod { WipeMethodID = 5, Name = "Random Data", OverwritePass = 3, Description = "Overskriver med tilfældige data i 3 gennemløb. Velegnet til HDD'er, mindre egnet til SSD'er. Ikke ISO-certificeret." },
            new WipeMethod { WipeMethodID = 6, Name = "Write Zero", OverwritePass = 1, Description = "Skriver nulværdier i ét gennemløb. God til HDD'er, mindre effektiv på SSD'er. Ikke ISO-certificeret." },
            new WipeMethod { WipeMethodID = 7, Name = "Schneier Method", OverwritePass = 7, Description = "Metode med 7 gennemløb, som er sikker og velegnet til HDD'er. Overkill for SSD'er og NVMe. Ikke ISO-certificeret." },
            new WipeMethod { WipeMethodID = 8, Name = "HMG IS5 (Enhanced)", OverwritePass = 3, Description = "Standardiseret metode med 3 gennemløb. Velegnet til HDD'er. Ikke egnet til SSD'er eller NVMe. Ikke ISO-certificeret." },
            new WipeMethod { WipeMethodID = 9, Name = "Peter Gutmann's Method", OverwritePass = 35, Description = "Ekstremt sikker metode med 35 gennemløb, designet til ældre HDD'er. Ikke egnet til SSD'er eller NVMe. Ikke ISO-certificeret." },
            new WipeMethod { WipeMethodID = 10, Name = "Single Pass Zeroing", OverwritePass = 1, Description = "Hurtig metode med ét gennemløb af nulværdier. Velegnet til HDD'er, men mindre effektiv for SSD'er pga. wear leveling. Ikke ISO-certificeret." },
            new WipeMethod { WipeMethodID = 11, Name = "DoD 5220.22-M (E)", OverwritePass = 4, Description = "DoD-standard med 4 gennemløb. Velegnet til HDD'er, mindre relevant for SSD'er. Ikke ISO-certificeret." },
            new WipeMethod { WipeMethodID = 12, Name = "ISO/IEC 27040", OverwritePass = 1, Description = "ISO-standardiseret metode med ét gennemløb af nulværdier. Ideel til SSD'er, NVMe og HDD'er. ISO-certificeret." }
        );

    }
}
