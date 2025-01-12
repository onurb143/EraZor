namespace EraZor.DTO
{
    public class WipeReportReadDto
    {
        /// Unik identifikator for sletterapporten (fra WipeJob).
        public int WipeJobId { get; set; }

        /// Starttidspunkt for sletningen.
        public DateTime StartTime { get; set; }

        /// Sluttidspunkt for sletningen.
        public DateTime EndTime { get; set; }

        /// Status for sletningen (f.eks. "Completed", "Failed").
        public required string Status { get; set; }

        /// Diskens type (f.eks. SSD, HDD).
        public required string DiskType { get; set; }

        /// Diskens kapacitet i GB.
        public int Capacity { get; set; }

        /// Serienummeret på disken.
        public required string SerialNumber { get; set; }

        /// Producenten af disken.
        public required string Manufacturer { get; set; }

        /// Navnet på den slettemetode, der blev brugt.
        public required string WipeMethodName { get; set; }

        /// Antallet af overskrivninger, der blev udført.
        public required int OverwritePasses { get; set; }

        /// Brugerens navn, som udførte sletningen.
        public required string PerformedBy { get; set; } 
    }
}
