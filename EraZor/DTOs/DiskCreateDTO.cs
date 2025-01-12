using System.ComponentModel.DataAnnotations;

namespace EraZor.DTOs
{
    /// DTO til oprettelse af en ny disk. Bruges til at modtage data fra klienten ved POST-anmodninger.
    public class DiskCreateDto
    {
        /// Diskens type. Kræves og kan ikke overstige 50 tegn.
        [Required(ErrorMessage = "Type is required.")]
        [StringLength(50, ErrorMessage = "Type cannot exceed 50 characters.")]
        public required string Type { get; set; }

        /// Diskens kapacitet i GB. Skal være større end 0.
        [Range(1, int.MaxValue, ErrorMessage = "Capacity must be greater than 0.")]
        public int Capacity { get; set; }

        /// Diskens filsti. Maksimal længde er 8 tegn.
        [Required(ErrorMessage = "Path is required.")]
        [MaxLength(8, ErrorMessage = "Path cannot exceed 8 characters.")]
        public required string Path { get; set; }

        /// Diskens serienummer. Kræves og kan ikke overstige 18 tegn.
        [Required(ErrorMessage = "Serial number is required.")]
        [MaxLength(18, ErrorMessage = "Serial number cannot exceed 18 characters.")]
        public required string SerialNumber { get; set; }

        /// Diskens producent. Maksimal længde er 24 tegn.
        [Required(ErrorMessage = "Manufacturer is required.")]
        [MaxLength(24, ErrorMessage = "Manufacturer cannot exceed 24 characters.")]
        public required string Manufacturer { get; set; }
    }
}
