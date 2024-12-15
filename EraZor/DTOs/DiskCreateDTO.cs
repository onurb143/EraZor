using System.ComponentModel.DataAnnotations;

namespace EraZor.DTOs
{
    public class DiskCreateDto
    {
        [Required]
        [MaxLength(50)]
        public string Type { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Capacity must be greater than 0.")]
        public int Capacity { get; set; }

        [MaxLength(200)]
        public string? Path { get; set; }

        [Required]
        [MaxLength(100)]
        public string? SerialNumber { get; set; }

        [MaxLength(100)]
        public string? Manufacturer { get; set; }
    }


}
