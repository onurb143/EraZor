namespace EraZor.DTO
{
    public class WipeReportReadDto
    {
        public int WipeJobId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public  string Status { get; set; }
        public  string DiskType { get; set; }
        public int Capacity { get; set; }
        public  string SerialNumber { get; set; }
        public  string Manufacturer { get; set; }
        public  string WipeMethodName { get; set; }
        public  int OverwritePass { get; set; }
        public  string PerformedBy { get; set; } 
    }
}



