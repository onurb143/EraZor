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
        [StringLength(30, ErrorMessage = "Name cannot exceed 30 characters.")] // Sætter en maksimal længde for navn
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")] // Maksimal længde for beskrivelse
        public string Description { get; set; } = string.Empty;

        public int OverwritePass { get; set; }

        // Navigation property til relationen med WipeJobs
        public ICollection<WipeJob> WipeJobs { get; set; } = new List<WipeJob>();
    }
}
