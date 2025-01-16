using System.ComponentModel.DataAnnotations;

namespace EraZor.DTO
{
    public class WipeReportCreateDto
    {
        [Required(ErrorMessage = "Start time is required.")]
        public DateTime StartTime { get; set; }

        [Required(ErrorMessage = "End time is required.")]
        public DateTime EndTime { get; set; }

        [Required(ErrorMessage = "Status is required.")]
        public string? Status { get; set; }

        [Required(ErrorMessage = "Serial number is required.")]
        [MaxLength(18, ErrorMessage = "Serial number cannot exceed 18 characters.")]
        public string? SerialNumber { get; set; }

        [MaxLength(24, ErrorMessage = "Manufacturer cannot exceed 24 characters.")]
        public string? Manufacturer { get; set; }

        [Required(ErrorMessage = "Disk type is required.")]
        [MaxLength(50, ErrorMessage = "Disk type cannot exceed 50 characters.")]
        public string? DiskType { get; set; }

        [Required(ErrorMessage = "Capacity is required.")]
        public int Capacity { get; set; } // Tilføjet Capacity

        [Required(ErrorMessage = "Wipe method name is required.")]
        public string? WipeMethodName { get; set; }

        [Required(ErrorMessage = "Overwrite passes are required.")]
        public int OverwritePasses { get; set; }

        [Required(ErrorMessage = "Performed by is required.")]
        public string? PerformedBy { get; set; }
    }
}




