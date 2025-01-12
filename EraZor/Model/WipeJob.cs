using EraZor.Model;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;

public class WipeJob
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int WipeJobId { get; set; } // Primær nøgle, auto-genereret af databasen.

    public DateTime StartTime { get; set; } // Tidspunkt for, hvornår sletningen startede.
    public DateTime EndTime { get; set; } // Tidspunkt for, hvornår sletningen blev afsluttet.
    public string? Status { get; set; } // Status for sletningsjobbet (f.eks. "Completed", "Failed").

    public int DiskId { get; set; } // Fremmednøgle til den disk, der blev slettet.
    public int WipeMethodId { get; set; } // Fremmednøgle til den slettemetode, der blev anvendt.

    // Navigation property til disken, der blev slettet.
    [ForeignKey("DiskId")]
    [JsonIgnore] // Undgår cyklisk serialisering ved JSON-konvertering.
    public virtual Disk Disk { get; set; } = null!; // Non-nullable for at sikre, at relationen altid eksisterer.

    // Navigation property til den slettemetode, der blev anvendt.
    [ForeignKey("WipeMethodId")]
    [JsonIgnore] // Undgår cyklisk serialisering ved JSON-konvertering.
    public virtual WipeMethod WipeMethod { get; set; } = null!; // Non-nullable for at sikre, at relationen altid eksisterer.

    // Fremmednøgle til den bruger, der udførte sletningen (valgfrit).
    public string? PerformedByUserId { get; set; } // ID for den bruger, der udførte jobben.
    public virtual IdentityUser? PerformedByUser { get; set; } // Navigation property til IdentityUser.
}
