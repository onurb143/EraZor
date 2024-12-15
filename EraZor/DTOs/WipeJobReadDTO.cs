namespace EraZor.DTOs
{
    public class WipeJobReadDTO
    {
        public int WipeJobId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Status { get; set; }
        public string DiskSerialNumber { get; set; }
        public string WipeMethodName { get; set; }
    }

}
