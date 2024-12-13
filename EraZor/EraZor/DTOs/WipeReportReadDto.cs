namespace EraZor.DTO
{
    public class WipeReportReadDto
    {
        public int WipeJobId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public required string Status { get; set; }
        public required string DiskType { get; set; }
        public int Capacity { get; set; }
        public required string SerialNumber { get; set; }
        public required string Manufacturer { get; set; }
        public required string WipeMethodName { get; set; }
        public required int OverwritePasses { get; set; }
        public required string PerformedBy { get; set; } 
    }
}



