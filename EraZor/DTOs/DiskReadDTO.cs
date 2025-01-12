namespace EraZor.DTOs
{
    /// DTO til læsning af disk-data. Bruges til at sende data fra API'et til klienten.
    public class DiskReadDto
    {
        /// Unik identifikator for disken.
        public int DiskID { get; set; }

        /// Diskens type (f.eks. SSD, HDD). 
        public required string Type { get; set; }

        /// Diskens kapacitet i GB. Skal være et positivt tal.
        public int Capacity { get; set; }

        /// Diskens filsti.
        public required string Path { get; set; }

        /// Diskens serienummer
        public required string SerialNumber { get; set; }

        /// Diskens producent (f.eks. Samsung, Seagate). 
        public required string Manufacturer { get; set; }
    }
}
