using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace EraZor.Models
{
    public class Disk
    {
        [Key] // Primær nøgle
        public int DiskID { get; set; }
      
        [MaxLength(5)]
        public string Type { get; set; } = string.Empty;

        [Range(1, int.MaxValue, ErrorMessage = "Capacity must be greater than 0.")]
        public int Capacity { get; set; }

        [MaxLength(8)]
        public required string Path { get; set; }

        [MaxLength(18)]
        public required string SerialNumber { get; set; }

        [MaxLength(24)]
        public string? Manufacturer { get; set; }

        [JsonIgnore] // Undgå serialization af WipeJobs i JSON
        public ICollection<WipeJob> WipeJobs { get; set; } = new List<WipeJob>();
    }
}



