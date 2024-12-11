using EraZor.Models;
using Microsoft.AspNetCore.Identity;

public class WipeJob
{
    public int WipeJobId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string Status { get; set; }
    public int DiskId { get; set; }
    public int MethodId { get; set; }
    public int WipeMethodId { get; set; }
    public string UserId { get; set; } // Fremmednøgle

    public Disk Disk { get; set; }
    public WipeMethod WipeMethod { get; set; }
    public ICollection<LogEntry> LogEntries { get; set; }

    // Navigation property til IdentityUser
    public IdentityUser User { get; set; }
}

