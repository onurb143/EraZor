using System.ComponentModel.DataAnnotations;

namespace EraZor.DTOs
{
    /// DTO til oprettelse af en ny disk. Bruges til at modtage data fra klienten ved POST-anmodninger.
    public class DiskCreateDto
    {
        /// Diskens type. Kræves og kan ikke overstige 50 tegn.
        [Required(ErrorMessage = "Type is required.")]
        [StringLength(50, ErrorMessage = "Type cannot exceed 50 characters.")]
        public string Type { get; set; } = string.Empty;

        /// Diskens kapacitet i GB. Skal være større end 0.
        [Range(1, int.MaxValue, ErrorMessage = "Capacity must be greater than 0.")]
        public int Capacity { get; set; }

        /// Diskens filsti. Maksimal længde er 8 tegn.
        [Required(ErrorMessage = "Path is required.")]
        [MaxLength(8, ErrorMessage = "Path cannot exceed 8 characters.")]
        public string Path { get; set; } = string.Empty;

        /// Diskens serienummer. Kræves og kan ikke overstige 18 tegn.
        [Required(ErrorMessage = "Serial number is required.")]
        [MaxLength(18, ErrorMessage = "Serial number cannot exceed 18 characters.")]
        public string SerialNumber { get; set; } = string.Empty;

        /// Diskens producent. Maksimal længde er 24 tegn.
        [Required(ErrorMessage = "Manufacturer is required.")]
        [MaxLength(24, ErrorMessage = "Manufacturer cannot exceed 24 characters.")]
        public string Manufacturer { get; set; } = string.Empty;
    }
}
