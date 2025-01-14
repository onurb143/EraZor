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
        public required string Status { get; set; }

        [Required(ErrorMessage = "Serial number is required.")]
        [MaxLength(18, ErrorMessage = "Serial number cannot exceed 18 characters.")]
        public required string SerialNumber { get; set; }

        [MaxLength(24, ErrorMessage = "Manufacturer cannot exceed 24 characters.")]
        public required string Manufacturer { get; set; }

        [Required(ErrorMessage = "Wipe method name is required.")]
        public required string WipeMethodName { get; set; }

        [Required(ErrorMessage = "OverWritePasses is required.")]
        public int OverwritePasses { get; set; }

        [Required(ErrorMessage = "Performed by is required.")]
        public required string PerformedBy { get; set; }
    }
}



