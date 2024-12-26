using EraZor.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

public class WipeJob
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int WipeJobId { get; set; }  // Primær nøgle, auto-genereret

    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }


    [MaxLength(50)]
    public string? Status { get; set; }

    public int DiskId { get; set; }
    public int WipeMethodId { get; set; }

    // Navigation properties
    [ForeignKey("DiskId")]
    [JsonIgnore] // Forhindrer cyklus under serialisering
    public virtual Disk Disk { get; set; }

    [ForeignKey("WipeMethodId")]
    [JsonIgnore] // Forhindrer cyklus under serialisering
    public virtual WipeMethod WipeMethod { get; set; }

    public ICollection<LogEntry> LogEntries { get; set; } = new List<LogEntry>();
}
