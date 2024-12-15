namespace EraZor.DTOs
{
    public class DiskReadDto
    {
        public int DiskID { get; set; }
        public string Type { get; set; }
        public int Capacity { get; set; }
        public string? Path { get; set; }
        public string? SerialNumber { get; set; }
        public string? Manufacturer { get; set; }
    }
}
