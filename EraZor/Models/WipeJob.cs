using EraZor.Models;

public class WipeJob
{
    public int WipeJobId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string Status { get; set; }

    public int DiskId { get; set; }
    public Disk Disk { get; set; }

    public string UserId { get; set; }
    public User User { get; set; }

    public int WipeMethodId { get; set; }
    public WipeMethod WipeMethod { get; set; }

    // Navigationsproperty for LogEntries
    public ICollection<LogEntry> LogEntries { get; set; } = new List<LogEntry>();
}

