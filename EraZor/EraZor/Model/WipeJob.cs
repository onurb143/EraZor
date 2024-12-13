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

    [Required] // Tilføjet for at sikre, at StartTime altid er tilgængelig.
    public DateTime StartTime { get; set; } // Tidspunkt for, hvornår sletningen startede.

    [Required] // Tilføjet for at sikre, at EndTime altid er tilgængelig.
    public DateTime EndTime { get; set; } // Tidspunkt for, hvornår sletningen blev afsluttet.

    [Required] // Status skal være obligatorisk
    public string Status { get; set; } = string.Empty; // Status for sletningsjobbet (f.eks. "Completed", "Failed").

    [Required] // Fremmednøgle til Disk og WipeMethod skal også være nødvendige, da vi har relationer.
    public int DiskId { get; set; } // Fremmednøgle til den disk, der blev slettet.

    [Required]
    public int WipeMethodId { get; set; } // Fremmednøgle til den slettemetode, der blev anvendt.

    // Navigation property til disken, der blev slettet.
    [JsonIgnore] // Undgår cyklisk serialisering ved JSON-konvertering.
    public virtual Disk Disk { get; set; } = null!; // Non-nullable for at sikre, at relationen altid eksisterer.

    // Navigation property til den slettemetode, der blev anvendt.
    [JsonIgnore] // Undgår cyklisk serialisering ved JSON-konvertering.
    public virtual WipeMethod WipeMethod { get; set; } = null!; // Non-nullable for at sikre, at relationen altid eksisterer.

    // Fremmednøgle til den bruger, der udførte sletningen
    [Required]
    public string PerformedByUserId { get; set; } = string.Empty; // ID for den bruger, der udførte jobben.

    [Required]
    public virtual IdentityUser PerformedByUser { get; set; } = null!;    // Navigation property til IdentityUser.
}
