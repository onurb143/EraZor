namespace EraZor.DTOs
{
    /// DTO til læsning af disk-data. Bruges til at sende data fra API'et til klienten.
    public class DiskReadDto
    {
        /// Unik identifikator for disken.
        public int DiskID { get; set; }

        /// Diskens type (f.eks. SSD, HDD).
        public string Type { get; set; } = string.Empty;

        /// Diskens kapacitet i GB.
        public int Capacity { get; set; }

        /// Diskens filsti.
        public string Path { get; set; } = string.Empty;

        /// Diskens serienummer.
        public string SerialNumber { get; set; } = string.Empty;

        /// Diskens producent (f.eks. Samsung, Seagate).
        public string Manufacturer { get; set; } = string.Empty;
    }
}
