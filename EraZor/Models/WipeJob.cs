namespace EraZor.Models
{
    public class WipeJob
    {
        public int WipeJobId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Status { get; set; }
        public int DiskId { get; set; }
        public int MethodId { get; set; }
        public int UserId { get; set; }

        public Disk Disk { get; set; }
        public WipeMethod Method { get; set; }
        public User User { get; set; }
    }
}