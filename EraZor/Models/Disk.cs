using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace EraZor.Models
{
    public class Disk
    {
        [Key]
        public int DiskID { get; set; }

        [Required]
        [MaxLength(50)]
        public string Type { get; set; } = string.Empty;

        [Range(1, int.MaxValue, ErrorMessage = "Capacity must be greater than 0.")]
        public int Capacity { get; set; }

        [MaxLength(200)]
        public string? Path { get; set; }

        [MaxLength(100)]
        public string? SerialNumber { get; set; }

        [MaxLength(100)]
        public string? Manufacturer { get; set; }

        [JsonIgnore] // Undgå serialization af WipeJobs i JSON
        public ICollection<WipeJob> WipeJobs { get; set; } = new List<WipeJob>();
    }
}
