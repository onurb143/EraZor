using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace EraZor.Model
{
    /// Domænemodel for diske. Mapper til Disk-tabellen i databasen.
    public class Disk
    {
        /// Unik identifikator for disken. Bruges som primær nøgle.
        [Key]
        public int DiskID { get; set; }

        /// Diskens type (f.eks. SSD, HDD). Kræves og kan ikke overstige 50 tegn.
        [Required(ErrorMessage = "Type is required.")]
        [StringLength(50, ErrorMessage = "Type cannot exceed 50 characters.")]
        public string Type { get; set; } = string.Empty;

        /// Diskens kapacitet i GB. Skal være større end 0.
        [Range(1, int.MaxValue, ErrorMessage = "Capacity must be greater than 0.")]
        public int Capacity { get; set; }

        /// Diskens filsti. Maksimal længde er 8 tegn.
        [MaxLength(8)]
        public string Path { get; set; } = string.Empty;

        /// Diskens serienummer. Kræves og kan ikke overstige 18 tegn.
        [MaxLength(18)]
        public string SerialNumber { get; set; } = string.Empty;

        /// Diskens producent. Maksimal længde er 24 tegn.
        [MaxLength(24)]
        public string Manufacturer { get; set; } = string.Empty;

        /// Liste af WipeJobs, der refererer til denne disk. Bruges til at opretholde relationen.
        [JsonIgnore]
        public ICollection<WipeJob> WipeJobs { get; set; } = new List<WipeJob>();
    }
}
