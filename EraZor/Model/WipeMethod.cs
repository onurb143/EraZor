using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EraZor.Model
{
    public class WipeMethod
    {
        [Key] // Primærnøgle
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Sikrer auto-generering af ID
        public int WipeMethodID { get; set; }

        [Required] // Feltet er påkrævet
        [MaxLength(30)] // Sætter en maksimal længde for navn
        public string Name { get; set; } = string.Empty;

        [MaxLength(500)] // Beskrivelse kan være længere, men vi sætter en grænse
        public string Description { get; set; } = string.Empty;

        [Range(1, int.MaxValue, ErrorMessage = "OverwritePass skal være større end 0.")] // Validering for positive værdier
        public int OverwritePass { get; set; }

        // Navigation property til relationen med WipeJobs
        public ICollection<WipeJob> WipeJobs { get; set; } = new List<WipeJob>();
    }
}
