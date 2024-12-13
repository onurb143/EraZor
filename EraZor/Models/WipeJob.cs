using EraZor.Models;
using Microsoft.AspNetCore.Identity;

public class WipeJob
{
    public int WipeJobId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string Status { get; set; }
    public int? DiskId { get; set; } // Fremmednøgle til Disk
    public int? WipeMethodId { get; set; } // Fremmednøgle til WipeMethod
    public string UserId { get; set; } // Fremmednøgle til IdentityUser

    // Navigation properties
    public Disk Disk { get; set; }
    public WipeMethod WipeMethod { get; set; }
    public IdentityUser User { get; set; }
    public ICollection<LogEntry> LogEntries { get; set; }
}
