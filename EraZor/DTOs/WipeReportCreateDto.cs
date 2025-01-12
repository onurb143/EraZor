using System.ComponentModel.DataAnnotations;

namespace EraZor.DTO
{
    public class WipeReportCreateDto
    {
        /// Starttidspunkt for sletningsjobbet.
        [Required(ErrorMessage = "Start time is required.")]
        public DateTime StartTime { get; set; }

        /// Sluttidspunkt for sletningsjobbet.
        [Required(ErrorMessage = "End time is required.")]
        public DateTime EndTime { get; set; }

        /// Status for sletningen (f.eks. "Completed", "Failed").
        [Required(ErrorMessage = "Status is required.")]
        public required string Status { get; set; }

        /// Serienummeret for den disk, der er blevet slettet.
        [Required(ErrorMessage = "Serial number is required.")]
        [MaxLength(18, ErrorMessage = "Serial number cannot exceed 18 characters.")]
        public required string SerialNumber { get; set; }

        /// Producenten af disken.
        [MaxLength(24, ErrorMessage = "Manufacturer cannot exceed 24 characters.")]
        public required string Manufacturer { get; set; }

        /// Navnet på den slettemetode, der blev brugt.
        [Required(ErrorMessage = "Wipe method name is required.")]
        public required string WipeMethodName { get; set; }

        /// Antal overskrivninger, der blev udført under sletningen.
        [Range(1, int.MaxValue, ErrorMessage = "Overwrite passes must be greater than 0.")]
        public int OverwritePasses { get; set; }

        /// Brugerens navn, som udførte sletningen.
        [Required(ErrorMessage = "Performed by is required.")]
        public required string PerformedBy { get; set; }
    }
}
