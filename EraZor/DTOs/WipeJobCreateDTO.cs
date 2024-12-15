namespace EraZor.DTOs
{
    public class WipeJobCreateDto
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Status { get; set; }
        public int DiskId { get; set; }
        public int WipeMethodId { get; set; }
    }
}
